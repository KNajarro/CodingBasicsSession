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
  background: white;
  border: 1px solid #e0e0e0;
  color: #2c3e50;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  display: flex;
  align-items: center;
  gap: 1.5rem;
  transition: all 0.3s ease;
  border-left: 4px solid #3498db;
}

.kpi-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.12);
  border-left-color: #2980b9;
}

.kpi-icon {
  width: 60px;
  height: 60px;
  background: #ecf0f1;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  color: #3498db;
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
  font-size: 0.85rem;
  font-weight: 600;
  color: #7f8c8d;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.kpi-value {
  margin: 0.5rem 0 0 0;
  font-size: 2rem;
  font-weight: 700;
  color: #2c3e50;
}

.kpi-description {
  margin: 0.5rem 0 0 0;
  font-size: 0.9rem;
  color: #95a5a6;
}

@media (max-width: 768px) {
  .kpi-card {
    flex-direction: column;
    text-align: center;
    padding: 1.5rem;
  }

  .kpi-icon {
    width: 50px;
    height: 50px;
  }

  .kpi-value {
    font-size: 1.5rem;
  }
}
</style>
