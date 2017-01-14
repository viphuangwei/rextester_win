editAreaLoader.load_syntax["ocaml"] = {
    'COMMENT_SINGLE': { 1: "(*" },
    'COMMENT_MULTI' : {'(*' : '*)'},
    'QUOTEMARKS': { 0: "\'", 1: "\"" },
    'KEYWORD_CASE_SENSITIVE': true,
    'KEYWORDS': {
        'keywords': ['true', 'false', 'let', 'rec', 'in', 'of', 'and', 'succ', 'if', 'then', 'else', 'for', 'to', 'while', 'do', 'done', 'fun', 'function', 'val', 'type', 'mutable', 'match', 'with', 'try', 'raise', 'begin', 'end', 'open', 'trace', 'ignore', 'exit', 'print_string', 'print_endline']
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
