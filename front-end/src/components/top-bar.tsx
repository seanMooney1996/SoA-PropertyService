import { GalleryVerticalEnd } from "lucide-react"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Separator } from "@/components/ui/separator";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { AuthContext } from "@/context/AuthContext";
import { useContext } from "react";
import { log } from "console";
import { useNavigate } from "react-router-dom";



export function TopBar({
    title,
  ...props
}:  React.ComponentProps<"div">) {
 const { logout,user } = useContext(AuthContext)!;
 const navigate = useNavigate();
 console.log(user?.fname)
  return (
        <div className="flex items-center justify-between mb-6">
          <h1 className="text-2xl font-bold">{title}</h1>

          <div className="flex items-center gap-3">
            <Avatar>
              <AvatarFallback>{user?.fname.charAt(0)}</AvatarFallback>
            </Avatar>
            <span className="font-medium">{user?.fname}</span>
            <Button onClick={() => {logout(); navigate("/login", { replace: true });}}>Logout</Button>
          </div>
        </div>
  )
}
