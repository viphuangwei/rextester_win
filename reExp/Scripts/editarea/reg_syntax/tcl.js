editAreaLoader.load_syntax["tcl"] = {
    'DISPLAY_NAME': 'Tcl'
	, 'COMMENT_SINGLE': { 0: "#" }
	, 'COMMENT_MULTI': { "#*": "*#" }
	, 'QUOTEMARKS': { 0: '"', 1: '\'' }
	, 'KEYWORD_CASE_SENSITIVE': true
	, 'KEYWORDS': {
	    'keywords': [
            "Tcl", "safe", "after", "append", "array", "auto_execok", "auto_import", "auto_load", "auto_mkindex", "auto_mkindex_old", "auto_qualify", "auto_reset", "bgerror", "binary", "break", "catch", "cd", "close", "concat", "continue", "dde", "eof", "encoding", "error", "eval", "exec", "exit", "expr", "fblocked", "fconfigure", "fcopy", "file", "fileevent", "filename", "filename", "flush", "for", "foreach", "format", "gets", "glob", "global", "history", "http", "if", "incr", "info", "interp", "join", "lappend", "lindex", "linsert", "list", "llength", "load", "lrange", "lreplace", "lsearch", "lset", "lsort", "memory", "msgcat", "namespace", "open", "package", "parray", "pid", "pkg::create", "pkg_mkIndex", "proc", "puts", "pwd", "re_syntax", "read", "regex", "regexp", "registry", "regsub", "rename", "resource", "return", "scan", "seek", "set", "socket", "source", "split", "string", "subst", "switch", "tcl_endOfWord", "tcl_findLibrary", "tcl_startOfNextWord", "tcl_wordBreakAfter", "tcl_startOfPreviousWord", "tcl_wordBreakBefore", "tcltest", "tclvars", "tell", "time", "trace", "unknown", "unset", "update", "uplevel", "upvar", "variable", "vwait"
	    ]
	}
	, 'OPERATORS': [
		'+', '-', '/', '*', '=', '<', '>', '!', '&'
	]
	, 'DELIMITERS': [
		'(', ')', '[', ']', '{', '}'
	]
	, 'STYLES': {
	    'COMMENTS': 'color: #379F4B;'
		, 'QUOTESMARKS': 'color: #CC0000;'
		, 'KEYWORDS': {
		    'keywords': 'color: #0000FF;'
			, 'functions': 'color: #0000FF;'
			, 'statements': 'color: #0000FF;'
		}
		, 'OPERATORS': 'color: #000000;'
		, 'DELIMITERS': 'color: #000000;'

	}
};
