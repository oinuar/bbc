import renderer from 'react-test-renderer';
import {
  React, useSelector, useDispatch, useAction,
} from '@/components';

import MovieCard from '@/components/MovieCard';
import Pill from '@/components/Pill';

import likeOrDislikeMovie, { getLike } from '@/scenarios/LikeOrDislikeMovie';

jest.mock('@/components', () => ({
  ...jest.requireActual('@/components'),
  useSelector: jest.fn(),
  useAction: jest.fn(),
  useDispatch: jest.fn(),
}));

jest.mock('@/scenarios/LikeOrDislikeMovie', () => ({
  __esModule: true,
  default: {
    actions: {
      'user likes a movie': jest.fn(),
      'user dislikes a movie': jest.fn(),
    },
  },
  getLike: jest.fn(),
}));

beforeEach(() => {
  useSelector.mockImplementation(f => f());
});

function getExpectedTree(style, genres, name, likeTitle, heartSymbol) {
  return {
    type: 'div',
    props: {
      className: 'w-64 h-72 m-6 flex flex-col justify-end shadow-lg border-2 border-gray-200',
      style,
    },
    children: [
      {
        type: 'div',
        props: {
          className: 'flex-1 p-2',
        },
        children: [
          {
            type: 'div',
            props: {
              className: 'flex flex-wrap',
            },
            children: genres,
          },
        ],
      },
      {
        type: 'div',
        props: {
          className: 'p-2',
        },
        children: [
          {
            type: 'button',
            props: {
              role: 'button',
              className: 'text-5xl text-white',
              title: likeTitle,
            },
            children: [
              heartSymbol,
            ],
          },
        ],
      },
      {
        type: 'div',
        props: {
          className: 'bg-gray-500 bg-opacity-40 text-2xl text-white uppercase text-center p-2',
        },
        children: name,
      },
    ],
  };
}

it('renders name', () => {
  const name = 'unit test movie name';
  const tree = renderer.create(<MovieCard name={name} />);

  expect(tree.toJSON()).toEqual(getExpectedTree({}, null, [name], 'Like', '♡'));
});

it('renders genres', () => {
  const genres = ['genre 1', 'genre 2'];
  const pills = renderer.create(genres.map(label => <Pill key={label}>{label}</Pill>));
  const tree = renderer.create(<MovieCard genres={genres} />);

  expect(tree.toJSON()).toEqual(getExpectedTree({}, pills.toJSON(), null, 'Like', '♡'));
});

it('renders like button', () => {
  const dispatch = {};
  const id = 'unit test movie id';

  useDispatch.mockReturnValue(dispatch);
  getLike.mockReturnValue(false);

  const tree = renderer.create(<MovieCard id={id} />);

  expect(tree.toJSON()).toEqual(getExpectedTree({}, null, null, 'Like', '♡'));
  expect(useAction).toHaveBeenCalledWith(dispatch, likeOrDislikeMovie.actions['user likes a movie'], id);
});

it('renders dislike button', () => {
  const dispatch = {};
  const id = 'unit test movie id';

  useDispatch.mockReturnValue(dispatch);
  getLike.mockReturnValue(true);

  const tree = renderer.create(<MovieCard id={id} />);

  expect(tree.toJSON()).toEqual(getExpectedTree({}, null, null, 'Dislike', '♥'));
  expect(useAction).toHaveBeenCalledWith(dispatch, likeOrDislikeMovie.actions['user dislikes a movie'], id);
});

it('acts on like button', () => {
  const id = 'unit test movie id';
  const act = jest.fn();

  useAction.mockImplementation((_, ...args) => () => act(...args));
  getLike.mockReturnValue(false);

  const tree = renderer.create(<MovieCard id={id} />);

  tree.root.findByType('button').props.onClick();

  expect(act).toHaveBeenCalledWith(likeOrDislikeMovie.actions['user likes a movie'], id);
});

it('acts on dislike button', () => {
  const id = 'unit test movie id';
  const act = jest.fn();

  useAction.mockImplementation((_, ...args) => () => act(...args));
  getLike.mockReturnValue(true);

  const tree = renderer.create(<MovieCard id={id} />);

  tree.root.findByType('button').props.onClick();

  expect(act).toHaveBeenCalledWith(likeOrDislikeMovie.actions['user dislikes a movie'], id);
});
