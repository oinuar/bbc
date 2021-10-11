import {
  all, takeLatest, select, put, call,
} from 'redux-saga/effects';
import slice, {
  getAll, isLoading, hasMoreResults, saga,
} from '@/scenarios/GetMovieRecommendationsBasedOnUserGenrePreference';
import { useApi } from '@/api';

import likeOrDislikeMovie from '@/scenarios/LikeOrDislikeMovie';
import { userHasAccessToken } from '@/scenarios/GenerateAccessToken';

jest.mock('@/config', () => ({
  getApiUrl: () => 'http://unit-test/api',
}));

jest.mock('redux-saga/effects', () => ({
  all: jest.fn(),
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
      'add the liked movie to the user preference': () => 'like',
      'remove the liked movie from the user preference': () => 'dislike',
    },
  },
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
  expect(slice.name).toEqual('Get movie recommendations for user based on genre preference');
});

it('reduces user requests more movie recommendations', () => {
  const state = {};

  slice.caseReducers['user requests more movie recommendations'](state);

  expect(state).toEqual({
    loading: true,
  });
});

describe('reduce query next chunk of movies from a movie library', () => {
  it('has more results', () => {
    const state = {
      all: [0],
      offset: 11,
      limit: 3,
    };

    const payload = [1, 2, 3];

    slice.caseReducers['query next chunk of movies from a movie library'](state, { payload });

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

    slice.caseReducers['query next chunk of movies from a movie library'](state, { payload });

    expect(state).toEqual({
      all: [0].concat(payload),
      offset: payload.length + 11,
      limit: 4,
      hasMoreResults: false,
      loading: false,
    });
  });
});

describe('reduce query movies from a movie library', () => {
  it('has more results', () => {
    const state = {
      all: [0],
      offset: 11,
      limit: 3,
    };

    const payload = [1, 2, 3];

    slice.caseReducers['query movies from a movie library'](state, { payload });

    expect(state).toEqual({
      all: payload,
      offset: payload.length,
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

    slice.caseReducers['query movies from a movie library'](state, { payload });

    expect(state).toEqual({
      all: payload,
      offset: payload.length,
      limit: 4,
      hasMoreResults: false,
      loading: false,
    });
  });
});

it('reduces reset', () => {
  const state = {};

  slice.caseReducers.reset(state);

  expect(state).toEqual({
    offset: 0,
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

it('acts user requests more movie recommendations', function* actsUserRequestsMoreMovieRecommendations() {
  yield saga();

  expect(takeLatest).toHaveBeenCalled();
  expect(takeLatest.mock.calls[0][0].toString()).toEqual(slice.actions['user requests more movie recommendations'].toString());

  const queryNextChunkOfMoviesFromMovieLibrary = takeLatest.mock.calls[0][1];
  const api = useApi();
  const offset = 11;
  const limit = 1000;
  const movies = [{ id: 1 }, { id: 2 }, { id: 3 }];
  const state = {
    [slice.name]: { offset, limit },
  };

  select.mockImplementation(f => Promise.resolve(f(state)));

  api.post.mockReturnValue(Promise.resolve({
    data: movies,
  }));

  yield queryNextChunkOfMoviesFromMovieLibrary();

  expect(api.post).toHaveBeenCalledWith('movie/recommendations', {}, { headers: { Authorization: 'Bearer unit test access token' }, params: { offset, limit } });
  expect(put).toHaveBeenCalledWith(slice.actions['query next chunk of movies from a movie library'](movies));
});

it('acts add the liked movie to the user preference & remove the liked movie from the user preference', function* actsLikeOrDislike() {
  yield saga();

  expect(takeLatest).toHaveBeenCalled();
  expect(takeLatest.mock.calls[1][0].map(x => x.toString())).toEqual([
    likeOrDislikeMovie.actions['add the liked movie to the user preference'].toString(),
    likeOrDislikeMovie.actions['remove the liked movie from the user preference'].toString(),
  ]);

  const queryMoviesFromMovieLibrary = takeLatest.mock.calls[1][1];
  const api = useApi();
  const offset = 0;
  const limit = 1000;
  const movies = [{ id: 1 }, { id: 2 }, { id: 3 }];
  const state = {
    [slice.name]: { offset, limit },
  };

  select.mockImplementation(f => Promise.resolve(f(state)));

  api.post.mockReturnValue(Promise.resolve({
    data: movies,
  }));

  yield queryMoviesFromMovieLibrary();

  expect(put).toHaveBeenCalledWith(slice.actions.reset());
  expect(api.post).toHaveBeenCalledWith('movie/recommendations', {}, { headers: { Authorization: 'Bearer unit test access token' }, params: { offset, limit } });
  expect(put).toHaveBeenCalledWith(slice.actions['query movies from a movie library'](movies));
});
