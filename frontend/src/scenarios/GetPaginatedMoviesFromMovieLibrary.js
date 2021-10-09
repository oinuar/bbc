import {
   takeLatest, select, call, put,
 } from 'redux-saga/effects';
import { createSlice } from '@reduxjs/toolkit';
import { useApi, makeUserHeader } from '@/api';

import { userHasAccessToken } from '@/scenarios/GenerateAccessToken';

const slice = createSlice({
   name: 'Get paginated movies from movie library',

   initialState: {
      all: [],
      offset: 0,
      limit: 25,
      loading: undefined,
      hasMoreResults: true,
   },

   reducers: {
      'user scrolls down the movie list': state => {
         state.loading = true;
      },

      'query next chunk of movie results from movie library': (state, { payload }) => {
         state.all.push(...payload);
         state.offset += Math.min(payload.length, state.limit);
         state.hasMoreResults = payload.length >= state.limit;
         state.loading = false;
      }
   }
});

export default slice;

export function getAll(state) {
   return state[slice.name].all;
}

export function isLoading(state) {
   return state[slice.name].loading;
}

export function hasMoreResults(state) {
   return state[slice.name].hasMoreResults;
}

function getOffset(state) {
   return state[slice.name].offset;
}

function getLimit(state) {
   return state[slice.name].limit;
}

function* queryNextChunkOfMovieResultsFromMovieLibrary({ payload }) {
   const api = useApi();
   const token = yield* userHasAccessToken();

   const offset = yield select(getOffset);
   const limit = yield select(getLimit);

   const response = yield call(api.get, 'movie', {
      headers: makeUserHeader(token),
      params: { offset, limit },
   });

   yield put(slice.actions['query next chunk of movie results from movie library'](response.data));
}

export function* saga() {
   yield takeLatest(slice.actions['user scrolls down the movie list'], queryNextChunkOfMovieResultsFromMovieLibrary);
}
