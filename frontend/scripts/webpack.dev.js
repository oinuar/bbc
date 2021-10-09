const { merge } = require('webpack-merge');
const common = require('./webpack.common');
const path = require('path');

const config = {
  mode: 'development',
  devtool: 'inline-source-map',
  devServer: {
    contentBase: path.join(__dirname, '..', 'src'),
    hot: true,
    host: '0.0.0.0',
    port: 80,
    historyApiFallback: true,
    disableHostCheck: true,
    watchOptions: {
      poll: true,
    },
  },
  optimization: {
    runtimeChunk: true,
    removeAvailableModules: false,
    removeEmptyChunks: false,
    splitChunks: false,
  },
};

module.exports = [merge(common[0], config)];
