import {Outlet , Navigate} from "react-router-dom";
import useAuth from "../hooks/useAuth";

const ProtectedRoutes = () => {
    const {auth, isLoading} = useAuth();
    if (isLoading) {
        return <></>;
    }
    if(!auth || !auth?.roles?.includes("channel_owner")){
        return <Navigate to="/auth" replace={true}></Navigate>
    }
    else{
        return <Outlet/>
    }

}

export default ProtectedRoutes;