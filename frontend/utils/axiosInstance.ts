import axios from "axios";

// Create the Axios instance
const axiosInstance = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

// Add a request interceptor to include the access token
axiosInstance.interceptors.request.use(
  (config) => {
    // Get the access token from local storage
    const accessToken = localStorage.getItem("accessToken");

    if (!accessToken) {
      config.headers["Authorization"] = ``;
    }

    // If the access token exists, include it in the Authorization header
    if (accessToken) {
      config.headers["Authorization"] = `Bearer ${accessToken}`;
    }


    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default axiosInstance;
