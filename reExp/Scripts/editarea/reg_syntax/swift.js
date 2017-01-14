editAreaLoader.load_syntax["swift"] = {
    'COMMENT_SINGLE' : {1: "//"}, 
    'COMMENT_MULTI' : {"/*": "*/"},
    'QUOTEMARKS': { 0: "\'", 1: "\"" }, 
    'KEYWORD_CASE_SENSITIVE' : true,
    'KEYWORDS' : {
        'keywordgroup1': ["_", "var", "let", "class", "enum", "extension", "import", "protocol", "struct", "func", "typealias", "associatedtype",
                          "open", "public", "internal", "fileprivate", "private", "deinit", "init", "new", "override", "self", "subscript", "super",
                          "convenience", "dynamic", "final", "indirect", "lazy", "required", "static", "unowned", "unowned(safe)", "unowned(unsafe)", "weak", "as", "is",
                          "break", "case", "continue", "default", "else", "fallthrough", "for", "guard", "if", "in", "repeat", "switch", "where", "while",
                          "defer", "return", "inout", "mutating", "nonmutating", "catch", "do", "rethrows", "throw", "throws", "try", "didSet", "get", "set", "willSet",
                          "assignment", "associativity", "infix", "left", "none", "operator", "postfix", "precedence", "precedencegroup", "prefix", "right",
                          "Any", "AnyObject", "Type", "dynamicType", "Self", "Protocol", "__COLUMN__", "__FILE__", "__FUNCTION__", "__LINE__"],
        'keywordgroup2': ["true", "false", "nil", "self", "super", "_"],
        'keywordgroup3': ["var", "let", "class", "enum", "extension", "import", "protocol", "struct", "func", "typealias", "associatedtype", "for"],
        'keywordgroup4': ["Array", "Bool", "Character", "Dictionary", "Double", "Float", "Int", "Int8", "Int16", "Int32", "Int64", "Never", "Optional", "Set", "String","UInt8", "UInt16", "UInt32", "UInt64", "Void"]
    },
    'OPERATORS' : ["+", "-", "*", "?", "=", "/", "%", "&", ">", "<", "^", "!", ":", ";", "|"], 
    'DELIMITERS' : [ '(', ')', '[', ']', '{', '}' ], 
    'STYLES' : { 
        'COMMENTS' : 'color: #379F4B;',
        'QUOTESMARKS' : 'color: #CC0000;',
        'KEYWORDS' : { 
            'keywordgroup1': 'color: #0000FF;',
            'keywordgroup2': 'color: #0000FF;',
            'keywordgroup3': 'color: #0000FF;',
            'keywordgroup4': 'color: #0000FF;'
        },
        'OPERATORS' : 'color: #000000;',
        'DELIMITERS' : 'color: #000000;'
    } 
}; 
