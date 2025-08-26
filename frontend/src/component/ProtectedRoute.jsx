import { Navigate } from "react-router-dom";
import { useSelector } from "react-redux";

const ProtectedRoute = ({ children }) => {
  const { username, role } = useSelector((state) => state.auth);

  // Check if user is logged in
  if (!username) {
    console.log("User not logged in, redirecting to /login");
    return <Navigate to="/login" replace />;
  }

  // Trace the role (log it for now)
  console.log("User role:", role);

  return children; // Allow access if logged in
};

export default ProtectedRoute;