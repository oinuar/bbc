import { React, useSelector, useAction } from '@/components';
import renderer from 'react-test-renderer';

import MovieRecommendations from '@/components/MovieRecommendations';
import MovieCard from '@/components/MovieCard';

import getMovieRecommendationsBasedOnUserGenrePreference, {
   getAll, isLoading, hasMoreResults,
} from '@/scenarios/GetMovieRecommendationsBasedOnUserGenrePreference';

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
         movie card: {JSON.stringify(props)}
      </p>
   ),
}));

jest.mock('@/scenarios/GetMovieRecommendationsBasedOnUserGenrePreference', () => ({
   __esModule: true,
   default: {
      actions: {
         'user requests more movie recommendations': jest.fn(),
      },
   },
   getAll: jest.fn(),
   isLoading: jest.fn(),
   hasMoreResults: jest.fn(),
}));

beforeEach(() => {
   useSelector.mockImplementation(f => f());
});

function getExpectedTree(movies, showLoading, showMore) {
   const children = (movies ?? [])
      .concat(showLoading
         ? [{
               "type": "div",
               "props": {},
               "children": [
               "Loading..."
               ]
            }]
         : [])
      .concat(showMore
         ? [{
               "type": "button",
               "props": {
               "role": "button"
               },
               "children": [
               "Show more"
               ]
            }]
          : []);

   return {
      "type": "div",
      "props": {
         "className": "flex flex-wrap justify-start items-center"
      },
      "children": children.length === 0 ? null : children
   };
}

it('renders movie cards', () => {
   const movies = [
      { id: 1, passThroughProps: 'like this' },
      { id: 2, passThroughOtherProps: 'like this as well' },
   ];

   getAll.mockReturnValue(movies);

   const tree = renderer.create(<MovieRecommendations />);
   const cards = renderer.create(movies.map(x => <MovieCard key={x.id} {...x} />));

   expect(tree.toJSON()).toEqual(getExpectedTree(cards.toJSON()));
});

it('renders loading indicator', () => {
   isLoading.mockReturnValue(true);

   const tree = renderer.create(<MovieRecommendations />);

   expect(tree.toJSON()).toEqual(getExpectedTree(null, true, false));
});

describe('render show more', () => {
   it('has more results', () => {
      isLoading.mockReturnValue(false);
      hasMoreResults.mockReturnValue(true);

      const tree = renderer.create(<MovieRecommendations />);

      expect(tree.toJSON()).toEqual(getExpectedTree(null, false, true));
   });

   it('is hidden when loading', () => {
      isLoading.mockReturnValue(true);
      hasMoreResults.mockReturnValue(true);

      const tree = renderer.create(<MovieRecommendations />);

      expect(tree.toJSON()).toEqual(getExpectedTree(null, true, false));
   });
});

it('acts on show more button', () => {
   const act = jest.fn();

   useAction.mockImplementation((_, ...args) => () => act(...args));
   isLoading.mockReturnValue(false);
   hasMoreResults.mockReturnValue(true);

   const tree = renderer.create(<MovieRecommendations />);

   tree.root.findByType('button').props.onClick();

   expect(act).toHaveBeenCalledWith(getMovieRecommendationsBasedOnUserGenrePreference.actions['user requests more movie recommendations']);
});
