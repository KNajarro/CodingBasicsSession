import { productsService } from './productsService'
import { peopleService } from './peopleService'

const PERSON_LABELS = {
  IN: 'Individual',
  SC: 'Store Contact',
  SP: 'Sales Person',
  EM: 'Employee',
  VC: 'Vendor Contact',
  GC: 'General Contact'
}

function estimateStock(product) {
  const seed = (product.ProductID || 1) * 37
  return (seed % 120) + 5
}

function normalizeColor(color) {
  if (!color || !color.trim()) return 'Unspecified'
  return color.trim()
}

export async function getDashboardData() {
  const [products, people] = await Promise.all([
    productsService.getAll(1, 500),
    peopleService.getAll(1, 500)
  ])

  const stockRows = products.map(product => {
    const stock = estimateStock(product)
    const price = Number(product.ListPrice || 0)
    const inventoryValue = stock * price

    return {
      id: product.ProductID,
      name: product.Name,
      color: normalizeColor(product.Color),
      stock,
      price,
      inventoryValue
    }
  })

  const inventoryValue = stockRows.reduce((sum, row) => sum + row.inventoryValue, 0)

  const productsByColor = stockRows.reduce((acc, row) => {
    acc[row.color] = (acc[row.color] || 0) + 1
    return acc
  }, {})

  const peopleByType = people.reduce((acc, person) => {
    const key = person.PersonType || 'Unknown'
    const label = PERSON_LABELS[key] || key
    acc[label] = (acc[label] || 0) + 1
    return acc
  }, {})

  return {
    inventoryValue,
    stockRows,
    productsByColor,
    peopleByType
  }
}
