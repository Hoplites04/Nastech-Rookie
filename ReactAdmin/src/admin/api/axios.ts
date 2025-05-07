
import axios from 'axios';
import userManager from '../../auth/authConfig';

const instance = axios.create({
  baseURL: 'https://localhost:7251', // ✅ Your Resource Server URL
  headers: {
    'Content-Type': 'application/json',
  },
});

// 🔐 Add the access token to every request
instance.interceptors.request.use(async (config) => {
  const user = await userManager.getUser();

  if (user && !user.expired) {
    config.headers.Authorization = `Bearer ${user.access_token}`;
  }

  return config;
}, (error) => {
  return Promise.reject(error);
});

export default instance;
