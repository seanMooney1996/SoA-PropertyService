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

  const submit = async () => {
    try {
      const authRes = await api.post("/Auth/signup", {
        email,
        password,
        role,
      });

      const navigate = useNavigate()
      const userId = authRes.data.userId;
      const token = authRes.data.token;

      // set token for authorization since tenant and landlord require it
      localStorage.setItem("token", token);
      api.defaults.headers.common["Authorization"] = `Bearer ${token}`;

      if (role === "Tenant") {
        await api.post("/tenant", {
          id:  authRes.data.userId,
          firstName,
          lastName,
        });
      }

      if (role === "Landlord") {
        await api.post("/landlord", {
          id: userId,
          firstName,
          lastName,
          email,
          phone,
          companyName,
        });
      }

      //put user details and token in local storage
      login(
      { userId: authRes.data.userId, email: authRes.data.email },
      authRes.data.token);

      navigate("/", { replace: true });
    } catch (err) {
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
