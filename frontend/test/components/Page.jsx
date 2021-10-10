import { React } from '@/components';
import renderer from 'react-test-renderer';

import Page from '@/components/Page';
import Header from '@/components/Header';

it('renders', () => {
   const tree = renderer.create(<Page>unit test page content</Page>);
   const header = renderer.create(<Header />);

   expect(tree.toJSON()).toEqual([
      header.toJSON(),
      {
         "type": "div",
         "props": {
            "className": "p-10"
         },
         "children": [
            "unit test page content"
         ]
      }
   ]);
});
