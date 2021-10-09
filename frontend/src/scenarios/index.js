import {
   all, call, takeEvery,
} from 'redux-saga/effects';

export default {
};

/** This debug saga will print all the dispatched actions to console's debug log.
 *
 * Useful for troubleshooting even in production since this allows us to follow user's journey inside the application.
 */
function* debugSaga(action) {
   yield call(console.debug, `⚔️ ${action.type} <=`, action.payload); // eslint-disable-line no-alert, no-console
}

export function* saga() {
   yield all([
      takeEvery('*', debugSaga),
   ]);
}
