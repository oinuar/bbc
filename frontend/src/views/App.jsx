import { hot } from 'react-hot-loader/root';
import React from 'react'; // eslint-disable-line
import { Provider } from 'react-redux';
import store from '@/store';
import Views from '@/views';

import 'tailwindcss/tailwind.css';
import '@/style.css';

const App = () => (
  <Provider store={store}>
    <Views />
  </Provider>
);

export default hot(App);
