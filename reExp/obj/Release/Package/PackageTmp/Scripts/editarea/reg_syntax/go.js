editAreaLoader.load_syntax["go"] = {
    'COMMENT_SINGLE': { 1: "//" },
    'COMMENT_MULTI': { "/*": "*/" },
    'QUOTEMARKS': { 0: "\"", 1: "'" },
    'KEYWORD_CASE_SENSITIVE': true,
    'KEYWORDS': {
        'keywords': ["break", "case", "chan", "const", "continue",
                     "default", "defer", "else", "fallthrough", "for",
                     "func", "go", "goto", "if", "import",
                     "interface", "map", "package", "range", "return",
                     "select", "struct", "switch", "type", "var",
                     "bool", "byte", "complex64", "complex128",
                     "float32", "float64", "int8", "int16", "int32",
                     "int64", "string", "uint8", "uint16", "uint32",
                     "uint64", "int", "uint", "uintptr", "true", "false", "iota", "nil", "append",
                     "cap", "close", "complex", "copy", "imag",
                     "len", "make", "new", "panic", "print",
                     "println", "real", "recover",
                     "else", "for", "func", "if", "interface",
                     "select", "struct", "switch"]
    },
    'OPERATORS': [],
    'DELIMITERS': [],
    'STYLES': {
        'COMMENTS': 'color: #379F4B;',
        'QUOTESMARKS': 'color: #CC0000;',
        'KEYWORDS': {
            'keywords': 'color: #0000FF;'
        },
        'OPERATORS': 'color: #000000;',
        'DELIMITERS': 'color: #000000;'
    }
};
