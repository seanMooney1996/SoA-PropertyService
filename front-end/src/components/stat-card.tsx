import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"

export function StatCard({
  title,
  value,
}: {
  title: string
  value: string | number
}) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>{title}</CardTitle>
      </CardHeader>
      <CardContent>
        <p className="text-3xl font-bold">{value}</p>
      </CardContent>
    </Card>
  )
}
