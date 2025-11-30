import { useEffect, useState } from "react";
import api from "@/api/client";

import { Separator } from "@/components/ui/separator";
import { TopBar } from "@/components/top-bar";
import { Sidebar } from "@/components/sidebar";
import { DashboardMain } from "@/components/dashboard-main";
import { StatCard } from "@/components/stat-card";
import RequestsTable from "@/components/requests-table";

interface PropertyDto {
  id: string;
  addressLine1: string;
  city: string;
  county: string;
  bedrooms: number;
  bathrooms: number;
  rentPrice: number;
  isAvailable: boolean;
}

interface RequestsDto {
  id: string;
  propertyId: string;
  tenantId: string;
  status: string;
  city: string;
  requestedAt: string;
  tenantName: string;
  address: string;
}

export default function LandlordDashboard() {
  const [activeSection, setActiveSection] = useState("Overview");
  const [properties, setProperties] = useState<PropertyDto[]>([]);
  const [requests, setRequests] = useState<RequestsDto[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
    try {
      fetchMyProperties();
      fetchMyRequests();
    } finally {
      setLoading(false);
    }
  };

  const fetchMyProperties = async () => {
    const res = await api.get("/Landlord/properties");
    setProperties(res.data);
  };

  const fetchMyRequests = async () => {
    const res = await api.get("/Landlord/requests");
    console.log("Address in requests[0] ->" + res.data[0].address);
    setRequests(res.data);
  };

  const approveRequest = () => {
    console.log("approved");
  };

  const declineRequest = () => {
    console.log("declined");
  };

  useEffect(() => {
    fetchData();
  }, []);

  if (loading) return <p>Loading...</p>;

  const totalProperties = properties.length;
  const activeTenants = properties.filter((p) => !p.isAvailable).length;
  const availableProperties = properties.filter((p) => p.isAvailable).length;
  const pendingRequestSize = requests.length;
  const monthlyRent = properties.reduce((sum, p) => sum + p.rentPrice, 0);

  return (
    <div className="flex h-screen w-full">
      <Sidebar
        active={activeSection}
        onNavigate={setActiveSection}
        role="landlord"
      />

      <main className="flex-1 p-6">
        <TopBar title={activeSection} />
        <Separator className="mb-6" />

        {activeSection === "Overview" && (
          <>
            <div className="grid grid-cols-2 gap-6">
              <StatCard title="Total Properties" value={totalProperties} />
              <StatCard title="Active Tenants" value={activeTenants} />
              <StatCard
                title="Available Properties"
                value={availableProperties}
              />
              <StatCard title="Pending Requests" value={pendingRequestSize} />
              <StatCard title="Monthly Rent" value={`€${monthlyRent}`} />
            </div>
          </>
        )}

        {activeSection === "Properties" && (
          <DashboardMain properties={properties} reloadProperties={fetchData} />
        )}

        {activeSection === "Tenants" && (
          <p className="text-muted-foreground">Placeholder</p>
        )}

        {activeSection === "Payments" && (
          <p className="text-muted-foreground">Placeholder</p>
        )}

        {activeSection === "Requests" && (
          <RequestsTable
            mode="landlord"
            requests={requests}
            onApprove={(id) => approveRequest(id)}
            onDecline={(id) => declineRequest(id)}
          />
        )}
      </main>
    </div>
  );
}
