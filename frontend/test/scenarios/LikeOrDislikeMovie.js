import slice, { getLike, saga } from '@/scenarios/LikeOrDislikeMovie';
import {
   takeLatest, call, put, all,
 } from 'redux-saga/effects';
import { useApi } from '@/api';
import { userHasAccessToken } from '@/scenarios/GenerateAccessToken';

jest.mock('@/config', () => ({
   getApiUrl: () => 'http://unit-test/api',
}));

jest.mock('redux-saga/effects', () => ({
   all: jest.fn(),
   takeLatest: jest.fn(),
   put: jest.fn(),
   call: jest.fn(),
}));

jest.mock('@/api', () => ({
   ...jest.requireActual('@/api'),
   useApi: jest.fn(),
}));

jest.mock('@/scenarios/GenerateAccessToken', () => ({
   userHasAccessToken: jest.fn(),
}));

beforeEach(() => {
   all.mockReturnValue([]);
   takeLatest.mockReturnValue([]);
   put.mockReturnValue([]);
   call.mockImplementation((f, ...args) => f(...args));

   useApi.mockReturnValue({
      post: jest.fn(),
   });

   userHasAccessToken.mockReturnValue(['unit test access token']);
});

it('has name', () => {
   expect(slice.name).toEqual('Like or dislike movie');
});

it('reduces user likes a movie', () => {
   const state = {};

   slice.caseReducers['user likes a movie'](state);

   expect(state).toEqual({});
});

it('reduces user dislikes a movie', () => {
   const state = {};

   slice.caseReducers['user dislikes a movie'](state);

   expect(state).toEqual({});
});

it('reduces add the liked movie to the user preference', () => {
   const state = {
      likesByMovieId: {},
   };

   slice.caseReducers['add the liked movie to the user preference'](state, { payload: 'unit test movie id' });

   expect(state).toEqual({
      likesByMovieId: {
         'unit test movie id': true,
      },
   });
});

it('reduces remove the liked movie from the user preference', () => {
   const state = {
      likesByMovieId: {
         'unit test movie id': true
      }
   };

   slice.caseReducers['remove the liked movie from the user preference'](state, { payload: 'unit test movie id' });

   expect(state).toEqual({
      likesByMovieId: {},
   });
});

it('retrieves no likes', () => {
   const state = {
      [slice.name]: {
         likesByMovieId: {},
      },
   };

   expect(getLike('1', state)).toEqual(false);
});

it('retrieves like', () => {
   const state = {
      [slice.name]: {
         likesByMovieId: {
            '1': true,
         },
      },
   };

   expect(getLike('1', state)).toEqual(true);
});

it('acts user likes a movie', function* actsUserLikesMovie() {
   yield saga();

   expect(takeLatest).toHaveBeenCalled();
   expect(takeLatest.mock.calls[0][0].toString()).toEqual(slice.actions['user likes a movie'].toString());

   const likeMovie = takeLatest.mock.calls[0][1];
   const api = useApi();

   api.post.mockReturnValue(Promise.resolve({}));

   yield likeMovie({ payload: '1' });

   expect(api.post).toHaveBeenCalledWith('movie/like/1', {}, { headers: { Authorization: 'Bearer unit test access token' }});
   expect(put).toHaveBeenCalledWith(slice.actions['add the liked movie to the user preference']('1'));
});

it('acts user dislikes a movie', function* actsUserDislikesMovie() {
   yield saga();

   expect(takeLatest).toHaveBeenCalled();
   expect(takeLatest.mock.calls[1][0].toString()).toEqual(slice.actions['user dislikes a movie'].toString());

   const dislikeMovie = takeLatest.mock.calls[1][1];
   const api = useApi();

   api.post.mockReturnValue(Promise.resolve({}));

   yield dislikeMovie({ payload: '1' });

   expect(api.post).toHaveBeenCalledWith('movie/dislike/1', {}, { headers: { Authorization: 'Bearer unit test access token' }});
   expect(put).toHaveBeenCalledWith(slice.actions['remove the liked movie from the user preference']('1'));
});
