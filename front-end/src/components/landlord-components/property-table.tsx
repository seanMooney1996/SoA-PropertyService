import { useState } from "react"
import api from "@/api/client"
import { Button } from "@/components/ui/button"
import { ConfirmDialog } from "../confirm-dialog"
import { DataTable, Column } from "@/components/data-table"

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
  reloadProperties
}: {
  properties: PropertyDto[]
  reloadProperties: () => void
}) {

  const handleDelete = async (id: string) => {
    try {
      await api.delete(`/Property/${id}`)
      reloadProperties()
    } catch (err) {
      console.error("Failed to delete property:", err)
      alert("Failed to delete property.")
    }
  }

  const columns: Column<PropertyDto>[] = [
    { header: "Address", accessor: p => p.addressLine1 },
    { header: "City", accessor: p => p.city },
    { header: "County", accessor: p => p.county },
    { header: "Bedrooms", accessor: p => p.bedrooms },
    { header: "Bathrooms", accessor: p => p.bathrooms },
    { header: "Rent", accessor: p => `€${p.rentPrice}` },
    {
      header: "Available",
      accessor: p =>
        p.isAvailable
          ? <span className="text-green-600 font-semibold">Yes</span>
          : <span className="text-red-600 font-semibold">No</span>
    }
  ]

  return (
    <DataTable
      rows={properties}
      columns={columns}
      actions={(p) =>
        p.isAvailable ? (
          <ConfirmDialog
            title="Delete Property?"
            description="This action is permanent and cannot be undone."
            confirmLabel="Delete"
            confirmColor="bg-red-600 hover:bg-red-700"
            onConfirm={() => handleDelete(p.id)}
            trigger={<Button variant="destructive" size="sm">Delete</Button>}
          />
        ) : (
          <span className="text-gray-400 italic">In Use</span>
        )
      }
    />
  )
}
