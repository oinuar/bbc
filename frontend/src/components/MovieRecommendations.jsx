import {
   React, useDispatch, useAction, useSelector,
} from '@/components';

import MovieCard from '@/components/MovieCard';

import getMovieRecommendationsBasedOnUserGenrePreference, {
   getAll, isLoading, hasMoreResults,
} from '@/scenarios/GetMovieRecommendationsBasedOnUserGenrePreference';

export default () => {
   const dispatch = useDispatch();

   const movies = useSelector(getAll);
   const loading = useSelector(isLoading);
   const hasNextPage = useSelector(hasMoreResults);

   const cards = movies.map(x => (
      <MovieCard key={x.id} {...x} />
   ));

   return (
      <React.Fragment>
         <div className="flex flex-wrap justify-start items-center">
            {cards}

            {hasNextPage ? (
               <button role="button" onClick={useAction(dispatch, getMovieRecommendationsBasedOnUserGenrePreference.actions['user requests more movie recommendations'])}>
                  Show more
               </button>
            ) : null}
         </div>

         {loading ? (
            <div>
               Loading...
            </div>
         ) : null}
      </React.Fragment>
   );
};
