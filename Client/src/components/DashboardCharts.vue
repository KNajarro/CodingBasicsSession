<template>
  <section class="charts-grid">
    <article class="chart-card">
      <header>
        <h2>Product Distribution by Color</h2>
      </header>
      <div ref="colorChartRef" class="chart-host"></div>
    </article>

    <article class="chart-card">
      <header>
        <h2>Customer Analysis by Person Type</h2>
      </header>
      <div ref="peopleChartRef" class="chart-host"></div>
    </article>
  </section>
</template>

<script setup>
import { BarChart, PieChart } from 'echarts/charts'
import {
  GridComponent,
  LegendComponent,
  TitleComponent,
  TooltipComponent
} from 'echarts/components'
import { CanvasRenderer } from 'echarts/renderers'
import { init, use } from 'echarts/core'
import { nextTick, onBeforeUnmount, onMounted, ref, watch } from 'vue'

use([
  BarChart,
  PieChart,
  GridComponent,
  LegendComponent,
  TitleComponent,
  TooltipComponent,
  CanvasRenderer
])

const props = defineProps({
  productsByColor: { type: Object, required: true },
  peopleByType: { type: Object, required: true }
})

const colorChartRef = ref(null)
const peopleChartRef = ref(null)
let colorChart
let peopleChart

function buildSeries(dataMap) {
  return Object.entries(dataMap).map(([name, value]) => ({ name, value }))
}

function renderColorChart() {
  if (!colorChart) return

  colorChart.setOption({
    tooltip: { trigger: 'item' },
    series: [
      {
        type: 'pie',
        radius: ['35%', '72%'],
        avoidLabelOverlap: true,
        data: buildSeries(props.productsByColor)
      }
    ]
  })
}

function renderPeopleChart() {
  if (!peopleChart) return

  const labels = Object.keys(props.peopleByType)
  const values = Object.values(props.peopleByType)

  peopleChart.setOption({
    tooltip: { trigger: 'axis' },
    xAxis: { type: 'category', data: labels },
    yAxis: { type: 'value' },
    series: [
      {
        data: values,
        type: 'bar',
        itemStyle: { color: '#004f59', borderRadius: [6, 6, 0, 0] }
      }
    ]
  })
}

function resizeCharts() {
  colorChart?.resize()
  peopleChart?.resize()
}

onMounted(async () => {
  await nextTick()
  colorChart = init(colorChartRef.value)
  peopleChart = init(peopleChartRef.value)
  renderColorChart()
  renderPeopleChart()
  window.addEventListener('resize', resizeCharts)
})

watch(() => props.productsByColor, renderColorChart, { deep: true })
watch(() => props.peopleByType, renderPeopleChart, { deep: true })

onBeforeUnmount(() => {
  window.removeEventListener('resize', resizeCharts)
  colorChart?.dispose()
  peopleChart?.dispose()
})
</script>

<style scoped>
.charts-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(280px, 1fr));
  gap: 1rem;
}

.chart-card {
  background: var(--panel);
  border: 1px solid var(--line);
  border-radius: var(--radius);
  padding: 1rem;
  box-shadow: var(--shadow);
}

.chart-card h2 {
  margin: 0;
  font-size: 1.05rem;
}

.chart-host {
  margin-top: 0.8rem;
  height: 320px;
}

@media (max-width: 900px) {
  .charts-grid {
    grid-template-columns: 1fr;
  }
}
</style>
