import {
  React, useDispatch, useAction, useSelector,
} from '@/components';

import MovieCard from '@/components/MovieCard';

import getMovieRecommendationsBasedOnUserGenrePreference, {
  getAll, isLoading, hasMoreResults,
} from '@/scenarios/GetMovieRecommendationsBasedOnUserGenrePreference';

export default () => {
  const dispatch = useDispatch();
  const onShowMore = useAction(dispatch, getMovieRecommendationsBasedOnUserGenrePreference.actions['user requests more movie recommendations']);

  const movies = useSelector(getAll);
  const loading = useSelector(isLoading);
  const hasNextPage = useSelector(hasMoreResults);

  const cards = movies?.map(movie => (
    <MovieCard key={movie.id} {...movie} />
  ));

  return (
    <React.Fragment>
      <div className="flex flex-wrap justify-start items-center">
        {cards}

        {(!loading && hasNextPage) ? (
          <button type="button" onClick={onShowMore}>
            Show more
          </button>
        ) : null}

        {loading ? (
          <div>
            Loading...
          </div>
        ) : null}
      </div>
    </React.Fragment>
  );
};
