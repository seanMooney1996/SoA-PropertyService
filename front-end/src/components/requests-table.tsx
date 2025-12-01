import { DataTable, Column } from "@/components/data-table";
import { Button } from "@/components/ui/button";

interface RequestsDto {
  id: string;
  propertyId: string;
  tenantId: string;
  status: string;
  city: string;
  county: string;
  requestedAt: string;
  tenantName: string;
  address: string;
}

export default function RequestsTable({
  requests,
  mode,
  onApprove,
  onDecline,
  onCancel,
}: {
  requests: RequestsDto[];
  mode: "tenant" | "landlord";
  onApprove?: (id: string) => void;
  onDecline?: (id: string) => void;
  onCancel?: (id: string) => void;
}) {
  const columns: Column<RequestsDto>[] = [
    { header: "Address", accessor: (r) => r.address },
    { header: "City", accessor: (r) => r.city },
    { header: "County", accessor: (r) => r.county },
    { header: "Status", accessor: (r) => r.status },
    {
      header: "Requested",
      accessor: (r) => new Date(r.requestedAt).toLocaleDateString(),
    },
  ];

  if (mode === "landlord") {
    columns.push({
      header: "Tenant",
      accessor: (r) => r.tenantName || "Unknown",
    });
  }

  return (
    <DataTable
      rows={requests}
      columns={columns}
      actions={(row) => (
        <>
          {mode === "landlord" && row.status == "Pending" && (
            <div className="flex gap-2">
              <Button
                size="sm"
                className="bg-green-600"
                onClick={() => onApprove && onApprove(row.id)}
              >
                Approve
              </Button>

              <Button
                size="sm"
                className="bg-red-600"
                onClick={() => onDecline && onDecline(row.id)}
              >
                Decline
              </Button>
            </div>
          )}

          {mode === "tenant" && row.status == "Pending" && (
            <Button
              size="sm"
              variant="destructive"
              onClick={() => onCancel && onCancel(row.id)}
            >
              Remove
            </Button>
          )}
        </>
      )}
    />
  );
}
