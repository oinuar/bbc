import useInfiniteScroll from 'react-infinite-scroll-hook';
import {
  React, useDispatch, useAction, useSelector,
} from '@/components';

import MovieCard from '@/components/MovieCard';

import getPaginatedMoviesFromMovieLibrary, {
  getAll, isLoading, hasMoreResults,
} from '@/scenarios/GetPaginatedMoviesFromMovieLibrary';

export default () => {
  const dispatch = useDispatch();

  const movies = useSelector(getAll);
  const loading = useSelector(isLoading);
  const hasNextPage = useSelector(hasMoreResults);

  const [scrollRef] = useInfiniteScroll({
    loading,
    hasNextPage,
    onLoadMore: useAction(dispatch, getPaginatedMoviesFromMovieLibrary.actions['user scrolls down the movie list']),
    rootMargin: '0px 0px 200px 0px',
    delayInMs: 300,
  });

  const cards = movies?.map(x => (
    <MovieCard key={x.id} {...x} />
  ));

  return (
    <React.Fragment>
      <div className="flex flex-wrap justify-center items-center">
        {cards}
      </div>

      {loading || hasNextPage ? (
        <div ref={scrollRef}>
          Loading...
        </div>
      ) : null}
    </React.Fragment>
  );
};
