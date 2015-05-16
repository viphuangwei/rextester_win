editAreaLoader.load_syntax["clisp"] = {
    'COMMENT_SINGLE': { 1: ";" },
    'COMMENT_MULTI': { "#|": "|#" },
    'QUOTEMARKS': { 0: "\""},
    'KEYWORD_CASE_SENSITIVE': false,
    'KEYWORDS': {
        'keywords': ["with", "def", "do", "prog", "case", "cond", "bind", "when", "unless"]
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
