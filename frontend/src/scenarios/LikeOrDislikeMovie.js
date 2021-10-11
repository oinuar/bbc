import {
  takeLatest, call, put, all,
} from 'redux-saga/effects';
import { createSlice } from '@reduxjs/toolkit';
import { makeUserHeader, useApi } from '@/api';

import { userHasAccessToken } from '@/scenarios/GenerateAccessToken';

const slice = createSlice({
  name: 'Like or dislike movie',

  initialState: {
    likesByMovieId: {},
  },

  reducers: {
    'user likes a movie': () => {

    },

    'user dislikes a movie': () => {

    },

    'add the liked movie to the user preference': (state, { payload }) => {
      state.likesByMovieId[payload] = true;
    },

    'remove the liked movie from the user preference': (state, { payload }) => {
      delete state.likesByMovieId[payload];
    },
  },
});

export default slice;

export function getLike(id, state) {
  return state[slice.name].likesByMovieId[id] ?? false;
}

function* likeMovie({ payload }) {
  const api = useApi();
  const token = yield userHasAccessToken();

  yield call(api.post, `movie/like/${payload}`, {}, { headers: makeUserHeader(token) });

  yield put(slice.actions['add the liked movie to the user preference'](payload));
}

function* dislikeMovie({ payload }) {
  const api = useApi();
  const token = yield userHasAccessToken();

  yield call(api.post, `movie/dislike/${payload}`, {}, { headers: makeUserHeader(token) });

  yield put(slice.actions['remove the liked movie from the user preference'](payload));
}

export function* saga() {
  yield all([
    yield takeLatest(slice.actions['user likes a movie'], likeMovie),
    yield takeLatest(slice.actions['user dislikes a movie'], dislikeMovie),
  ]);
}
