import {
   all, call, takeEvery, fork,
} from 'redux-saga/effects';

import generateAccessToken, { saga as generateAccessTokenSaga } from '@/scenarios/GenerateAccessToken';
import getPaginatedMoviesFromMovieLibrary, { saga as getPaginatedMoviesSaga } from '@/scenarios/GetPaginatedMoviesFromMovieLibrary';
import likeOrDislikeMovie, { saga as likeOrDislikeMovieSaga } from '@/scenarios/LikeOrDislikeMovie';
import getMovieRecommendationsBasedOnUserGenrePreference, { saga as getMovieRecommendationsBasedOnUserGenrePreferenceSaga } from '@/scenarios/GetMovieRecommendationsBasedOnUserGenrePreference';

export default {
   [generateAccessToken.name]: generateAccessToken.reducer,
   [getPaginatedMoviesFromMovieLibrary.name]: getPaginatedMoviesFromMovieLibrary.reducer,
   [likeOrDislikeMovie.name]: likeOrDislikeMovie.reducer,
   [getMovieRecommendationsBasedOnUserGenrePreference.name]: getMovieRecommendationsBasedOnUserGenrePreference.reducer,
};

/** This debug saga will print all the dispatched actions to console's debug log.
 *
 * Useful for troubleshooting even in production since this allows us to follow user's journey inside the application.
 */
function* debugSaga(action) {
   yield call(console.debug, `⚔️ ${action.type} <=`, action.payload); // eslint-disable-line no-console
}

export function* saga() {
   yield all([
      fork(generateAccessTokenSaga),
      fork(getPaginatedMoviesSaga),
      fork(likeOrDislikeMovieSaga),
      fork(getMovieRecommendationsBasedOnUserGenrePreferenceSaga),
      takeEvery('*', debugSaga),
   ]);
}
