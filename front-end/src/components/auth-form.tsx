import { GalleryVerticalEnd } from "lucide-react"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  Field,
  FieldDescription,
  FieldGroup,
  FieldLabel,
  FieldSeparator,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"

type AuthFormProps = {
  mode: "login" | "signup"
  email: string
  password: string
  onEmailChange: (value: string) => void
  onPasswordChange: (value: string) => void
  onSubmit: () => void
}

export function AuthForm({
  mode,
  email,
  password,
  onEmailChange,
  onPasswordChange,
  onSubmit,
  className,
  ...props
}: AuthFormProps & React.ComponentProps<"div">) {
  const isLogin = mode === "login"

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <form
        onSubmit={(e) => {
          e.preventDefault()
          onSubmit()
        }}
      >
        <FieldGroup>
          <div className="flex flex-col items-center gap-2 text-center ">
            <a className="flex flex-col items-center gap-2 font-medium">
              <div className="flex size-8 items-center justify-center rounded-md">
                <GalleryVerticalEnd className="size-6" />
              </div>
              <span className="sr-only">Property Platform</span>
            </a>

            <h1 className="text-xl font-bold">
              {isLogin ? "Welcome Back" : "Create an Account"}
            </h1>

            <FieldDescription>
              {isLogin ? (
                <>
                  Don&apos;t have an account?{" "}
                  <a href="/signup" className="underline">
                    Sign up
                  </a>
                </>
              ) : (
                <>
                  Already have an account?{" "}
                  <a href="/login" className="underline">
                    Log in
                  </a>
                </>
              )}
            </FieldDescription>
          </div>

          <Field>
            <FieldLabel>Email</FieldLabel>
            <Input
              type="email"
              placeholder="m@example.com"
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
            <Button type="submit">
              {isLogin ? "Login" : "Create Account"}
            </Button>
          </Field>
        </FieldGroup>
      </form>

      <FieldDescription className="px-6 text-center">
        By continuing, you agree to our{" "}
        <a className="underline">Terms of Service</a> and{" "}
        <a className="underline">Privacy Policy</a>.
      </FieldDescription>
    </div>
  )
}
