import { useEffect, useState } from "react"
import api from "@/api/client"

import { Button } from "@/components/ui/button"
import { Separator } from "@/components/ui/separator"
import { TopBar } from "@/components/top-bar"
import { DashboardMain } from "@/components/dashboard-main"
import { StatCard } from "@/components/stat-card"

interface PropertyDto {
  addressLine1: string
  city: string
  county: string
  bedrooms: number
  bathrooms: number
  rentPrice: number
  isAvailable: boolean
}

export default function DashboardPage() {
  const [properties, setProperties] = useState<PropertyDto[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await api.get("/Landlord/mine")
        setProperties(res.data)
      } finally {
        setLoading(false)
      }
    }
    fetchData()
  }, [])

  if (loading) return <p>Loading...</p>

  const totalProperties = properties.length
  const activeTenants = properties.filter(p => !p.isAvailable).length
  const availableProperties = properties.filter(p => p.isAvailable).length
  const pendingApplications = 3 // placeholder for now
  const monthlyRent = properties.reduce((sum, p) => sum + p.rentPrice, 0)
  

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

        <TopBar />

        <Separator className="mb-6" />

        <div className="grid grid-cols-5 gap-6">
          <StatCard title="Total Properties" value={totalProperties} />
          <StatCard title="Active Tenants" value={activeTenants} />
          <StatCard title="Available Properties" value={availableProperties} />
          <StatCard title="Pending Applications" value={pendingApplications} />
          <StatCard title="Monthly Rent" value={`€${monthlyRent}`} />
        </div>

        {/* Main Section */}
        <DashboardMain properties={properties} />

      </main>
    </div>
  )
}
