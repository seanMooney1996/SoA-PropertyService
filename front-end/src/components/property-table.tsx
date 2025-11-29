import { useEffect, useState } from "react"
import api from "@/api/client"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { Button } from "@/components/ui/button"
import { ConfirmDialog } from "./confirm-dialog"
import { toast } from "sonner"

export interface PropertyDto {
  id: string
  addressLine1: string
  city: string
  county: string
  bedrooms: number
  bathrooms: number
  rentPrice: number
  isAvailable: boolean
}

export function PropertyTable({
  properties,
  reloadProperties,
}: {
  properties: PropertyDto[]
  reloadProperties: () => void
}) 
{const [rows, setRows] = useState(properties)
useEffect(() => {
    setRows(properties)
  }, [properties])

  const handleDelete = async (id: string) => {
    try {
      await api.delete(`/Property/${id}`)
      reloadProperties()
      setRows(prev => prev.filter(p => p.id !== id))
      toast.success("Deleted!", {description: "Your property was deleted from the system."})
    } catch (err) {
      toast.error("Something went wrong when deleting the property.")
      console.error("Failed to delete property:", err)
    }
  }

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
          <TableHead>Actions</TableHead>
        </TableRow>
      </TableHeader>

      <TableBody>
        {rows.map((p) => (
          <TableRow key={p.id}>
            <TableCell>{p.addressLine1}</TableCell>
            <TableCell>{p.city}</TableCell>
            <TableCell>{p.county}</TableCell>
            <TableCell>{p.bedrooms}</TableCell>
            <TableCell>{p.bathrooms}</TableCell>
            <TableCell>€{p.rentPrice}</TableCell>

            <TableCell>
              {p.isAvailable ? (
                <span className="text-green-600 font-semibold">Yes</span>
              ) : (
                <span className="text-red-600 font-semibold">No</span>
              )}
            </TableCell>

            <TableCell>
              {p.isAvailable ? (
                <ConfirmDialog
                  title="Delete Property?"
                  description="This action is permanent and cannot be undone."
                  confirmLabel="Delete"
                  confirmColor="bg-red-600 hover:bg-red-700"
                  onConfirm={() => handleDelete(p.id)}
                  trigger={
                    <Button variant="destructive" size="sm">
                      Delete
                    </Button>
                  }
                />
              ) : (
                <span className="text-gray-400 italic">In Use</span>
              )}
          </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  )
}
