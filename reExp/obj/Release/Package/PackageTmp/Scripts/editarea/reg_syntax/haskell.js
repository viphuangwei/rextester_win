editAreaLoader.load_syntax["haskell"] = {
    'DISPLAY_NAME': 'Haskell'
	, 'COMMENT_SINGLE': {1: "--"}
	, 'COMMENT_MULTI': {"{-": "-}"}
	, 'QUOTEMARKS': { 0: "\'", 1: "\"" } 
	, 'KEYWORD_CASE_SENSITIVE': true
	, 'KEYWORDS': {
		 'keywords': [
              'case', 'class', 'data', 'default', 'deriving', 'do', 'else', 'foreign',
              'if', 'import', 'in', 'infix', 'infixl', 'infixr', 'instance', 'let',
              'module', 'newtype', 'of', 'then', 'type', 'where', '_',
              'Bool', 'Bounded', 'Char', 'Double', 'EQ', 'Either', 'Enum', 'Eq',
              'False', 'FilePath', 'Float', 'Floating', 'Fractional', 'Functor', 'GT',
              'IO', 'IOError', 'Int', 'Integer', 'Integral', 'Just', 'LT', 'Left',
              'Maybe', 'Monad', 'Nothing', 'Num', 'Ord', 'Ordering', 'Rational', 'Read',
              'ReadS', 'Real', 'RealFloat', 'RealFrac', 'Right', 'Show', 'ShowS',
              'String', 'True'
        ]

		, 'functions': [
            'abs', 'acos', 'acosh', 'all', 'and', 'any', 'appendFile', 'asTypeOf',
            'asin', 'asinh', 'atan', 'atan2', 'atanh', 'break', 'catch', 'ceiling',
            'compare', 'concat', 'concatMap', 'const', 'cos', 'cosh', 'curry',
            'cycle', 'decodeFloat', 'div', 'divMod', 'drop', 'dropWhile', 'either',
            'elem', 'encodeFloat', 'enumFrom', 'enumFromThen', 'enumFromThenTo',
            'enumFromTo', 'error', 'even', 'exp', 'exponent', 'fail', 'filter',
            'flip', 'floatDigits', 'floatRadix', 'floatRange', 'floor', 'fmap',
            'foldl', 'foldl1', 'foldr', 'foldr1', 'fromEnum', 'fromInteger',
            'fromIntegral', 'fromRational', 'fst', 'gcd', 'getChar', 'getContents',
            'getLine', 'head', 'id', 'init', 'interact', 'ioError', 'isDenormalized',
            'isIEEE', 'isInfinite', 'isNaN', 'isNegativeZero', 'iterate', 'last',
            'lcm', 'length', 'lex', 'lines', 'log', 'logBase', 'lookup', 'map',
            'mapM', 'mapM_', 'max', 'maxBound', 'maximum', 'maybe', 'min', 'minBound',
            'minimum', 'mod', 'negate', 'not', 'notElem', 'null', 'odd', 'or',
            'otherwise', 'pi', 'pred', 'print', 'product', 'properFraction',
            'putChar', 'putStr', 'putStrLn', 'quot', 'quotRem', 'read', 'readFile',
            'readIO', 'readList', 'readLn', 'readParen', 'reads', 'readsPrec',
            'realToFrac', 'recip', 'rem', 'repeat', 'replicate', 'return', 'reverse',
            'round', 'scaleFloat', 'scanl', 'scanl1', 'scanr', 'scanr1', 'seq',
            'sequence', 'sequence_', 'show', 'showChar', 'showList', 'showParen',
            'showString', 'shows', 'showsPrec', 'significand', 'signum', 'sin',
            'sinh', 'snd', 'span', 'splitAt', 'sqrt', 'subtract', 'succ', 'sum',
            'tail', 'take', 'takeWhile', 'tan', 'tanh', 'toEnum', 'toInteger',
            'toRational', 'truncate', 'uncurry', 'undefined', 'unlines', 'until',
            'unwords', 'unzip', 'unzip3', 'userError', 'words', 'writeFile', 'zip',
            'zip3', 'zipWith', 'zipWith3'
		]
	}
	, 'OPERATORS': [
		'+', '-', '/', '*', '\.\.', ':', '::', '=', '\\', '\'', '<-', '->', '@', '~', '=>',
        '!!', '$!', '$', '&&', '+', '++', '-', '.', '/', '/=', '<', '<=', '=<<',
        '==', '>', '>=', '>>', '>>=', '^', '^^', '||', '*', '**'
	]
	, 'DELIMITERS': [
		'(', ')', '[', ']'
	]
	, 'STYLES': {
	    'COMMENTS': 'color: #379F4B;'
		, 'QUOTESMARKS': 'color: #CC0000;'
		, 'KEYWORDS': {
		      'keywords': 'color: #0000FF;'
			, 'functions': 'color: #0000FF;'
		}
		, 'OPERATORS': 'color: #000000;'
		, 'DELIMITERS': 'color: #000000;'

	}
};
