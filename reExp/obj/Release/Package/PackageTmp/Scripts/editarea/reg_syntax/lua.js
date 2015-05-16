﻿editAreaLoader.load_syntax["lua"] = {
    'DISPLAY_NAME': 'Lua'
	, 'COMMENT_SINGLE': { 1: "--" }
	, 'COMMENT_MULTI': { "--[[": "]]" }
	, 'QUOTEMARKS': { 1: "'", 2: '"' }
	, 'KEYWORD_CASE_SENSITIVE': true
	, 'KEYWORDS': {
	    'keywords': [
			"_G", "_VERSION", "assert", "collectgarbage", "dofile", "error", "getfenv", "getmetatable", "ipairs", "load",
            "loadfile", "loadstring", "module", "next", "pairs", "pcall", "print", "rawequal", "rawget", "rawset", "require",
            "select", "setfenv", "setmetatable", "tonumber", "tostring", "type", "unpack", "xpcall",

            "coroutine.create", "coroutine.resume", "coroutine.running", "coroutine.status", "coroutine.wrap", "coroutine.yield",

            "debug.debug", "debug.getfenv", "debug.gethook", "debug.getinfo", "debug.getlocal", "debug.getmetatable",
            "debug.getregistry", "debug.getupvalue", "debug.setfenv", "debug.sethook", "debug.setlocal", "debug.setmetatable",
            "debug.setupvalue", "debug.traceback",

            "close", "flush", "lines", "read", "seek", "setvbuf", "write",

            "io.close", "io.flush", "io.input", "io.lines", "io.open", "io.output", "io.popen", "io.read", "io.stderr", "io.stdin",
            "io.stdout", "io.tmpfile", "io.type", "io.write",

            "math.abs", "math.acos", "math.asin", "math.atan", "math.atan2", "math.ceil", "math.cos", "math.cosh", "math.deg",
            "math.exp", "math.floor", "math.fmod", "math.frexp", "math.huge", "math.ldexp", "math.log", "math.log10", "math.max",
            "math.min", "math.modf", "math.pi", "math.pow", "math.rad", "math.random", "math.randomseed", "math.sin", "math.sinh",
            "math.sqrt", "math.tan", "math.tanh",

            "os.clock", "os.date", "os.difftime", "os.execute", "os.exit", "os.getenv", "os.remove", "os.rename", "os.setlocale",
            "os.time", "os.tmpname",

            "package.cpath", "package.loaded", "package.loaders", "package.loadlib", "package.path", "package.preload",
            "package.seeall",

            "string.byte", "string.char", "string.dump", "string.find", "string.format", "string.gmatch", "string.gsub",
            "string.len", "string.lower", "string.match", "string.rep", "string.reverse", "string.sub", "string.upper",

            "table.concat", "table.insert", "table.maxn", "table.remove", "table.sort",

            "and", "break", "elseif", "false", "nil", "not", "or", "return",
			"true", "function", "end", "if", "then", "else", "do",
			"while", "repeat", "until", "for", "in", "local"
		]
	}
	, 'OPERATORS': [
		'+', '-', '/', '*', '==', '~=', '>', '<', '<=', '>='
	]
	, 'DELIMITERS': [
		'(', ')', '[', ']', '{', '}'
	]
	, 'STYLES': {
	    'COMMENTS': 'color: #379F4B;'
		, 'QUOTESMARKS': 'color: #CC0000;'
		, 'KEYWORDS': {
		    'keywords': 'color: #0000FF;'
		}
		, 'OPERATORS': 'color: #000000;'
		, 'DELIMITERS': 'color: #000000;'
	}
};
