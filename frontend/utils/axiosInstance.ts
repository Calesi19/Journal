import axios from "axios";

// Create the Axios instance
const axiosInstance = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true, // Ensure cookies are sent with requests
});

export default axiosInstance;
