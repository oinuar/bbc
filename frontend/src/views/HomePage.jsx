import {
   React, useDispatch, useAction, useSelector,
} from '@/components';

import useInfiniteScroll from 'react-infinite-scroll-hook';
import Page from '@/components/Page';

import getPaginatedMoviesFromMovieLibrary, {
   getAll, isLoading, hasMoreResults,
} from '@/scenarios/GetPaginatedMoviesFromMovieLibrary';

import likeOrDislikeMovie, {
   getLikes,
} from '@/scenarios/LikeOrDislikeMovie';

function getLabelStyle(id) {
   return {
      backgroundImage: 'url(https://m.media-amazon.com/images/M/MV5BMDFkYTc0MGEtZmNhMC00ZDIzLWFmNTEtODM1ZmRlYWMwMWFmXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_.jpg)',
      backgroundPosition: '50% 15%',
   };
}

export default () => {
   const dispatch = useDispatch();

   const movies = useSelector(getAll);
   const loading = useSelector(isLoading);
   const hasNextPage = useSelector(hasMoreResults);
   const likes = useSelector(getLikes);

   const [infinityScrollRef] = useInfiniteScroll({
      loading,
      hasNextPage,
      onLoadMore: useAction(dispatch, getPaginatedMoviesFromMovieLibrary.actions['user scrolls down the movie list']),
      rootMargin: '0px 0px 200px 0px',
      delayInMs: 300,
   });

   const cards = movies.map(x => (
      <div key={x.id} className="w-64 h-64 m-6 flex flex-col justify-end shadow-lg border-2 border-gray-200" style={getLabelStyle(x.id)}>
         <div className="flex-1 p-2">
            <div className="flex flex-wrap">
               {x.genres.map(y => (
                  <div key={y} className="p-1 pl-2 pr-2 m-1 bg-white rounded-xl">
                     {y}
                  </div>
               ))}
            </div>
         </div>

         <div className="p-2">
            <button role="button" className="text-5xl text-white" title={likes[x.id] ? 'Dislike' : 'Like'} onClick={likes[x.id] ? useAction(dispatch, likeOrDislikeMovie.actions['user dislikes a movie'], x.id) : useAction(dispatch, likeOrDislikeMovie.actions['user likes a movie'], x.id)}>
               {likes[x.id] ? '♥' : '♡'}
            </button>
         </div>

         <div className="bg-gray-500 bg-opacity-40 text-2xl text-white uppercase text-center p-2">
            {x.name}
         </div>
      </div>
   ));

   return (
      <Page>
         <div className="flex flex-wrap justify-center items-center">
            {cards}
         </div>

         {loading || hasNextPage ? (
            <div ref={infinityScrollRef}>
               Loading...
            </div>
         ) : null}
      </Page>
   );
};
