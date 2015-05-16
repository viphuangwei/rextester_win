﻿CodeMirror.defineMode("scala", function (config, parserConfig) {
    var indentUnit = config.indentUnit,
        keywords = parserConfig.keywords || {},
        blockKeywords = parserConfig.blockKeywords || {},
        atoms = parserConfig.atoms || {},
        hooks = parserConfig.hooks || {},
        multiLineStrings = parserConfig.multiLineStrings;
    var isOperatorChar = /[+\-*&%=<>!?|\/]/;

    var curPunc;

    function tokenBase(stream, state) {
        var ch = stream.next();
        if (hooks[ch]) {
            var result = hooks[ch](stream, state);
            if (result !== false) return result;
        }
        if (ch == "'") {
            state.tokenize = tokenChar();
            return state.tokenize(stream, state);
        }
        if (ch == '"') {
            return (state.tokenize = inString)(stream, state);
        }
        if (/[\[\]{}\(\),;\:\.]/.test(ch)) {
            curPunc = ch;
            return null
        }
        if (/\d/.test(ch)) {
            stream.eatWhile(/[\w\.]/);
            return "number";
        }
        if (ch == "/") {
            if (stream.eat("*")) {
                state.tokenize = tokenComment;
                return tokenComment(stream, state);
            }
            if (stream.eat("/")) {
                stream.skipToEnd();
                return "comment";
            }
        }
        if (isOperatorChar.test(ch)) {
            stream.eatWhile(isOperatorChar);
            return "operator";
        }
        stream.eatWhile(/[\w\$_]/);
        var cur = stream.current();
        if (keywords.propertyIsEnumerable(cur)) {
            if (blockKeywords.propertyIsEnumerable(cur)) curPunc = "newstatement";
            return "keyword";
        }
        if (atoms.propertyIsEnumerable(cur)) return "atom";
        return "word";
    }

    function tokenChar() {
        return function (stream, state) {
            var escaped = false, next, end = false;
            while ((next = stream.next()) != null) {
                if (next == "'" && !escaped) { end = true; break; }
                escaped = !escaped && next == "\\";
            }
            if (end || !(escaped || multiLineStrings))
                state.tokenize = null;
            return "string";
        };
    }

    function inString(stream, state) {
        var escaped = false, next;
        while (next = stream.next()) {
            if (next == '"' && !escaped) { state.tokenize = null; break; }
            escaped = !escaped && next == "\\";
        }
        return "string";
    }


    function tokenComment(stream, state) {
        var maybeEnd = false, ch;
        while (ch = stream.next()) {
            if (ch == "/" && maybeEnd) {
                state.tokenize = null;
                break;
            }
            maybeEnd = (ch == "*");
        }
        return "comment";
    }

    function Context(indented, column, type, align, prev) {
        this.indented = indented;
        this.column = column;
        this.type = type;
        this.align = align;
        this.prev = prev;
    }
    function pushContext(state, col, type) {
        return state.context = new Context(state.indented, col, type, null, state.context);
    }
    function popContext(state) {
        var t = state.context.type;
        if (t == ")" || t == "]" || t == "}")
            state.indented = state.context.indented;
        return state.context = state.context.prev;
    }

    // Interface

    return {
        startState: function (basecolumn) {
            return {
                tokenize: null,
                context: new Context((basecolumn || 0) - indentUnit, 0, "top", false),
                indented: 0,
                startOfLine: true
            };
        },

        token: function (stream, state) {
            var ctx = state.context;
            if (stream.sol()) {
                if (ctx.align == null) ctx.align = false;
                state.indented = stream.indentation();
                state.startOfLine = true;
            }
            if (stream.eatSpace()) return null;
            curPunc = null;
            var style = (state.tokenize || tokenBase)(stream, state);
            if (style == "comment" || style == "meta") return style;
            if (ctx.align == null) ctx.align = true;

            if ((curPunc == ";" || curPunc == ":") && ctx.type == "statement") popContext(state);
            else if (curPunc == "{") pushContext(state, stream.column(), "}");
            else if (curPunc == "[") pushContext(state, stream.column(), "]");
            else if (curPunc == "(") pushContext(state, stream.column(), ")");
            else if (curPunc == "}") {
                while (ctx.type == "statement") ctx = popContext(state);
                if (ctx.type == "}") ctx = popContext(state);
                while (ctx.type == "statement") ctx = popContext(state);
            }
            else if (curPunc == ctx.type) popContext(state);
            else if (ctx.type == "}" || ctx.type == "top" || (ctx.type == "statement" && curPunc == "newstatement"))
                pushContext(state, stream.column(), "statement");
            state.startOfLine = false;
            return style;
        },

        indent: function (state, textAfter) {
            if (state.tokenize != tokenBase && state.tokenize != null) return 0;
            var ctx = state.context, firstChar = textAfter && textAfter.charAt(0);
            if (ctx.type == "statement" && firstChar == "}") ctx = ctx.prev;
            var closing = firstChar == ctx.type;
            if (ctx.type == "statement") return ctx.indented + (firstChar == "{" ? 0 : indentUnit);
            else if (ctx.align) return ctx.column + (closing ? 0 : 1);
            else return ctx.indented + (closing ? 0 : indentUnit);
        },

        electricChars: "{}"
    };
});

(function () {
    function words(str) {
        var obj = {}, words = str.split(" ");
        for (var i = 0; i < words.length; ++i) obj[words[i]] = true;
        return obj;
    }
    var cKeywords = "auto if break int case long char register continue return default short do sizeof " +
      "double static else struct entry switch extern typedef float union for unsigned " +
      "goto while enum void const signed volatile";

    function cppHook(stream, state) {
        if (!state.startOfLine) return false;
        stream.skipToEnd();
        return "meta";
    }

    // C#-style strings where "" escapes a quote.
    function tokenAtString(stream, state) {
        var next;
        while ((next = stream.next()) != null) {
            if (next == '"' && !stream.eat('"')) {
                state.tokenize = null;
                break;
            }
        }
        return "string";
    }
    CodeMirror.defineMIME("text/x-scala", {
        name: "scala",
        keywords: words(

          /* scala */
          "abstract case catch class def do else extends false final finally for forSome if " +
          "implicit import lazy match new null object override package private protected return " +
          "sealed super this throw trait try trye type val var while with yield _ : = => <- <: " +
          "<% >: # @ " +

          /* package scala */
          "assert assume require print println printf readLine readBoolean readByte readShort " +
          "readChar readInt readLong readFloat readDouble " +

          "AnyVal App Application Array BufferedIterator BigDecimal BigInt Char Console Either " +
          "Enumeration Equiv Error Exception Fractional Function IndexedSeq Integral Iterable " +
          "Iterator List Map Numeric Nil NotNull Option Ordered Ordering PartialFunction PartialOrdering " +
          "Product Proxy Range Responder Seq Serializable Set Specializable Stream StringBuilder " +
          "StringContext Symbol Throwable Traversable TraversableOnce Tuple Unit Vector :: #:: " +

          /* package java.lang */
          "Boolean Byte Character CharSequence Class ClassLoader Cloneable Comparable " +
          "Compiler Double Exception Float Integer Long Math Number Object Package Pair Process " +
          "Runtime Runnable SecurityManager Short StackTraceElement StrictMath String " +
          "StringBuffer System Thread ThreadGroup ThreadLocal Throwable Triple Void"


        ),
        blockKeywords: words("catch class do else finally for forSome if match switch try while"),
        atoms: words("true false null"),
        hooks: {
            "@": function (stream) {
                stream.eatWhile(/[\w\$_]/);
                return "meta";
            }
        }
    });

}());
