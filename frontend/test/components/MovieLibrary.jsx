import renderer from 'react-test-renderer';

import useInfiniteScroll from 'react-infinite-scroll-hook';
import {
  React, useSelector, useDispatch, useAction,
} from '@/components';

import MovieLibrary from '@/components/MovieLibrary';
import MovieCard from '@/components/MovieCard';

import getPaginatedMoviesFromMovieLibrary, {
  getAll, isLoading, hasMoreResults,
} from '@/scenarios/GetPaginatedMoviesFromMovieLibrary';

jest.mock('react-infinite-scroll-hook', () => ({
  __esModule: true,
  default: jest.fn(),
}));

jest.mock('@/components', () => ({
  ...jest.requireActual('@/components'),
  useSelector: jest.fn(),
  useAction: jest.fn(),
  useDispatch: jest.fn(),
}));

jest.mock('@/components/MovieCard', () => ({
  __esModule: true,
  default: props => (
    <p>
      movie card:
      {' '}
      {JSON.stringify(props)}
    </p>
  ),
}));

jest.mock('@/scenarios/GetPaginatedMoviesFromMovieLibrary', () => ({
  __esModule: true,
  default: {
    actions: {
      'user scrolls down the movie list': jest.fn(),
    },
  },
  getAll: jest.fn(),
  isLoading: jest.fn(),
  hasMoreResults: jest.fn(),
}));

beforeEach(() => {
  useSelector.mockImplementation(f => f());
  useInfiniteScroll.mockReturnValue([]);
});

function getExpectedTree(movies, showLoadingIndicator) {
  const cards = {
    type: 'div',
    props: {
      className: 'flex flex-wrap justify-center items-center',
    },
    children: movies,
  };

  if (showLoadingIndicator) {
    return [
      cards,
      {
        type: 'div',
        props: {},
        children: [
          'Loading...',
        ],
      },
    ];
  }

  return cards;
}

it('hooks infinite scroll', () => {
  const dispatch = {};
  const loading = false;
  const hasNextPage = false;
  const onLoadMore = jest.fn();

  useDispatch.mockReturnValue(dispatch);
  isLoading.mockReturnValue(loading);
  hasMoreResults.mockReturnValue(hasNextPage);
  useAction.mockImplementation(() => onLoadMore);

  renderer.create(<MovieLibrary />);

  expect(useAction).toHaveBeenCalledWith(dispatch, getPaginatedMoviesFromMovieLibrary.actions['user scrolls down the movie list']);
  expect(useInfiniteScroll).toHaveBeenCalledWith({
    loading,
    hasNextPage,
    onLoadMore,
    rootMargin: '0px 0px 200px 0px',
    delayInMs: 300,
  });
});

it('renders movie cards', () => {
  const movies = [
    { id: 1, passThroughProps: 'like this' },
    { id: 2, passThroughOtherProps: 'like this as well' },
  ];

  getAll.mockReturnValue(movies);

  const tree = renderer.create(<MovieLibrary />);
  const cards = renderer.create(movies.map(x => <MovieCard key={x.id} {...x} />));

  expect(tree.toJSON()).toEqual(getExpectedTree(cards.toJSON(), false));
});

describe('render loading indicator', () => {
  function run(visible) {
    const scrollRef = jest.fn();

    useInfiniteScroll.mockReturnValue([scrollRef]);

    const tree = renderer.create(<MovieLibrary />);

    expect(tree.toJSON()).toEqual(getExpectedTree(null, visible));

    if (visible) expect(scrollRef).toHaveBeenCalled();
  }

  it('loads', () => {
    isLoading.mockReturnValue(true);

    run(true);
  });

  it('has more results', () => {
    hasMoreResults.mockReturnValue(true);

    run(true);
  });

  it('is not visible', () => {
    isLoading.mockReturnValue(false);
    hasMoreResults.mockReturnValue(false);

    run(false);
  });
});
