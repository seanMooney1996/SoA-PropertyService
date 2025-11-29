import { Button } from "@/components/ui/button"

interface SidebarProps {
  active: string
  onNavigate: (section: string) => void
}

export function Sidebar({ active, onNavigate }: SidebarProps) {
  return (
    <aside className="w-64 border-r bg-white p-6 hidden md:block">
      <h2 className="text-xl font-semibold mb-6">Property Service</h2>

      <nav className="space-y-3">
        <Button
          className="w-full justify-start"
          variant={active === "Overview" ? "default" : "ghost"}
          onClick={() => onNavigate("Overview")}
        >
          Overview
        </Button>

        <Button
          className="w-full justify-start"
          variant={active === "Properties" ? "default" : "ghost"}
          onClick={() => onNavigate("Properties")}
        >
          Properties
        </Button>

        <Button
          className="w-full justify-start"
          variant={active === "Tenants" ? "default" : "ghost"}
          onClick={() => onNavigate("Tenants")}
        >
          Tenants
        </Button>

        <Button
          className="w-full justify-start"
          variant={active === "Payments" ? "default" : "ghost"}
          onClick={() => onNavigate("Payments")}
        >
          Payments
        </Button>
      </nav>
    </aside>
  )
}
