import { Button } from "@/components/ui/button";

interface SidebarProps {
  active: string;
  onNavigate: (section: string) => void;
  role: "landlord" | "tenant";
}

export function Sidebar({ active, onNavigate, role }: SidebarProps) {
  const landlordNav = [
    "Overview",
    "Properties",
    "Tenants",
    "Payments",
    "Requests",
  ];

  const tenantNav = ["Overview", "My Rental", "Open Rentals", "Requests"];

  const navItems = role === "landlord" ? landlordNav : tenantNav;

  return (
    <aside className="w-64 border-r bg-white p-6 hidden md:block">
      <h2 className="text-xl font-semibold mb-6">
        {role === "landlord" ? "Landlord Panel" : "Tenant Panel"}
      </h2>

      <nav className="space-y-3">
        {navItems.map((item) => (
          <Button
            key={item}
            className="w-full justify-start"
            variant={active === item ? "default" : "ghost"}
            onClick={() => onNavigate(item)}
          >
            {item}
          </Button>
        ))}
      </nav>
    </aside>
  );
}
