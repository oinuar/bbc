const { merge } = require('webpack-merge');
const CssMinimizerPlugin = require('css-minimizer-webpack-plugin');
const common = require('./webpack.common');

const prodConfig = {
  mode: 'production',
  output: {
    filename: '[name].[contenthash].bundle.js',
  },
  optimization: {
    moduleIds: 'deterministic',
    runtimeChunk: 'single',
    minimize: true,
    minimizer: [
      '...',
      new CssMinimizerPlugin(),
    ],
    splitChunks: {
      cacheGroups: {
        vendor: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendors',
          chunks: 'all',
        },
      },
    },
  },
};

module.exports = [merge(common[0], prodConfig)];
