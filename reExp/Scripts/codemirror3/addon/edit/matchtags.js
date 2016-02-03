(function() {
  "use strict";

  CodeMirror.defineOption("matchTags", false, function(cm, val, old) {
    if (old && old != CodeMirror.Init) {
      cm.off("cursorActivity", doMatchTags);
      cm.off("viewportChange", maybeUpdateMatch);
      clear(cm);
    }
    if (val) {
      cm.state.matchBothTags = typeof val == "object" && val.bothTags;
      cm.on("cursorActivity", doMatchTags);
      cm.on("viewportChange", maybeUpdateMatch);
      doMatchTags(cm);
    }
  });

  function clear(cm) {
    if (cm.state.tagHit) cm.state.tagHit.clear();
    if (cm.state.tagOther) cm.state.tagOther.clear();
    cm.state.tagHit = cm.state.tagOther = null;
  }

  function doMatchTags(cm) {
    cm.state.failedTagMatch = false;
    cm.operation(function() {
      clear(cm);
      if (cm.somethingSelected()) return;
      var cur = cm.getCursor(), range = cm.getViewport();
      range.from = Math.min(range.from, cur.line); range.to = Math.max(cur.line + 1, range.to);
      var match = CodeMirror.findMatchingTag(cm, cur, range);
      if (!match) return;
      if (cm.state.matchBothTags) {
        var hit = match.at == "open" ? match.open : match.close;
        if (hit) cm.state.tagHit = cm.markText(hit.from, hit.to, {className: "CodeMirror-matchingtag"});
      }
      var other = match.at == "close" ? match.open : match.close;
      if (other)
        cm.state.tagOther = cm.markText(other.from, other.to, {className: "CodeMirror-matchingtag"});
      else
        cm.state.failedTagMatch = true;
    });
  }

  function maybeUpdateMatch(cm) {
    if (cm.state.failedTagMatch) doMatchTags(cm);
  }

  CodeMirror.commands.toMatchingTag = function(cm) {
    var found = CodeMirror.findMatchingTag(cm, cm.getCursor());
    if (found) {
      var other = found.at == "close" ? found.open : found.close;
      if (other) cm.setSelection(other.to, other.from);
    }
  };
})();

(function () {
    var ie_lt8 = /MSIE \d/.test(navigator.userAgent) &&
      (document.documentMode == null || document.documentMode < 8);

    var Pos = CodeMirror.Pos;

    var matching = { "(": ")>", ")": "(<", "[": "]>", "]": "[<", "{": "}>", "}": "{<" };
    function findMatchingBracket(cm, where, strict) {
        var state = cm.state.matchBrackets;
        var maxScanLen = (state && state.maxScanLineLength) || 10000;

        var cur = where || cm.getCursor(), line = cm.getLineHandle(cur.line), pos = cur.ch - 1;
        var match = (pos >= 0 && matching[line.text.charAt(pos)]) || matching[line.text.charAt(++pos)];
        if (!match) return null;
        var forward = match.charAt(1) == ">", d = forward ? 1 : -1;
        if (strict && forward != (pos == cur.ch)) return null;
        var style = cm.getTokenTypeAt(Pos(cur.line, pos + 1));

        var stack = [line.text.charAt(pos)], re = /[(){}[\]]/;
        function scan(line, lineNo, start) {
            if (!line.text) return;
            var pos = forward ? 0 : line.text.length - 1, end = forward ? line.text.length : -1;
            if (line.text.length > maxScanLen) return null;
            if (start != null) pos = start + d;
            for (; pos != end; pos += d) {
                var ch = line.text.charAt(pos);
                if (re.test(ch) && cm.getTokenTypeAt(Pos(lineNo, pos + 1)) == style) {
                    var match = matching[ch];
                    if (match.charAt(1) == ">" == forward) stack.push(ch);
                    else if (stack.pop() != match.charAt(0)) return { pos: pos, match: false };
                    else if (!stack.length) return { pos: pos, match: true };
                }
            }
        }
        for (var i = cur.line, found, e = forward ? Math.min(i + 100, cm.lineCount()) : Math.max(-1, i - 100) ; i != e; i += d) {
            if (i == cur.line) found = scan(line, i, pos);
            else found = scan(cm.getLineHandle(i), i);
            if (found) break;
        }
        return {
            from: Pos(cur.line, pos), to: found && Pos(i, found.pos),
            match: found && found.match, forward: forward
        };
    }

    function matchBrackets(cm, autoclear) {
        // Disable brace matching in long lines, since it'll cause hugely slow updates
        var maxHighlightLen = cm.state.matchBrackets.maxHighlightLineLength || 1000;
        var found = findMatchingBracket(cm);
        if (!found || cm.getLine(found.from.line).length > maxHighlightLen ||
           found.to && cm.getLine(found.to.line).length > maxHighlightLen)
            return;

        var style = found.match ? "CodeMirror-matchingbracket" : "CodeMirror-nonmatchingbracket";
        var one = cm.markText(found.from, Pos(found.from.line, found.from.ch + 1), { className: style });
        var two = found.to && cm.markText(found.to, Pos(found.to.line, found.to.ch + 1), { className: style });
        // Kludge to work around the IE bug from issue #1193, where text
        // input stops going to the textare whever this fires.
        if (ie_lt8 && cm.state.focused) cm.display.input.focus();
        var clear = function () {
            cm.operation(function () { one.clear(); two && two.clear(); });
        };
        if (autoclear) setTimeout(clear, 800);
        else return clear;
    }

    var currentlyHighlighted = null;
    function doMatchBrackets(cm) {
        cm.operation(function () {
            if (currentlyHighlighted) { currentlyHighlighted(); currentlyHighlighted = null; }
            if (!cm.somethingSelected()) currentlyHighlighted = matchBrackets(cm, false);
        });
    }

    CodeMirror.defineOption("matchBrackets", false, function (cm, val, old) {
        if (old && old != CodeMirror.Init)
            cm.off("cursorActivity", doMatchBrackets);
        if (val) {
            cm.state.matchBrackets = typeof val == "object" ? val : {};
            cm.on("cursorActivity", doMatchBrackets);
        }
    });

    CodeMirror.defineExtension("matchBrackets", function () { matchBrackets(this, true); });
    CodeMirror.defineExtension("findMatchingBracket", function (pos, strict) {
        return findMatchingBracket(this, pos, strict);
    });
})();


