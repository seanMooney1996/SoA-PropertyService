import { useState, useContext } from "react";
import api from "@/api/client";
import { AuthContext } from "@/context/AuthContext";
import { AuthForm } from "@/components/auth-form";
import { Card, CardContent } from "@/components/ui/card";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const { login } = useContext(AuthContext)!;

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
   const navigate = useNavigate();

  const submit = async () => {
    try {
      const res = await api.post("/auth/login", { email, password });
      login(
        { userId: res.data.userId, email: res.data.email },
        res.data.token
      );
      navigate("/", { replace: true });
    } catch {
      alert("Login failed.");
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50 px-4">
      <Card className="w-full max-w-md shadow-lg border">
        <CardContent className="pt-6">
          <AuthForm
            mode="login"
            email={email}
            password={password}
            onEmailChange={setEmail}
            onPasswordChange={setPassword}
            onSubmit={submit}
          />
        </CardContent>
      </Card>
    </div>
  );
}
