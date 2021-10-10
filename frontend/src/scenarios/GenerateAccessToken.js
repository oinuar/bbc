import {
   takeLatest, call, put, take, select,
 } from 'redux-saga/effects';
import { createSlice } from '@reduxjs/toolkit';
import { useApi } from '@/api';

const slice = createSlice({
   name: 'Generate access token',

   initialState: {
      token: null,
   },

   reducers: {
      'user requests an access token': () => {

      },

      'generate a JWT token': (state, { payload }) => {
         state.token = payload;
      }
   }
});

export default slice;

function getToken(state) {
   return state[slice.name].token;
}

function* generateJwtToken() {
   const api = useApi();

   let token = yield select(getToken);

   // Generate token if it does not already exists. We should check expiry here
   // as well (and regenerate if expired) if our token could expire.
   if (!token) {
      const response = yield call(api.post, 'user/token/1');

      token = response.data;
   }

   yield put(slice.actions['generate a JWT token'](token));
}

export function* saga() {
   yield takeLatest(slice.actions['user requests an access token'], generateJwtToken);
}

export function* userHasAccessToken() {
   yield put(slice.actions['user requests an access token']());
   yield take(slice.actions['generate a JWT token']);

   return yield select(getToken);
}
