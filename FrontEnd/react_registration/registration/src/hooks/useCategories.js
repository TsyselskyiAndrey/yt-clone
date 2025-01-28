import { useContext } from "react";
import { CategoryContext } from "../Contexts/CategoryContext"

const useCategories = () => {
    return useContext(CategoryContext);
}

export default useCategories;