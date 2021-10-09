import {
   React, useSelector, useAction, useDispatch,
} from '@/components';

import Pill from '@/components/Pill';

import likeOrDislikeMovie, {
   getLike,
} from '@/scenarios/LikeOrDislikeMovie';

function getLabelStyle(id) {
   return {
      backgroundImage: 'url(https://m.media-amazon.com/images/M/MV5BMDFkYTc0MGEtZmNhMC00ZDIzLWFmNTEtODM1ZmRlYWMwMWFmXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_.jpg)',
      backgroundPosition: '50% 15%',
   };
}

export default ({ id, genres, name }) => {
   const dispatch = useDispatch();

   const hasLike = useSelector(getLike.bind(null, id));
   const onLikeOrDislike = hasLike
      ? useAction(dispatch, likeOrDislikeMovie.actions['user dislikes a movie'], id)
      : useAction(dispatch, likeOrDislikeMovie.actions['user likes a movie'], id);

   const pills = genres.map(label => (
      <Pill key={label}>
         {label}
      </Pill>
   ));

   return (
      <div className="w-64 h-64 m-6 flex flex-col justify-end shadow-lg border-2 border-gray-200" style={getLabelStyle(id)}>
         <div className="flex-1 p-2">
            <div className="flex flex-wrap">
               {pills}
            </div>
         </div>

         <div className="p-2">
            <button role="button" className="text-5xl text-white" title={hasLike ? 'Dislike' : 'Like'} onClick={onLikeOrDislike}>
               {hasLike ? '♥' : '♡'}
            </button>
         </div>

         <div className="bg-gray-500 bg-opacity-40 text-2xl text-white uppercase text-center p-2">
            {name}
         </div>
      </div>
   );
};
