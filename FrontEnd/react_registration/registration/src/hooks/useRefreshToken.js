import axios from "../API/axioscfg";

let isRefreshing = false; // Флаг для отслеживания текущего состояния
let refreshPromise = null; // Хранение текущего Promise, чтобы повторно использовать его

const useRefreshToken = () => {
    const refresh = async () => {
        if (isRefreshing) {
            // Если уже идет запрос, возвращаем существующий Promise
            return refreshPromise;
        }

        isRefreshing = true; // Устанавливаем блокировку
        refreshPromise = new Promise(async (resolve, reject) => {
            try {
                console.log("refresh called");
                const token = localStorage.getItem("accessToken");
                const response = await axios.post(
                    "/api/auth/refresh-token",
                    { accessToken: token },
                    {
                        headers: { 'Content-Type': 'application/json' },
                        withCredentials: true,
                    }
                );

                if (response.data.accessToken) {
                    localStorage.setItem("accessToken", response.data.accessToken);
                    resolve({
                        accessToken: response.data.accessToken,
                        user: response.data.user,
                    });
                } else {
                    resolve(null);
                }
            } catch (error) {
                console.error("Error during token refreshment:", error.response || error.message);
                reject(error);
            } finally {
                isRefreshing = false; // Сбрасываем блокировку
                refreshPromise = null; // Очищаем Promise
            }
        });

        return refreshPromise;
    };

    return refresh;
};

export default useRefreshToken;
