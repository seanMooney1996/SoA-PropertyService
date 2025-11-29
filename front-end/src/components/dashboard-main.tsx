import { useState } from "react";
import { Button } from "@/components/ui/button";
import { CreatePropertyForm } from "./create-property-form";

export function DashboardMain() {
  const [activePanel, setActivePanel] = useState<string | null>(null);

  return (
    <div className="mt-8">
      <h2 className="text-xl font-semibold mb-3">Quick Actions</h2>

      <div className="flex gap-4 mb-6">
        <Button onClick={() => setActivePanel("createProperty")}>
          Add Property
        </Button>
      </div>

        {activePanel === "createProperty" && (
            <div className="w-full">
            <CreatePropertyForm />
            </div>
        )}
    </div>
  );
}

