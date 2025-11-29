import { Field, FieldGroup, FieldLabel } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import api from "@/api/client";
import { useState } from "react";
import { toast } from "sonner"


export function CreatePropertyForm({ reloadProperties }: { reloadProperties: () => void }) {
  const [addressLine1, setAddressLine1] = useState("");
  const [city, setCity] = useState("");
  const [county, setCounty] = useState("");
  const [bedrooms, setBedrooms] = useState(1);
  const [bathrooms, setBathrooms] = useState(1);
  const [rentPrice, setRentPrice] = useState(1000);

  const handleSubmit = async () => {
    try {
      await api.post("/Property", {
        addressLine1,
        city,
        county,
        bedrooms,
        bathrooms,
        rentPrice,
      });
      toast.success("Saved!", {description: "Your property was added to the system."})
      reloadProperties()
    } catch {
      toast.error("Something went wrong when creating property.")
    }
  };

  return (
  <Card className="w-full border shadow-md">
  <CardContent className="p-10 max-w-4xl mx-auto">
    <h2 className="text-2xl font-semibold mb-8 text-center">
      Add Property
    </h2>

    <form
      onSubmit={(e) => {
        e.preventDefault();
        handleSubmit();
      }}
      className="space-y-6"
    >
      <div className="grid grid-cols-1 gap-4">
        <Field>
          <FieldLabel>Address</FieldLabel>
          <Input
            value={addressLine1}
            onChange={(e) => setAddressLine1(e.target.value)}
            required
          />
        </Field>
      </div>

      <div className="grid grid-cols-2 gap-6">
        <Field>
          <FieldLabel>City</FieldLabel>
          <Input
            value={city}
            onChange={(e) => setCity(e.target.value)}
            required
          />
        </Field>

        <Field>
          <FieldLabel>County</FieldLabel>
          <Input
            value={county}
            onChange={(e) => setCounty(e.target.value)}
            required
          />
        </Field>
      </div>

      <div className="grid grid-cols-3 gap-6">
        <Field>
          <FieldLabel>Bedrooms</FieldLabel>
          <Input
            type="number"
            value={bedrooms}
            min={1}
            onChange={(e) => setBedrooms(Number(e.target.value))}
            required
          />
        </Field>

        <Field>
          <FieldLabel>Bathrooms</FieldLabel>
          <Input
            type="number"
            value={bathrooms}
            min={1}
            onChange={(e) => setBathrooms(Number(e.target.value))}
            required
          />
        </Field>

        <Field>
          <FieldLabel>Rent (€)</FieldLabel>
          <Input
            type="number"
            value={rentPrice}
            onChange={(e) => setRentPrice(Number(e.target.value))}
            required
          />
        </Field>
      </div>

      <Button type="submit" className="w-full">
        Create Property
      </Button>
    </form>
  </CardContent>
</Card>
  );
}
