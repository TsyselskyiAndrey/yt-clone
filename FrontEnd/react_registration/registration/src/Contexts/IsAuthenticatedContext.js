import { createContext, useState, useRef, useEffect } from 'react';
import useRefreshToken from '../hooks/useRefreshToken';

export const IsAuthenticatedContext = createContext({});

export function IsAuthenticatedProvider({ children }) {
  const [isLoading, setIsLoading] = useState(true);
  const [auth, setAuth] = useState(null);
  const refresh = useRefreshToken();


  useEffect(() => {
    const checkAuth = async () => {
      setIsLoading(true);
      const accessToken = localStorage.getItem('accessToken');

      if (!accessToken) {
        setIsLoading(false);
        setAuth(null);
        return;
      }
      try {
        const newData = await refresh();
        const newAccessToken = newData?.accessToken;
        if (newAccessToken) {
          setAuth(newData.user);
        }
        else {
          setAuth(null);
        }
      } catch (error) {
        console.log(error)
      } finally{
        setIsLoading(false);
      }
      
      
    }
    checkAuth();


  }, []);


  return (
    <IsAuthenticatedContext.Provider value={{ isLoading, setIsLoading, auth, setAuth }}>
      {children}
    </IsAuthenticatedContext.Provider>
  );
}