import { useContext } from "react";
import {IsAuthenticatedContext} from "../Contexts/IsAuthenticatedContext"

const useAuth = () => {
    return useContext(IsAuthenticatedContext);
}

export default useAuth;