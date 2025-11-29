import { useContext, useState } from "react";
import api from "@/api/client";
import { SignupForm } from "@/components/signup-form";
import { Card, CardContent } from "@/components/ui/card";
import { AuthContext } from "@/context/AuthContext";
import { useNavigate } from "react-router-dom";

export default function SignupPage() {
  const { login } = useContext(AuthContext)!;

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [phone, setPhone] = useState("");
  const [companyName, setCompanyName] = useState("");
  const navigate = useNavigate();

  const submit = async () => {
    try {
      const res = await api.post("/auth/signup", {
        email,
        password,
        role,
        firstName,
        lastName,
        phone,
        companyName,
      });

      login(
        { userId: res.data.userId, email: res.data.email, fname: firstName, role: res.data.role }
      );

      navigate("/", { replace: true });
    } catch {
      alert("Signup failed.");
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50 px-4">
      <Card className="w-full max-w-md shadow-lg border">
        <CardContent className="pt-6">
          <SignupForm
            email={email}
            password={password}
            role={role}
            firstName={firstName}
            lastName={lastName}
            phone={phone}
            companyName={companyName}
            onEmailChange={setEmail}
            onPasswordChange={setPassword}
            onRoleChange={setRole}
            onFirstNameChange={setFirstName}
            onLastNameChange={setLastName}
            onPhoneChange={setPhone}
            onCompanyNameChange={setCompanyName}
            onSubmit={submit}
          />
        </CardContent>
      </Card>
    </div>
  );
}
