<template>
  <canvas ref="chartCanvas"></canvas>
</template>

<script setup>
import { ref, watch, onMounted } from "vue";
import { Chart, BarController, ArcElement, Tooltip, Legend, Title } from "chart.js";

const props = defineProps({
  chartData: {
    type: Object,
    required: true
  }
});

Chart.register(BarController, ArcElement, Tooltip, Legend, Title)


const chartCanvas = ref(null);
let chartInstance = null;

const renderChart = () => {
  if (chartInstance) {
    chartInstance.destroy();
  }

  chartInstance = new Chart(chartCanvas.value, {
    type: "bar",
    data: props.chartData,
    options: {
      responsive: true,
      plugins: {
        legend: {
          position: "top"
        },
        title: {
          display: true,
          text: "Customer Analysis"
        }
      }
    }
  });
};

onMounted(() => {
  renderChart();
});

watch(
  () => props.chartData,
  () => {
    renderChart();
  },
  { deep: true }
);
</script>