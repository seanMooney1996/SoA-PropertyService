import { useContext } from "react";
import { Navigate } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import { ReactNode } from "react";

export default function PrivateRoute({ children }: { children: ReactNode }) {
  const auth = useContext(AuthContext);
  console.log("checking private route")
  if (!auth || !auth.user) return <Navigate to="/login" replace />;

  return children;
}
