import {
   React,
} from '@/components';

import Header from '@/components/Header';

export default ({ children }) => {
   return (
      <React.Fragment>
         <Header />

         <div className="p-10">
            {children}
         </div>
      </React.Fragment>
   );
};
