import React from "react";
import ReactDOM from "react-dom/client";
import AppRouter from "./router/AppRouter";
import { AuthProvider } from "./context/AuthContext";
import { Toaster } from "@/components/ui/sonner"
import "./index.css";

const rootElement = document.getElementById("root") as HTMLElement;

ReactDOM.createRoot(rootElement).render(
    <AuthProvider>
      <AppRouter />
       <Toaster />
    </AuthProvider>
);