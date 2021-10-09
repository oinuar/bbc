module.exports = {
   purge: ['./src/**/*.js', '.src/**/*.jsx'],
   darkMode: false, // or 'media' or 'class'
   theme: {
      extend: {
         colors: {
            primary: {
               ultralight: '#f2fbfc',
               light: '#d8f4f6',
               DEFAULT: '#00aaba',
            },
         },
      },
   },
   plugins: [],
};
