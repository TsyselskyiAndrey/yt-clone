import axios from "axios";
const BASE_URL = 'https://localhost:44302';

export default axios.create({
    baseURL: BASE_URL
})

export const axiosWithToken = axios.create({
    baseURL: BASE_URL
})