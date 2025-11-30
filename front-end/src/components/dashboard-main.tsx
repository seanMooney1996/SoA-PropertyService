import { useState } from "react"
import { Button } from "@/components/ui/button"
import { CreatePropertyForm } from "./landlord-components/create-property-form"
import { PropertyTable, PropertyDto } from "./landlord-components/property-table"

export function DashboardMain({ properties, reloadProperties}: { properties: PropertyDto[]; reloadProperties: () => void }) {
  const [activePanel, setActivePanel] = useState<"create" | "properties">("properties")

      const reloadPropertiesAndSetPanel = async () => {
        reloadProperties()
        setActivePanel("properties")
    }
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

    {activePanel === "create" && (
      <CreatePropertyForm reloadProperties={reloadPropertiesAndSetPanel} />
    )}

    {activePanel === "properties" && (
      <PropertyTable properties={properties} reloadProperties={reloadPropertiesAndSetPanel} />
    )}
    </div>
  )
}
