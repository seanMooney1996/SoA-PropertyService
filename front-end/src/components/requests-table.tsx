import { DataTable, Column } from "@/components/data-table";
import { Button } from "@/components/ui/button";
import { RentalRequestDto } from "@/types/rental-request";

export default function RequestsTable({
  requests,
  mode,
  onApprove,
  onDecline,
  onCancel,
}: {
  requests: RentalRequestDto[];
  mode: "tenant" | "landlord";
  onApprove?: (id: string) => void;
  onDecline?: (id: string) => void;
  onCancel?: (id: string) => void;
}) {
  const columns: Column<RentalRequestDto>[] = [
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
          {mode === "landlord" && (
            <div className="flex gap-2">
              <Button
                size="sm"
                className="bg-green-600"
                onClick={() => onApprove && onApprove(row.requestId)}
              >
                Approve
              </Button>

              <Button
                size="sm"
                className="bg-red-600"
                onClick={() => onDecline && onDecline(row.requestId)}
              >
                Decline
              </Button>
            </div>
          )}

          {mode === "tenant" && (
            <Button
              size="sm"
              variant="destructive"
              onClick={() => onCancel && onCancel(row.requestId)}
            >
              Remove
            </Button>
          )}
        </>
      )}
    />
  );
}
