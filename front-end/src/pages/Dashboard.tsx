import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { TopBar } from "@/components/top-bar";

export default function DashboardPage() {
  return (
    <div className="flex h-screen w-full">
      {/* Sidebar */}
      <aside className="w-64 border-r bg-white p-6 hidden md:block">
        <h2 className="text-xl font-semibold mb-6">Dashboard</h2>

        <nav className="space-y-3">
          <Button className="w-full justify-start" variant="ghost">
            Overview
          </Button>
          <Button className="w-full justify-start" variant="ghost">
            Properties
          </Button>
          <Button className="w-full justify-start" variant="ghost">
            Tenants
          </Button>
          <Button className="w-full justify-start" variant="ghost">
            Payments
          </Button>
        </nav>
      </aside>

      {/* Main */}
      <main className="flex-1 p-6">
        {/* Top Bar */}
        <TopBar></TopBar>

        <Separator className="mb-6" />

        {/* Grid Cards */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          <Card>
            <CardHeader>
              <CardTitle>Total Properties</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-3xl font-bold">12</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Active Tenants</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-3xl font-bold">8</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Pending Applications</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-3xl font-bold">3</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Monthly Rent</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-3xl font-bold">€6,450</p>
            </CardContent>
          </Card>
        </div>

        {/* Actions Section */}
        <div className="mt-8">
          <h2 className="text-xl font-semibold mb-3">Quick Actions</h2>

          <div className="flex gap-4">
            <Button>Add Property</Button>
          </div>
        </div>
      </main>
    </div>
  );
}
