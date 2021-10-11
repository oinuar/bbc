import renderer from 'react-test-renderer';
import { React } from '@/components';

import Page from '@/components/Page';
import Header from '@/components/Header';

it('renders', () => {
  const content = 'unit test page content';
  const tree = renderer.create(<Page>{content}</Page>);
  const header = renderer.create(<Header />);

  expect(tree.toJSON()).toEqual([
    header.toJSON(),
    {
      type: 'div',
      props: {
        className: 'p-10',
      },
      children: [
        content,
      ],
    },
  ]);
});
