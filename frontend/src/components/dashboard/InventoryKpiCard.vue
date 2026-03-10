<template>
  <div class="kpi-card">
    <div class="kpi-icon">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
        <path d="M12 2L2 7l10 5 10-5-10-5z"/>
        <path d="M2 17l10 5 10-5M2 12l10 5 10-5"/>
      </svg>
    </div>
    <div class="kpi-body">
      <span class="kpi-label">Inventory Value</span>
      <span class="kpi-value">{{ formattedValue }}</span>
      <span class="kpi-sub">Total list price · all products</span>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  total: {
    type: Number,
    default: 0
  }
})

const formattedValue = computed(() =>
  new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0 }).format(props.total)
)
</script>

<style scoped>
.kpi-card {
  display: flex;
  align-items: center;
  gap: 1.25rem;
  background: linear-gradient(135deg, #0f2027 0%, #203a43 50%, #2c5364 100%);
  border: 1px solid rgba(255,255,255,0.08);
  border-radius: 16px;
  padding: 1.5rem 2rem;
  box-shadow: 0 8px 32px rgba(0,0,0,0.3);
}

.kpi-icon {
  flex-shrink: 0;
  width: 52px;
  height: 52px;
  background: rgba(99, 210, 255, 0.15);
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #63d2ff;
}

.kpi-icon svg {
  width: 26px;
  height: 26px;
}

.kpi-body {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
}

.kpi-label {
  font-size: 0.75rem;
  font-weight: 600;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: rgba(255,255,255,0.5);
}

.kpi-value {
  font-size: 2rem;
  font-weight: 800;
  color: #ffffff;
  line-height: 1;
  letter-spacing: -0.02em;
}

.kpi-sub {
  font-size: 0.72rem;
  color: rgba(255,255,255,0.35);
}
</style>
