import slice, { saga, userHasAccessToken } from '@/scenarios/GenerateAccessToken';
import {
   takeLatest, select, call, put, take,
 } from 'redux-saga/effects';
import { useApi } from '@/api';

jest.mock('@/config', () => ({
   getApiUrl: () => 'http://unit-test/api',
}));

jest.mock('redux-saga/effects', () => ({
   takeLatest: jest.fn(),
   select: jest.fn(),
   put: jest.fn(),
   take: jest.fn(),
   call: jest.fn(),
}));

jest.mock('@/api', () => ({
   useApi: jest.fn(),
}));

beforeEach(() => {
   takeLatest.mockReturnValue([]);
   put.mockReturnValue([]);
   take.mockReturnValue([]);
   call.mockImplementation((f, ...args) => f(...args));

   useApi.mockReturnValue({
      post: jest.fn(),
   });
});

it('has name', () => {
   expect(slice.name).toEqual('Generate access token');
});

it('reduces user request an access token', () => {
   const state = {};

   slice.caseReducers['user requests an access token'](state);

   expect(state).toEqual({});
});

it('reduces generate a JWT token', () => {
   const state = {};

   slice.caseReducers['generate a JWT token'](state, { payload: 'unit test token' });

   expect(state).toEqual({ token: 'unit test token' });
});

describe('act on user requests an access token', () => {
   function* run(state) {
      yield saga();

      expect(takeLatest).toHaveBeenCalled();
      expect(takeLatest.mock.calls[0][0].toString()).toEqual(slice.actions['user requests an access token'].toString());

      const generateJwtToken = takeLatest.mock.calls[0][1];

      select.mockImplementation(f => Promise.resolve(f(state)));

      yield generateJwtToken();
   }

   it('uses existing token', function*() {
      const state = {
         [slice.name]: {
            token: 'unit test token',
         },
      };

      yield run(state);

      expect(put).toHaveBeenCalledWith(slice.actions['generate a JWT token'](state[slice.name].token));
   });

   it('generates a new token', function*() {
      const api = useApi();

      api.post.mockReturnValue(Promise.resolve({
         data: 'generated unit test token',
      }));

      const state = {
         [slice.name]: {
            token: null,
         },
      };

      yield run(state);

      expect(call).toHaveBeenCalledWith(api.post, 'user/token/1');
      expect(put).toHaveBeenCalledWith(slice.actions['generate a JWT token']('generated unit test token'));
   });
});

it('obtains an access token', function*() {
   const state = {
      [slice.name]: {
         token: 'unit test token',
      }
   };

   select.mockImplementation(f => Promise.resolve(f(state)));

   const result = yield userHasAccessToken();

   expect(put).toHaveBeenCalledWith(slice.actions['user requests an access token']());
   expect(take).toHaveBeenCalled();
   expect(take.mock.calls[0][0].toString()).toEqual(slice.actions['generate a JWT token'].toString());

   expect(result).toEqual(state[slice.name].token);
});
