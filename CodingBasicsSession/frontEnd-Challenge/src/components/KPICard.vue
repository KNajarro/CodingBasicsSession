<template>
  <div class="card kpi-card h-100">
    <div class="card-body">
      <div class="d-flex justify-content-between align-items-start">
        <div>
          <p class="card-title mb-2 text-muted">{{ title }}</p>
          <h2 class="kpi-value mb-0">{{ formattedValue }}</h2>
        </div>
        <div class="kpi-icon" :style="{ backgroundColor: iconBgColor }">
          <i :class="`bi bi-${icon}`"></i>
        </div>
      </div>
      <small class="text-muted mt-3 d-block">{{ subtitle }}</small>
    </div>
  </div>
</template>

<script>
export default {
  name: 'KPICard',
  props: {
    title: {
      type: String,
      required: true
    },
    value: {
      type: [Number, String],
      required: true
    },
    format: {
      type: String,
      default: 'number', // 'currency', 'number', 'percent'
      validator: (v) => ['currency', 'number', 'percent'].includes(v)
    },
    icon: {
      type: String,
      default: 'bar-chart'
    },
    iconBgColor: {
      type: String,
      default: '#e3f2fd'
    },
    subtitle: {
      type: String,
      default: ''
    }
  },
  computed: {
    formattedValue() {
      const numValue = typeof this.value === 'string' ? parseFloat(this.value) : this.value
      
      if (this.format === 'currency') {
        return new Intl.NumberFormat('en-US', {
          style: 'currency',
          currency: 'USD'
        }).format(numValue)
      } else if (this.format === 'percent') {
        return numValue.toFixed(1) + '%'
      } else {
        return new Intl.NumberFormat('en-US').format(Math.round(numValue))
      }
    }
  }
}
</script>

<style scoped>
.kpi-card {
  border: none;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s, box-shadow 0.2s;
}

.kpi-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.kpi-value {
  font-size: 1.75rem;
  font-weight: 600;
  color: #1a1a1a;
}

.card-title {
  font-size: 0.875rem;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.kpi-icon {
  width: 50px;
  height: 50px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
}
</style>
