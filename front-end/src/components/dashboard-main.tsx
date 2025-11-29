import { useState } from "react"
import { Button } from "@/components/ui/button"
import { CreatePropertyForm } from "./create-property-form"
import { PropertyTable, PropertyDto } from "./property-table"

export function DashboardMain({ properties }: { properties: PropertyDto[] }) {
  const [activePanel, setActivePanel] = useState<"create" | "properties">("properties")

  return (
    <div className="mt-8">
      <h2 className="text-xl font-semibold mb-4">Quick Actions</h2>

      <div className="flex gap-4 mb-6">
        <Button
          variant={activePanel === "create" ? "default" : "outline"}
          onClick={() => setActivePanel("create")}
        >
          Add Property
        </Button>

        <Button
          variant={activePanel === "properties" ? "default" : "outline"}
          onClick={() => setActivePanel("properties")}
        >
          My Properties
        </Button>
      </div>

      {activePanel === "create" && <CreatePropertyForm />}

      {activePanel === "properties" && (
        <PropertyTable properties={properties} />
      )}
    </div>
  )
}
