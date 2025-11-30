import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"

export interface Column<T> {
  header: string
  accessor: (row: T) => React.ReactNode
}

interface DataTableProps<T> {
  rows: T[]
  columns: Column<T>[]
  actions?: (row: T) => React.ReactNode
}

export function DataTable<T>({ rows, columns, actions }: DataTableProps<T>) {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          {columns.map((col) => (
            <TableHead key={col.header}>{col.header}</TableHead>
          ))}
          {actions && <TableHead>Actions</TableHead>}
        </TableRow>
      </TableHeader>

      <TableBody>
        {rows.map((row, index) => (
          <TableRow key={index}>
            {columns.map((col) => (
              <TableCell key={col.header}>{col.accessor(row)}</TableCell>
            ))}

            {actions && (
              <TableCell>{actions(row)}</TableCell>
            )}
          </TableRow>
        ))}
      </TableBody>
    </Table>
  )
}
