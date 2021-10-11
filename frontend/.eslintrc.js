const path = require('path');

module.exports = {
   parser: '@babel/eslint-parser',
   extends: 'eslint-config-airbnb',
   rules: {
      'max-len': ['error', { code: 200 }],
      'react/jsx-wrap-multilines': ['error', { declaration: false, assignment: false }],
      'no-plusplus': ['error', { allowForLoopAfterthoughts: true }],
      'no-else-return': ['error', { allowElseIf: true }],
      'class-methods-use-this': 'off',
      'no-param-reassign': 'off',
      'react/forbid-prop-types': 0,
      'react/require-default-props': 0,
      'arrow-parens': ['error', 'as-needed'],
      'react/jsx-fragments': ['error', 'element'],
      'react/jsx-props-no-spreading': 0,
      'lines-between-class-members': ['error', 'always', { exceptAfterSingleLine: true }],
      'import/prefer-default-export': 'off',
      'react/prop-types': 'off',
      'react/prefer-stateless-function': 'error',
      'no-restricted-syntax': ['warn',
         {
            selector: "ImportSpecifier[imported.name='React'][local.name!='React']",
            message: 'Do not alias React import with `import { React as ... }`.'
         },
         {
            selector: "MemberExpression[object.name='React']",
            message: 'Do not use React object directly, instead import the functionality what you need from @/components.'
         }
      ],
      'no-restricted-imports': ['error', {
         paths: [
            {
               name: 'react',
               message: 'Please use @/components import instead and specify what you want to use.'
            }
         ],
         patterns: [
            {
               group: ['.*', '*/../*'],
               message: 'Please use absolute imports, like @/components/MyComponent.'
            }
         ]
      }]
   },
   globals: {
      window: true,
      document: true,
   },
   settings: {
      'import/resolver': {
         webpack: {
            config: {
               resolve: {
                  alias: {
                     '@': path.resolve('src'),
                  },
                  extensions: ['.js', '.jsx'],
               },
            },
         },
      },
   },
   overrides: [{
      files: [
         'test/**',
      ],
      env: {
         jest: true,
      },
   }],
};
