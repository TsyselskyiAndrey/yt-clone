import { createContext, useState, useRef, useEffect } from 'react';
import { axiosWithToken } from "../API/axioscfg"
import useAxiosWithToken from "../hooks/useAxiosWithToken"

export const CategoryContext = createContext({});

export function CategoryProvider({ children }) {
    const [categories, setCategories] = useState([]);
    useAxiosWithToken()


    useEffect(() => {
        GetCategories();
    }, [])

    async function GetCategories() {
        try {
            const response = await axiosWithToken.get(
                `/api/content/getcategories`,
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setCategories(response.data)
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else {
                console.error('Error during retrieving categories:', error.response?.data || error.message);
            }
        }
    }

    return (
        <CategoryContext.Provider value={{ categories, setCategories, GetCategories }}>
            {children}
        </CategoryContext.Provider>
    );
}