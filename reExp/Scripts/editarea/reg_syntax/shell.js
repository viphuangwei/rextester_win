editAreaLoader.load_syntax["shell"] = {
    'DISPLAY_NAME': 'shell'
	, 'COMMENT_SINGLE': { 0: "#" }
	, 'COMMENT_MULTI': { "#*": "*#" }
	, 'QUOTEMARKS': { 0: '"', 1: '\'' }
	, 'KEYWORD_CASE_SENSITIVE': true
	, 'KEYWORDS': {
	    'keywords': [
           'keyword', 'if', 'then', 'do', 'else', 'elif', 'while', 'until', 'for', 'in', 'esac', 'fi', 'fin', 'fil', 'done', 'exit', 'set', 'unset', 'export', 'function', 'builtin',
           'ab', 'awk', 'bash', 'beep', 'cat', 'cc', 'cd', 'chown', 'chmod', 'chroot', 'clear', 'cp', 'curl', 'cut', 'diff', 'echo', 'find', 'gawk', 'gcc', 'get', 'git', 'grep', 'kill',
           'killall', 'ln', 'ls', 'make', 'mkdir', 'openssl', 'mv', 'nc', 'node', 'npm', 'ping', 'ps', 'restart', 'rm', 'rmdir', 'sed', 'service', 'sh', 'shopt', 'shred', 'source', 'sort',
           'sleep', 'ssh', 'start', 'stop', 'su', 'sudo', 'tee', 'telnet', 'top', 'touch', 'vi', 'vim', 'wall', 'wc', 'wget', 'who', 'write', 'yes', 'zsh'
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
