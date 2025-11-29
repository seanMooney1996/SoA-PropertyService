import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"

export interface PropertyDto {
  addressLine1: string
  city: string
  county: string
  bedrooms: number
  bathrooms: number
  rentPrice: number
  isAvailable: boolean
}

export function PropertyTable({ properties }: { properties: PropertyDto[] }) {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Address</TableHead>
          <TableHead>City</TableHead>
          <TableHead>County</TableHead>
          <TableHead>Bedrooms</TableHead>
          <TableHead>Bathrooms</TableHead>
          <TableHead>Rent</TableHead>
          <TableHead>Available</TableHead>
        </TableRow>
      </TableHeader>

      <TableBody>
        {properties.map((p, index) => (
          <TableRow key={index}>
            <TableCell>{p.addressLine1}</TableCell>
            <TableCell>{p.city}</TableCell>
            <TableCell>{p.county}</TableCell>
            <TableCell>{p.bedrooms}</TableCell>
            <TableCell>{p.bathrooms}</TableCell>
            <TableCell>€{p.rentPrice}</TableCell>
            <TableCell>{p.isAvailable ? "Yes" : "No"}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  )
}
