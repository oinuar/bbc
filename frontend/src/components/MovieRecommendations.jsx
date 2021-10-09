import {
   React, useDispatch, useAction, useSelector,
} from '@/components';

import MovieCard from '@/components/MovieCard';

export default () => {
   const dispatch = useDispatch();
   const movies = [];

   const cards = movies.map(x => (
      <MovieCard key={x.id} {...x} />
   ));

   return (
      <React.Fragment>
         <div className="flex flex-wrap justify-center items-center">
            {cards}

            <div>
               Show more
            </div>
         </div>
      </React.Fragment>
   );
};
