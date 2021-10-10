import { React } from '@/components';
import renderer from 'react-test-renderer';

import Pill from '@/components/Pill';

it('renders', () => {
   const tree = renderer.create(<Pill>unit test pill content</Pill>);

   expect(tree.toJSON()).toEqual({
      "type": "div",
      "props": {
         "className": "p-1 pl-2 pr-2 m-1 bg-white rounded-xl"
      },
      "children": [
         "unit test pill content"
      ]
   });
});
