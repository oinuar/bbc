import {
  React, useSelector, useAction, useDispatch,
} from '@/components';

import Pill from '@/components/Pill';

import likeOrDislikeMovie, {
  getLike,
} from '@/scenarios/LikeOrDislikeMovie';

function getCoverStyle(id) {
  switch (id) {
    case 'tt0118715':
      return {
        backgroundImage: 'url(https://m.media-amazon.com/images/M/MV5BMTQ0NjUzMDMyOF5BMl5BanBnXkFtZTgwODA1OTU0MDE@._V1_.jpg)',
        backgroundPosition: '63% 27%',
      };

    case 'tt0068646':
      return {
        backgroundImage: 'url(https://m.media-amazon.com/images/M/MV5BM2MyNjYxNmUtYTAwNi00MTYxLWJmNWYtYzZlODY3ZTk3OTFlXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_.jpg)',
        backgroundPosition: '50% 15%',
      };

    case 'tt0088763':
      return {
        backgroundImage: 'url(https://m.media-amazon.com/images/M/MV5BZmU0M2Y1OGUtZjIxNi00ZjBkLTg1MjgtOWIyNThiZWIwYjRiXkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_.jpg)',
        backgroundPosition: '65% 28%',
      };

    case 'tt0111161':
      return {
        backgroundImage: 'url(https://m.media-amazon.com/images/M/MV5BMDFkYTc0MGEtZmNhMC00ZDIzLWFmNTEtODM1ZmRlYWMwMWFmXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_.jpg)',
        backgroundPosition: '50% 15%',
      };

    case 'tt0163651':
      return {
        backgroundImage: 'url(https://m.media-amazon.com/images/M/MV5BMTg3ODY5ODI1NF5BMl5BanBnXkFtZTgwMTkxNTYxMTE@._V1_.jpg)',
        backgroundPosition: '50% 27%',
      };

    case 'tt0848228':
      return {
        backgroundImage: 'url(https://m.media-amazon.com/images/M/MV5BNDYxNjQyMjAtNTdiOS00NGYwLWFmNTAtNThmYjU5ZGI2YTI1XkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_.jpg)',
        backgroundPosition: '38% 27%',
      };

    case 'tt2527336':
      return {
        backgroundImage: 'url(https://m.media-amazon.com/images/M/MV5BMjQ1MzcxNjg4N15BMl5BanBnXkFtZTgwNzgwMjY4MzI@._V1_.jpg)',
        backgroundPosition: '28% 30%',
      };

    default:
      break;
  }

  return {};
}

export default ({ id, genres, name }) => {
  const dispatch = useDispatch();

  const hasLike = useSelector(getLike.bind(null, id));
  const onLikeOrDislike = hasLike
    ? useAction(dispatch, likeOrDislikeMovie.actions['user dislikes a movie'], id)
    : useAction(dispatch, likeOrDislikeMovie.actions['user likes a movie'], id);

  const pills = genres?.map(label => (
    <Pill key={label}>
      {label}
    </Pill>
  ));

  return (
    <div className="w-64 h-72 m-6 flex flex-col justify-end shadow-lg border-2 border-gray-200" style={getCoverStyle(id)}>
      <div className="flex-1 p-2">
        <div className="flex flex-wrap">
          {pills}
        </div>
      </div>

      <div className="p-2">
        <button type="button" className="text-5xl text-white" title={hasLike ? 'Dislike' : 'Like'} onClick={onLikeOrDislike}>
          {hasLike ? '♥' : '♡'}
        </button>
      </div>

      <div className="bg-gray-500 bg-opacity-40 text-2xl text-white uppercase text-center p-2">
        {name}
      </div>
    </div>
  );
};
