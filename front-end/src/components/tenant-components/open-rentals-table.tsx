import { DataTable, Column } from "@/components/data-table"
import { Button } from "@/components/ui/button"

export interface OpenRentalDto {
  id: string
  addressLine1: string
  city: string
  county: string
  bedrooms: number
  bathrooms: number
  rentPrice: number
}

export default function OpenRentalsTable({
  rentals,
  onRequest,
}: {
  rentals: OpenRentalDto[]
  onRequest: (propertyId: string) => void
}) {
  const columns: Column<OpenRentalDto>[] = [
    { header: "Address", accessor: r => r.addressLine1 },
    { header: "City", accessor: r => r.city },
    { header: "County", accessor: r => r.county },
    { header: "Beds", accessor: r => r.bedrooms },
    { header: "Baths", accessor: r => r.bathrooms },
    { header: "Rent", accessor: r => `€${r.rentPrice}` },
  ]

  return (
    <DataTable
      rows={rentals}
      columns={columns}
      actions={(r) => (
        <Button size="sm" className="bg-blue-600" onClick={() => onRequest(r.id)}>
          Request
        </Button>
      )}
    />
  )
}
