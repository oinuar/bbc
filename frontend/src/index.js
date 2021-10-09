import 'react-hot-loader';
import React from 'react'; // eslint-disable-line
import ReactDOM from 'react-dom';

import App from '@/views/App';

const render = Component => {
  // eslint-disable-next-line react/jsx-filename-extension
  ReactDOM.render(<Component />, document.getElementById('app'));
};

render(App);
