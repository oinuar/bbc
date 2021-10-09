import axios from 'axios';

const instance = axios.create({
  baseURL: window.location.origin + '/api',
});

export function useApi() {
   return instance;
}

export function makeUserHeader(accessToken) {
   return {
      Authorization: `Bearer ${accessToken}`,
   };
}
