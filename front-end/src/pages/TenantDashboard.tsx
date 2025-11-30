import { useEffect, useState } from "react"
import api from "@/api/client"

import { Separator } from "@/components/ui/separator"
import { TopBar } from "@/components/top-bar"
import { Sidebar } from "@/components/sidebar"
import { StatCard } from "@/components/stat-card"
import OpenRentalsTable from "@/components/tenant-components/open-rentals-table"
import { toast } from "sonner"


interface TenantPropertyDto {
  id: string
  addressLine1: string
  city: string
  county: string
  bedrooms: number
  bathrooms: number
  rentPrice: number
}

export default function TenantDashboard() {
  const [activeSection, setActiveSection] = useState("Overview")
  const [myRental, setMyRental] = useState<TenantPropertyDto | null>(null)
  const [openRentals, setOpenRentals] = useState<TenantPropertyDto[]>([])
  const [loading, setLoading] = useState(true)

  const fetchData = async () => {
    try {
      const rentalRes = await api.get("/Tenant/myRental")
      const openRes = await api.get("/Tenant/openRentals")

      setMyRental(rentalRes.data)
      setOpenRentals(openRes.data)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchData()
  }, [])

  if (loading) return <p>Loading...</p>

  const rentalActive = myRental && myRental.id != null
  const rentalStatus = rentalActive ? "Active Lease" : "Not Renting"
  const monthlyRent = rentalActive ? `€${myRental!.rentPrice}` : "N/A"
  console.log("myRental "+myRental)
  console.log("rentalStatus "+rentalStatus)
  console.log("monthlyRent "+monthlyRent)


  const requestRental = async (id: string) => {
    try {
      const res = await api.post(`/Tenant/request/${id}`);
      toast.success("Request Sent", {
        description: "Your request for this property was submitted."
      });
      fetchData();
    } catch (err: any) {
      const message =
        err?.response?.data ?? "Failed to request property. Try again later.";

      toast.error("Request Failed", {
        description: message
      });

      console.error("Error sending request:", err);
    }
  };


  return (
    <div className="flex h-screen w-full">
      <Sidebar
        active={activeSection}
        onNavigate={setActiveSection}
        role="tenant"
      />

      <main className="flex-1 p-6">
        <TopBar title={activeSection} />
        <Separator className="mb-6" />

        {activeSection === "Overview" && (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            <StatCard title="Rental Status" value={rentalStatus} />
            <StatCard title="Monthly Rent" value={monthlyRent} />
            <StatCard title="City" value={myRental?.city || "N/A"} />
          </div>
        )}

        {activeSection === "My Rental" && (
          <div className="space-y-4">
            {myRental ? (
              <>
                <h2 className="text-2xl font-semibold">Your Rental Property</h2>

                <div className="p-4 border rounded-md shadow-sm bg-white space-y-2">
                  <p><strong>Address:</strong> {myRental.addressLine1}</p>
                  <p><strong>City:</strong> {myRental.city}</p>
                  <p><strong>County:</strong> {myRental.county}</p>
                  <p><strong>Bedrooms:</strong> {myRental.bedrooms}</p>
                  <p><strong>Bathrooms:</strong> {myRental.bathrooms}</p>
                  <p><strong>Monthly Rent:</strong> €{myRental.rentPrice}</p>
                  <p><strong>Status:</strong> {rentalStatus}</p>
                </div>
              </>
            ) : (
              <p className="text-muted-foreground">You are not currently renting any property.</p>
            )}
          </div>
        )}

        {activeSection === "Open Rentals" && (
          <div>
            <h2 className="text-2xl font-semibold mb-4">Available Properties</h2>
            <OpenRentalsTable
              rentals={openRentals}
              onRequest={requestRental}
            />
          </div>
        )}

        {activeSection === "Payments" && (
          <p className="text-muted-foreground">Payments</p>
        )}
      </main>
    </div>
  )
}
