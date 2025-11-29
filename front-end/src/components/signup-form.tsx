import { GalleryVerticalEnd } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import {
  Field,
  FieldGroup,
  FieldLabel,
  FieldDescription,
} from "@/components/ui/field"

type SignupFormProps = {
  email: string
  password: string
  role: string

  firstName: string
  lastName: string

  phone: string
  companyName: string

  onEmailChange: (v: string) => void
  onPasswordChange: (v: string) => void
  onRoleChange: (v: string) => void

  onFirstNameChange: (v: string) => void
  onLastNameChange: (v: string) => void

  onPhoneChange: (v: string) => void
  onCompanyNameChange: (v: string) => void

  onSubmit: () => void
}

export function SignupForm({
  email,
  password,
  role,
  firstName,
  lastName,
  phone,
  companyName,
  onEmailChange,
  onPasswordChange,
  onRoleChange,
  onFirstNameChange,
  onLastNameChange,
  onPhoneChange,
  onCompanyNameChange,
  onSubmit,
}: SignupFormProps) {
  return (
    <form
      onSubmit={(e) => {
        e.preventDefault()
        onSubmit()
      }}
      className="flex flex-col gap-6"
    >
      <div className="flex flex-col items-center gap-2 text-center">
        <div className="flex size-8 items-center justify-center rounded-md">
          <GalleryVerticalEnd className="size-6" />
        </div>

        <h1 className="text-xl font-bold">Create an Account</h1>

        <FieldDescription>
          Already have an account?{" "}
          <a href="/login" className="underline">
            Log in
          </a>
        </FieldDescription>
      </div>

      <FieldGroup>
        <Field>
          <FieldLabel>Email</FieldLabel>
          <Input
            type="email"
            placeholder="you@example.com"
            value={email}
            onChange={(e) => onEmailChange(e.target.value)}
            required
          />
        </Field>

        <Field>
          <FieldLabel>Password</FieldLabel>
          <Input
            type="password"
            placeholder="••••••••"
            value={password}
            onChange={(e) => onPasswordChange(e.target.value)}
            required
          />
        </Field>

        <Field>
          <FieldLabel>Account Type</FieldLabel>
          <select
            value={role}
            onChange={(e) => onRoleChange(e.target.value)}
            className="border rounded-md p-2"
            required
          >
            <option value="">Choose type</option>
            <option value="Tenant">Tenant</option>
            <option value="Landlord">Landlord</option>
          </select>
        </Field>

        {/* SHARED FIELDS */}
        {(role === "Tenant" || role === "Landlord") && (
          <>
            <Field>
              <FieldLabel>First Name</FieldLabel>
              <Input
                value={firstName}
                onChange={(e) => onFirstNameChange(e.target.value)}
                required
              />
            </Field>

            <Field>
              <FieldLabel>Last Name</FieldLabel>
              <Input
                value={lastName}
                onChange={(e) => onLastNameChange(e.target.value)}
                required
              />
            </Field>
          </>
        )}

        {/* LANDLORD-ONLY FIELDS */}
        {role === "Landlord" && (
          <>
            <Field>
              <FieldLabel>Phone</FieldLabel>
              <Input
                placeholder="087 123 4567"
                value={phone}
                onChange={(e) => onPhoneChange(e.target.value)}
                required
              />
            </Field>

            <Field>
              <FieldLabel>Company Name</FieldLabel>
              <Input
                placeholder="Company Ltd."
                value={companyName}
                onChange={(e) => onCompanyNameChange(e.target.value)}
              />
            </Field>
          </>
        )}

        <Button type="submit" className="w-full">
          Create Account
        </Button>
      </FieldGroup>

      <FieldDescription className="text-center text-sm">
        By continuing, you agree to our Terms and Privacy Policy.
      </FieldDescription>
    </form>
  )
}
