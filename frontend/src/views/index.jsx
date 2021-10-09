import { React } from '@/components';
import {
  Router, Switch, Route,
} from 'react-router-dom';
import { history } from '@/navigation';

import HomePage from '@/views/HomePage';

export default () => (
  <Router history={history}>
    <Switch>
      <Route path="/">
         <HomePage />
      </Route>
    </Switch>
  </Router>
);
