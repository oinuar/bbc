import {
   takeLatest, select, call, put, all,
 } from 'redux-saga/effects';
import { createSlice } from '@reduxjs/toolkit';
import { useApi, makeUserHeader } from '@/api';

import { userHasAccessToken } from '@/scenarios/GenerateAccessToken';
import likeOrDislikeMovie from '@/scenarios/LikeOrDislikeMovie';

const slice = createSlice({
   name: 'Get movie recommendations for user based on genre preference',

   initialState: {
      all: [],
      offset: 0,
      limit: 2,
      loading: undefined,
      hasMoreResults: true,
   },

   reducers: {
      'user requests more movie recommendations': state => {
         state.loading = true;
      },

      'query next chunk of movies from a movie library': (state, { payload }) => {
         state.all.push(...payload);
         state.offset += Math.min(payload.length, state.limit);
         state.hasMoreResults = payload.length >= state.limit;
         state.loading = false;
      },

      reset: state => {
         state.all = [];
         state.offset = 0;
      },
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

function* queryNextChunkOfMoviesFromMovieLibrary() {
   const api = useApi();
   const token = yield* userHasAccessToken();

   const offset = yield select(getOffset);
   const limit = yield select(getLimit);

   const response = yield call(api.get, 'movie/recommendations', {
      headers: makeUserHeader(token),
      params: { offset, limit },
   });

   yield put(slice.actions['query next chunk of movies from a movie library'](response.data));
}

function* queryFreshMoviesFromMovieLibrary() {
   yield put(slice.actions.reset());
   yield* queryNextChunkOfMoviesFromMovieLibrary();
}

export function* saga() {
   yield all([
      takeLatest(slice.actions['user requests more movie recommendations'], queryNextChunkOfMoviesFromMovieLibrary),
      takeLatest([
         likeOrDislikeMovie.actions['user likes a movie'],
         likeOrDislikeMovie.actions['user dislikes a movie'],
      ], queryFreshMoviesFromMovieLibrary),
   ]);
}
