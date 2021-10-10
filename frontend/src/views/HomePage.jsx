import {
   React, useSelector,
} from '@/components';

import Page from '@/components/Page';
import MovieLibrary from '@/components/MovieLibrary';
import MovieRecommendations from '@/components/MovieRecommendations';

import { getAll, isLoading } from '@/scenarios/GetMovieRecommendationsBasedOnUserGenrePreference';

export default () => {
   const recommendations = useSelector(getAll);
   const loadingRecommendations = useSelector(isLoading);

   return (
      <Page>
         {recommendations.length > 0 || loadingRecommendations ? (
            <div className="mb-20">
               <h1 className="text-4xl pt-10 pb-10">Movies that you might like...</h1>

               <MovieRecommendations />
            </div>
          ) : null}

         <div>
            <h1 className="text-4xl pt-10 pb-10">Explore something new!</h1>

            <MovieLibrary />
         </div>
      </Page>
   );
};
