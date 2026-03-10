<template>
  <article class="kpi-card">
    <p class="label">{{ label }}</p>
    <p class="value">{{ formattedValue }}</p>
    <p v-if="hint" class="hint">{{ hint }}</p>
  </article>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  label: { type: String, required: true },
  value: { type: Number, required: true },
  hint: { type: String, default: '' },
  currency: { type: String, default: 'USD' }
})

const formattedValue = computed(() =>
  new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: props.currency,
    maximumFractionDigits: 0
  }).format(props.value)
)
</script>

<style scoped>
.kpi-card {
  background: var(--panel);
  border: 1px solid var(--line);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
  padding: 1rem 1.1rem;
}

.label {
  margin: 0;
  color: var(--muted);
  font-size: 0.9rem;
}

.value {
  margin: 0.3rem 0 0;
  font-size: clamp(1.6rem, 4vw, 2.4rem);
  font-weight: 700;
  color: var(--brand);
}

.hint {
  margin: 0.5rem 0 0;
  font-size: 0.85rem;
  color: var(--muted);
}
</style>
