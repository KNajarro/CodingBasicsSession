<template>
  <div class="kpi-card">
    <div class="kpi-icon">
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M12 2v20M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"></path>
      </svg>
    </div>
    <div class="kpi-content">
      <h3 class="kpi-label">Inventory Value</h3>
      <p class="kpi-value">{{ formatCurrency(totalValue) }}</p>
      <p class="kpi-description">Total ListPrice across all products</p>
    </div>
  </div>
</template>

<script>
export default {
  name: 'InventoryValueCard',
  props: {
    totalValue: {
      type: Number,
      required: true
    }
  },
  methods: {
    formatCurrency(value) {
      return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
        maximumFractionDigits: 0
      }).format(value)
    }
  }
}
</script>

<style scoped>
.kpi-card {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
  display: flex;
  align-items: center;
  gap: 1.5rem;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.kpi-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 25px rgba(102, 126, 234, 0.4);
}

.kpi-icon {
  width: 60px;
  height: 60px;
  background: rgba(255, 255, 255, 0.2);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.kpi-icon svg {
  width: 32px;
  height: 32px;
}

.kpi-content {
  flex: 1;
}

.kpi-label {
  margin: 0;
  font-size: 0.9rem;
  font-weight: 600;
  opacity: 0.9;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.kpi-value {
  margin: 0.5rem 0 0 0;
  font-size: 1.8rem;
  font-weight: 700;
}

.kpi-description {
  margin: 0.5rem 0 0 0;
  font-size: 0.85rem;
  opacity: 0.85;
}

@media (max-width: 768px) {
  .kpi-card {
    flex-direction: column;
    text-align: center;
    padding: 1.5rem;
  }

  .kpi-value {
    font-size: 1.5rem;
  }
}
</style>
