<script setup>
import { ref, onMounted, nextTick } from "vue"
import Chart from "chart.js/auto"
import dashboardService from "../services/dashboardService"

const inventoryValue = ref(0)
const productsByColor = ref([])
const peopleByType = ref([])
const lowStockProducts = ref([])
const colorChart = ref(null)
const peopleChart = ref(null)

const loadDashboard = async () => {
  try {
    inventoryValue.value = await dashboardService.getInventoryValue()
    productsByColor.value = await dashboardService.getProductsByColor()
    peopleByType.value = await dashboardService.getPeopleByType()
    lowStockProducts.value = await dashboardService.getLowStockProducts()

    const labels = productsByColor.value.map(c => c.color)
    const counts = productsByColor.value.map(c => c.count)

    const palette = [
    "#4e79a7",
    "#f28e2b",
    "#e15759",
    "#76b7b2",
    "#59a14f",
    "#edc948",
    "#b07aa1",
    "#ff9da7",
    "#9c755f",
    "#bab0ab"
    ]

    new Chart(colorChart.value, {
    type: 'pie',
    data: {
        labels: labels,
        datasets: [{
        data: counts,
        backgroundColor: palette.slice(0, labels.length),
        borderColor: "#ffffff",
        borderWidth: 2
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,

        plugins: {
        legend: {
            position: "right"
        },

        tooltip: {
            callbacks: {
            label: function(context) {
                const label = context.label
                const value = context.raw
                return `${label}: ${value} products`
            }
            }
        }
        }
    }
    })

    await nextTick()

    $('#lowStockTable').DataTable({
        pageLength: 10,
        lengthChange: false,
        destroy: true
    })
  } catch (error) {
    console.error("Error loading dashboard:", error)
  }

    const types = peopleByType.value.map(p => p.personType)
    const typeCounts = peopleByType.value.map(p => p.count)

    new Chart(peopleChart.value, {
        type: 'bar',
        data: {
            labels: types,
            datasets: [{
            label: 'Customers',
            data: typeCounts,
            backgroundColor: "#4e73df",
            borderRadius: 6,
            barThickness: 50
            }]
        },
        options:{
            responsive:true,
            maintainAspectRatio:false,

            interaction: {
            mode: 'index',
            intersect: false
            },

            plugins:{
            legend:{
                display:false
            },
            tooltip:{
                callbacks:{
                label: function(context){
                    return `Customers: ${context.raw.toLocaleString()}`
                }
                }
            }
            },

            scales:{
            y:{
                beginAtZero:true,
                ticks:{
                callback:(value)=> value.toLocaleString()
                }
            }
            }
        }
    })
}

onMounted(() => {
  loadDashboard()
})
</script>

<template>
  <div class="dashboard">
    <h1>Dashboard</h1>

    <!-- KPI -->
    <div class="kpi-container">

    <div class="kpi-card inventory">
        
        <div class="kpi-header">
        <span class="kpi-title">Inventory Value</span>
        <span class="kpi-icon">📦</span>
        </div>

        <div class="kpi-value">
        ${{ inventoryValue.toLocaleString() }}
        </div>

        <div class="kpi-footer">
        Total value of all products in inventory
        </div>

    </div>

    </div>
    <div class="charts-container">

        <div class="chart-card">
            <h2>Distribution by Color</h2>
            <canvas ref="colorChart"></canvas>
        </div>

        <div class="chart-card">
            <h2>Customer Analysis</h2>
            <canvas ref="peopleChart"></canvas>
        </div>

    </div>

    <!-- Low Stock Table -->
    <div class="section">
      <h2>Low Stock Products</h2>

      <table id="lowStockTable">
        <thead>
          <tr>
            <th>Name</th>
            <th>Product Number</th>
            <th>Safety Stock Level</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="p in lowStockProducts" :key="p.productNumber">
            <td>{{ p.name }}</td>
            <td>{{ p.productNumber }}</td>
            <td class="low-stock">{{ p.safetyStockLevel }}</td>
          </tr>
        </tbody>

      </table>
    </div>

  </div>
</template>

<style scoped>

.dashboard {
  padding: 30px;
  background: #f5f7fb;
  min-height: 100vh;
}

.kpi-container{
  display:flex;
  gap:20px;
  margin-bottom:40px;
}

.kpi-card{
  width:320px;
  padding:25px;
  border-radius:14px;
  color:white;
  box-shadow:0 10px 25px rgba(0,0,0,0.1);
  display:flex;
  flex-direction:column;
  justify-content:space-between;
  transition: transform .2s;
}

.kpi-card:hover{
  transform:translateY(-4px);
}

.inventory{
  background:linear-gradient(135deg,#4e73df,#224abe);
}

.kpi-header{
  display:flex;
  justify-content:space-between;
  align-items:center;
}

.kpi-title{
  font-size:14px;
  opacity:0.9;
}

.kpi-icon{
  font-size:22px;
}

.kpi-value{
  font-size:38px;
  font-weight:bold;
  margin:15px 0;
}

.kpi-footer{
  font-size:12px;
  opacity:0.8;
}

.section {
  margin-top: 30px;
}

table {
  border-collapse: collapse;
  width: 100%;
  background:white;
  border-radius:8px;
  overflow:hidden;
}

th, td {
  border-bottom: 1px solid #eee;
  padding: 10px;
}

th{
  background:#f5f5f5;
  text-align:left;
}

.low-stock {
  color: red;
  font-weight: bold;
}

.charts-container{
  display:grid;
  grid-template-columns:1fr 1.4fr;
  gap:40px;
  margin-top:30px;
}

.chart-card{
  background:white;
  padding:20px;
  border-radius:12px;
  box-shadow:0 6px 16px rgba(0,0,0,0.08);
}

.chart-card h2{
  margin-bottom:10px;
}

canvas{
  width: 100%;
  max-height:350px;
  margin:auto;
}

</style>