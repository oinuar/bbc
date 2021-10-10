import { React } from '@/components';
import renderer from 'react-test-renderer';

import Header from '@/components/Header';

it('renders', () => {
   const tree = renderer.create(<Header />);

   expect(tree.toJSON()).toEqual({
      "type": "div",
      "props": {
         "className": "bg-yellow-200 flex justify-center items-center h-64"
      },
      "children": [
         {
            "type": "div",
            "props": {
               "className": "w-1/2"
            },
            "children": [
               {
                  "type": "div",
                  "props": {
                     "className": "text-5xl m-4"
                  },
                  "children": [
                     "Welcome to Boring Business Case!"
                  ]
               },
               {
                  "type": "div",
                  "props": {
                     "className": "text-3xl m-4"
                  },
                  "children": [
                     "We recommend movies if you like them with a stupid algorithm."
                  ]
               }
            ]
         }
      ]
   });
});
