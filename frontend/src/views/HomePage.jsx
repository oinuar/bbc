import {
   React,
} from '@/components';

import Page from '@/components/Page';
import MovieLibrary from '@/components/MovieLibrary';
import MovieRecommendations from '@/components/MovieRecommendations';

export default () => {
   return (
      <Page>
         <div class="mb-20">
            <h1 className="text-4xl pt-10 pb-10">Movies that you might like...</h1>

            <MovieRecommendations />
         </div>

         <div>
            <h1 className="text-4xl pt-10 pb-10">Explore something new!</h1>

            <MovieLibrary />
         </div>
      </Page>
   );
};
