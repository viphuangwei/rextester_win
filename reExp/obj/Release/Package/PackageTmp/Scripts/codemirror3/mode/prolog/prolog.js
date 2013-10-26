CodeMirror.defineMode("prolog", function (config) {
    var indentUnit = config.indentUnit;
    var curPunc;

    function wordRegexp(words) {
        return new RegExp("^(?:" + words.join("|") + ")$", "i");
    }
    var ops = wordRegexp([]);
    var keywords = wordRegexp([
      ('is'), ('get_char'), ('write'), ('min'), ('max'), ('garbage_collect'), ('fail'), ('catch'), ('throw'), ('repeat'), ('module'), ('use_module'), ('char_code'), ('char_code'), ('sub_string'), ('sub_atom'), ('format'), ('append'), ('length'), ('atom_length'), ('char_type'), ('upcase_atom'), ('downcase_atom'), ('No'), ('not'), ('true'), ('Yes'), ('ButFirst'), ('forall'), ('member'), ('concat_atom'), ('last'), ('append'), ('flatten'), ('length'), ('get_assoc'), ('reverse'), ('sort'), ('predsort'), ('keysort'), ('mergesort'), ('maplist'), ('sublist'), ('maplist2'), ('Nothing'), ('call'), ('Args'), ('findall'), ('Keys'), ('numlist'), ('between'), ('log10'), ('log'), ('mod'), ('rem'), ('random'), ('sqrt'), ('exp'), ('abs'), ('sin'), ('cos'), ('tan'), ('asin'), ('acos'), ('atan'), ('truncate'), ('round'), ('floor'), ('ceiling')
    ]);
    var operatorChars = /[*+\-<>=&|]/;

    function tokenBase(stream, state) {
        var ch = stream.next();
        curPunc = null;
        if (ch == "\"" || ch == "'") {
            state.tokenize = tokenLiteral(ch);
            return state.tokenize(stream, state);
        }
        else if (/[{}\(\),\.;\[\]]/.test(ch)) {
            curPunc = ch;
            return null;
        }
        else if (ch == "%") {
                stream.skipToEnd();
                return "comment";
        }
        else if (ch == "/") {
            if (stream.eat("*")) {
                state.tokenize = tokenComment;
                return tokenComment(stream, state);
            }
        }
        else if (operatorChars.test(ch)) {
            stream.eatWhile(operatorChars);
            return null;
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

        return null
    }

    function tokenComment(stream, state) {
        var maybeEnd = false, ch;
        while (ch = stream.next()) {
            if (ch == "/" && maybeEnd) {
                state.tokenize = tokenBase;
                break;
            }
            maybeEnd = (ch == "*");
        }
        return "comment";
    }

    function tokenLiteral(quote) {
        return function (stream, state) {
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

    function pushContext(state, type, col) {
        state.context = { prev: state.context, indent: state.indent, col: col, type: type };
    }
    function popContext(state) {
        state.indent = state.context.indent;
        state.context = state.context.prev;
    }

    return {
        startState: function (base) {
            return {
                tokenize: tokenBase,
                context: null,
                indent: 0,
                col: 0
            };
        },

        token: function (stream, state) {
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

        indent: function (state, textAfter) {
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

CodeMirror.defineMIME("text/x-prolog", "prolog");
