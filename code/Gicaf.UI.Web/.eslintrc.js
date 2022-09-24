module.exports = {
	env: {
	  browser: true,
	  es6: true,
	},
	extends: [
	  'plugin:@typescript-eslint/recommended',
	  'plugin:react/recommended',
	  'airbnb',
	  'plugin:import/typescript',
	],
	globals: {
	  Atomics: 'readonly',
	  SharedArrayBuffer: 'readonly',
	},
	parser: '@typescript-eslint/parser',
	parserOptions: {
	  ecmaFeatures: {
		jsx: true,
	  },
	  ecmaVersion: 2018,
	  sourceType: 'module',
	},
	plugins: ['react', '@typescript-eslint', 'import', 'jsx-a11y', 'react-hooks'],
	rules: {
	  // Permite o uso de JSX em apenas arquivos com a extensão .tsx
	  'react/jsx-filename-extension': ['error', { extensions: ['.tsx'] }],
	  // Permite que nunca seja necessário informar a extensão quando a extensão dos arquivos forem qualquer uma que esteja dentro do objeto
	  'import/extensions': [
		'error',
		{
		  ts: 'never',
		  tsx: 'never',
		  js: 'never',
		  jsx: 'never',
		},
	  ],
	},
	settings: {
	  // Configuramos o parser para reconhecer as extensões corretamente ****
	  'import/parsers': {
		'@typescript-eslint/parser': ['.ts', '.tsx'],
	  },
	  'import/resolver': {
		typescript: {
		  // Diretório do tsconfig.json
		  directory: '.',
		},
		// Configura o resolver do node para reconhecer arquivos com extensão .ts e .tsx
		node: {
		  extensions: ['.ts', '.tsx', '.js', '.jsx'],
		},
		// Configuramos o resolver do root import. A configuração é muito semelhante a do plugin em si.
		'babel-plugin-root-import': {
		  paths: [
			{
			  rootPathSuffix: 'src',
			},
			{
			  rootPathPrefix: '#redux/',
			  rootPathSuffix: 'src/store/modules/',
			},
		  ],
		},
	  },
	},
  };