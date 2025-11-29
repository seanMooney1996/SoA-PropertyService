import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "../pages/Login";
import Signup from "../pages/Signup";
import LandlordDashboard from "../pages/LandlordDashboard";
import TenantDashboard from "../pages/TenantDashboard";
import PrivateRoute from "./PrivateRoute";
import { useAuth } from "@/hooks/useAuth";
import { AuthContext } from "@/context/AuthContext";
import { useContext } from "react";

export default function AppRouter() {

  function RoleRedirect() {
  const { user } = useAuth();
  if (!user) return <Navigate to="/login" replace />;
  if (user.role === "landlord") return <Navigate to="/landlord" replace />;
  if (user.role === "tenant") return <Navigate to="/tenant" replace />;

  return <Navigate to="/login" replace />;
}
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<Signup/>} />

        <Route path="/" element={<RoleRedirect />} />


        <Route
          path="/landlord"
          element={
            <PrivateRoute>
              <LandlordDashboard />
            </PrivateRoute>
          }
        />

        <Route
          path="/tenant"
          element={
            <PrivateRoute>
              <TenantDashboard />
            </PrivateRoute>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}
