import { Navigate,  useLocation } from "react-router-dom";

const ProtectedRoute = ({children})=>{

    const token = sessionStorage.getItem("token")
    const location = useLocation();
    if(token)
        return children;
    return <Navigate to="/login" replace state = {{from:location}}/>

}

export default ProtectedRoute;