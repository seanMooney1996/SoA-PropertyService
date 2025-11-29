import api from "@/api/client";
import { createContext, useState, useEffect, ReactNode } from "react";

export interface User {
  userId: string;
  email: string;
  fname: string;
  role: string;
}

interface AuthContextType {
  user: User | null;
  login: (user: User) => void;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    const saved = localStorage.getItem("user");
    if (saved) {
      setUser(JSON.parse(saved));
    }
  }, []);

  const login = (user: User) => {
    localStorage.setItem("user", JSON.stringify(user));
    setUser(user);
  };


  const logout = async () => {
    try {
      await api.post("/Auth/logout", {}, { withCredentials: true });
    } catch {
      console.warn("Logout failed ");
    }

    localStorage.clear();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}
