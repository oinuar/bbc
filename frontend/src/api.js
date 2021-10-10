import axios from 'axios';
import { getApiUrl } from '@/config';

const instance = axios.create({
   baseURL: getApiUrl(),
});

export function useApi() {
   return instance;
}

export function makeUserHeader(accessToken) {
   return {
      Authorization: `Bearer ${accessToken}`,
   };
}
