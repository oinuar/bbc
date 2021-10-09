const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const tailwindcss = require('tailwindcss');
const autoprefixer = require('autoprefixer');

const modules = {
  rules: [
    {
      test: /\.(js|jsx)$/,
      include: path.resolve(__dirname, '../src'),
      loader: 'babel-loader',
      options: {
        presets: [
          '@babel/preset-env',
          '@babel/preset-react',
        ],
      },
    },
    {
      test: /\.css$/i,
      use: [
        MiniCssExtractPlugin.loader,
        'css-loader', {
          loader: 'postcss-loader',
          options: {
            postcssOptions: {
              ident: 'postcss',
              plugins: [tailwindcss, autoprefixer],
            },
          },
        }],
    },
    {
      test: /\.(png|svg|jpg|jpeg|gif|woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/i,
      type: 'asset/resource',
    },
  ],
};

const resolve = {
  extensions: ['.js', '.jsx'],
  alias: {
   '@': path.resolve(__dirname, '..', 'src'),
   'react-dom': '@hot-loader/react-dom',
  },
};

const distPath = path.resolve(__dirname, '../dist');
const output = {
  filename: '[name].bundle.js',
  publicPath: '/',
  clean: true,
};

module.exports = [
  {
    name: 'web',
    entry: {
      web: './src/index.js',
    },
    plugins: [
      new HtmlWebpackPlugin({
        template: './src/index.html',
        filename: './index.html',
        chunks: ['web'],
      }),
      new MiniCssExtractPlugin(),
    ],
    output: { ...output, path: distPath },
    module: modules,
    resolve,
  },
];
