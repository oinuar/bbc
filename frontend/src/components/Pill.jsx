import {
   React,
} from '@/components';

export default ({ children }) => {
   return (
      <div className="p-1 pl-2 pr-2 m-1 bg-white rounded-xl">
         {children}
      </div>
   );
};
