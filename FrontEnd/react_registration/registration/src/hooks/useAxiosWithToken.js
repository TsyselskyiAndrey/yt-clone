import { axiosWithToken } from "../API/axioscfg"
import { useEffect } from "react"
import useRefreshToken from './useRefreshToken';
import useAuth from "./useAuth";

const useAxiosWithToken = () => {
    const refresh = useRefreshToken();
    const { setAuth } = useAuth();
    useEffect(() => {
        const requestIntercept = axiosWithToken.interceptors.request.use(
            config => {
                if(!config.headers['Authorization']) {
                    config.headers['Authorization'] = `Bearer ${localStorage.getItem("accessToken")}`;
                }
                return config;
            }, (error) => Promise.reject(error)
        )
        const responseIntercept = axiosWithToken.interceptors.response.use(
            async response => {
                const prevRequest = response.config;
                if (response.headers['x-token-expired'] === 'true' && !prevRequest?.sent) {
                    prevRequest.sent = true;
                    try {
                        const newData = await refresh();
                        const newAccessToken = newData?.accessToken;
                        if (newAccessToken) {
                            setAuth(newData?.user);
                            response.config.headers['Authorization'] = `Bearer ${newAccessToken}`;
                            return axiosWithToken(response.config);
                        }
                    } catch (err) {
                        console.error("Failed to refresh token:", err);
                    }
                }
                return response;
            },
            async (error) => {
                const prevRequest = error?.config;
                if((error?.response?.status === 401 || error?.response?.status === 403) && !prevRequest?.sent){
                    prevRequest.sent = true;
                    const newData = await refresh();
                    const newAccessToken = newData?.accessToken;
                    if (newAccessToken) {
                        prevRequest.headers['Authorization'] = `Bearer ${newAccessToken}`;
                        setAuth(newData?.user);
                        return axiosWithToken(prevRequest);
                    }
                }
                return Promise.reject(error)
            }
        )
        return () => {
            axiosWithToken.interceptors.request.eject(requestIntercept);
            axiosWithToken.interceptors.response.eject(responseIntercept);
        }
    }, [refresh])
}

export default useAxiosWithToken;