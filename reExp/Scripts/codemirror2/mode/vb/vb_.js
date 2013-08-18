CodeMirror.defineMode("vb", function(config) {
  var indentUnit = config.indentUnit;
  var curPunc;

  function wordRegexp(words) {
    return new RegExp("^(?:" + words.join("|") + ")$", "i");
  }
  var ops = wordRegexp(['if', 'then', 'for each', 'for', 'while', 'do', 'loop',
            'else', 'elseif', 'select', 'case', 'end select', 'with', 'end with',
            'until', 'next', 'step', 'to', 'in', 'end if', 'from', 'using', 'end using']);
  var keywords = wordRegexp(['empty', 'isempty', 'nothing', 'null', 'isnull', 'true', 'false',
            'set', 'call', 'imports', 'inherits', 'implements',
            'sub', 'end sub', 'function', 'end function', 'exit', 'exit function',
            'dim', 'Mod', 'In', 'private', 'public', 'protected', 'overrides', 'shared', 'const', 'from',
            'module', 'end module', 'class', 'end class', 'me', 'as', 'new', 'mybase', 'myclass',
            'namespace', 'end namespace', 'structure', 'end structure',

            'CDate', 'Date', 'DateAdd', 'DateDiff', 'DatePart', 'DateSerial', 'DateValue', 'Day', 'FormatDateTime',
            'Hour', 'IsDate', 'Minute', 'Month',
            'MonthName', 'Now', 'Second', 'Time', 'Timer', 'TimeSerial', 'TimeValue', 'Weekday', 'WeekdayName ', 'Year',
            'Asc', 'CBool', 'CByte', 'CCur', 'CDate', 'CDbl', 'Chr', 'CInt', 'CLng', 'CSng', 'CStr', 'Hex', 'Oct', 'FormatCurrency',
            'FormatDateTime', 'FormatNumber', 'FormatPercent', 'Abs', 'Atn', 'Cos', 'Exp', 'Hex', 'Int', 'Fix', 'Log', 'Oct',
            'Rnd', 'Sgn', 'Sin', 'Sqr', 'Tan',
            'Array', 'Filter', 'IsArray', 'Join', 'LBound', 'Split', 'UBound',
            'InStr', 'InStrRev', 'LCase', 'Left', 'Len', 'LTrim', 'RTrim', 'Trim', 'Mid', 'Replace', 'Right', 'Space', 'StrComp',
            'String', 'StrReverse', 'UCase',
            'CreateObject', 'Eval', 'GetLocale', 'GetObject', 'GetRef', 'InputBox', 'IsEmpty', 'IsNull', 'IsNumeric',
            'IsObject', 'LoadPicture', 'MsgBox', 'RGB', 'Round', 'ScriptEngine', 'ScriptEngineBuildVersion', 'ScriptEngineMajorVersion',
            'ScriptEngineMinorVersion', 'SetLocale', 'TypeName', 'VarType'
  ]);
  var operatorChars = /[*+\-<>=&|]/;

  function tokenBase(stream, state) {
    var ch = stream.next();
    curPunc = null;
    if (ch == "$" || ch == "?") {
      stream.match(/^[\w\d]*/);
      return "variable-2";
    }
    else if (ch == "<" && !stream.match(/^[\s\u00a0=]/, false)) {
      stream.match(/^[^\s\u00a0>]*>?/);
      return "atom";
    }
    else if (ch == "\"") {
      state.tokenize = tokenLiteral(ch);
      return state.tokenize(stream, state);
    }
    else if (/[{}\(\),\.;\[\]]/.test(ch)) {
      curPunc = ch;
      return null;
    }
    else if (ch == "'") {
        stream.skipToEnd();
        return "comment";
    }
    else if (operatorChars.test(ch)) {
      stream.eatWhile(operatorChars);
      return null;
    }
    else if (ch == ":") {
      stream.eatWhile(/[\w\d\._\-]/);
      return "atom";
    }
    else {
      stream.eatWhile(/[_\w\d]/);
      if (stream.eat(":")) {
        stream.eatWhile(/[\w\d_\-]/);
        return "atom";
      }
      var word = stream.current(), type;
      if (ops.test(word))
        return null;
      else if (keywords.test(word))
        return "keyword";
      else
        return "variable";
    }
  }

  function tokenLiteral(quote) {
    return function(stream, state) {
      var escaped = false, ch;
      while ((ch = stream.next()) != null) {
        if (ch == quote && !escaped) {
          state.tokenize = tokenBase;
          break;
        }
        escaped = !escaped && ch == "\\";
      }
      return "string";
    };
  }

  function tokenOpLiteral(quote) {
    return function(stream, state) {
      var escaped = false, ch;
      while ((ch = stream.next()) != null) {
        if (ch == quote && !escaped) {
          state.tokenize = tokenBase;
          break;
        }
        escaped = !escaped && ch == "\\";
      }
      return "variable-2";
    };
  }


  function pushContext(state, type, col) {
    state.context = {prev: state.context, indent: state.indent, col: col, type: type};
  }
  function popContext(state) {
    state.indent = state.context.indent;
    state.context = state.context.prev;
  }

  return {
    startState: function(base) {
      return {tokenize: tokenBase,
              context: null,
              indent: 0,
              col: 0};
    },

    token: function(stream, state) {
      if (stream.sol()) {
        if (state.context && state.context.align == null) state.context.align = false;
        state.indent = stream.indentation();
      }
      if (stream.eatSpace()) return null;
      var style = state.tokenize(stream, state);

      if (style != "comment" && state.context && state.context.align == null && state.context.type != "pattern") {
        state.context.align = true;
      }

      if (curPunc == "(") pushContext(state, ")", stream.column());
      else if (curPunc == "[") pushContext(state, "]", stream.column());
      else if (curPunc == "{") pushContext(state, "}", stream.column());
      else if (/[\]\}\)]/.test(curPunc)) {
        while (state.context && state.context.type == "pattern") popContext(state);
        if (state.context && curPunc == state.context.type) popContext(state);
      }
      else if (curPunc == "." && state.context && state.context.type == "pattern") popContext(state);
      else if (/atom|string|variable/.test(style) && state.context) {
        if (/[\}\]]/.test(state.context.type))
          pushContext(state, "pattern", stream.column());
        else if (state.context.type == "pattern" && !state.context.align) {
          state.context.align = true;
          state.context.col = stream.column();
        }
      }

      return style;
    },

    indent: function(state, textAfter) {
      var firstChar = textAfter && textAfter.charAt(0);
      var context = state.context;
      if (/[\]\}]/.test(firstChar))
        while (context && context.type == "pattern") context = context.prev;

      var closing = context && firstChar == context.type;
      if (!context)
        return 0;
      else if (context.type == "pattern")
        return context.col;
      else if (context.align)
        return context.col + (closing ? 0 : 1);
      else
        return context.indent + (closing ? 0 : indentUnit);
    }
  };
});

CodeMirror.defineMIME("text/x-vb", "vb");