import slice, {
   getAll, isLoading, hasMoreResults, saga,
} from '@/scenarios/GetPaginatedMoviesFromMovieLibrary';
import {
   takeLatest, select, put, call,
 } from 'redux-saga/effects';
import { useApi } from '@/api';

import likeOrDislikeMovie from '@/scenarios/LikeOrDislikeMovie';
import { userHasAccessToken } from '@/scenarios/GenerateAccessToken';

jest.mock('@/config', () => ({
   getApiUrl: () => 'http://unit-test/api',
}));

jest.mock('redux-saga/effects', () => ({
   takeLatest: jest.fn(),
   select: jest.fn(),
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

jest.mock('@/scenarios/LikeOrDislikeMovie', () => ({
   __esModule: true,
   default: {
      actions: {
         'add the liked movie to the user preference': payload => 'like ' + payload,
      },
   },
}));

beforeEach(() => {
   takeLatest.mockReturnValue([]);
   put.mockReturnValue([]);
   call.mockImplementation((f, ...args) => f(...args));

   useApi.mockReturnValue({
      get: jest.fn(),
      post: jest.fn(),
   });

   userHasAccessToken.mockReturnValue(['unit test access token']);
});

it('has name', () => {
   expect(slice.name).toEqual('Get paginated movies from movie library');
});

it('reduces user scrolls down the movie list', () => {
   const state = {};

   slice.caseReducers['user scrolls down the movie list'](state);

   expect(state).toEqual({
      loading: true,
   });
});

describe('reduce query next chunk of movie results from movie library', () => {
   it('has more results', () => {
      const state = {
         all: [0],
         offset: 11,
         limit: 3,
      };

      const payload = [1, 2, 3];

      slice.caseReducers['query next chunk of movie results from movie library'](state, { payload });

      expect(state).toEqual({
         all: [0].concat(payload),
         offset: payload.length + 11,
         limit: 3,
         hasMoreResults: true,
         loading: false,
      });
   });

   it('does not have more results', () => {
      const state = {
         all: [0],
         offset: 11,
         limit: 4,
      };

      const payload = [1, 2, 3];

      slice.caseReducers['query next chunk of movie results from movie library'](state, { payload });

      expect(state).toEqual({
         all: [0].concat(payload),
         offset: payload.length + 11,
         limit: 4,
         hasMoreResults: false,
         loading: false,
      });
   });
});

it('retrieves all', () => {
   const state = {
      [slice.name]: {
         all: [1],
      },
   };

   expect(getAll(state)).toEqual([1]);
});

it('retrieves loading state', () => {
   const state = {
      [slice.name]: {
         loading: true,
      },
   };

   expect(isLoading(state)).toEqual(true);
});

it('retrieves has more results', () => {
   const state = {
      [slice.name]: {
         hasMoreResults: false,
      },
   };

   expect(hasMoreResults(state)).toEqual(false);
});

it('acts user scrolls down the movie list', function* actsUserScrollsDownTheMovieList() {
   yield saga();

   expect(takeLatest).toHaveBeenCalled();
   expect(takeLatest.mock.calls[0][0].toString()).toEqual(slice.actions['user scrolls down the movie list'].toString());

   const queryNextChunkOfMovieResultsFromMovieLibrary = takeLatest.mock.calls[0][1];
   const api = useApi();
   const offset = 11;
   const limit = 1000;
   const movies = [{id: 1}, {id: 2}, {id: 3}];
   const state = {
      [slice.name]: { offset, limit },
   };

   select.mockImplementation(f => Promise.resolve(f(state)));

   api.get.mockReturnValue(Promise.resolve({
      data: movies,
   }));

   api.post.mockReturnValue(Promise.resolve({
      data: [2, 3],
   }));

   yield queryNextChunkOfMovieResultsFromMovieLibrary();

   expect(api.get).toHaveBeenCalledWith('movie', { headers: { Authorization: 'Bearer unit test access token' }, params: { offset, limit } });
   expect(api.post).toHaveBeenCalledWith('user/moviePreference', movies.map(x => x.id), { headers: { Authorization: 'Bearer unit test access token' } });
   expect(put).toHaveBeenCalledWith(likeOrDislikeMovie.actions['add the liked movie to the user preference'](2));
   expect(put).toHaveBeenCalledWith(likeOrDislikeMovie.actions['add the liked movie to the user preference'](3));
   expect(put).toHaveBeenCalledWith(slice.actions['query next chunk of movie results from movie library'](movies));
});
