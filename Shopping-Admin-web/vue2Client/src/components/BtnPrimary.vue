<template>
  <div class="btnPrimary">
    <button
      :disabled="disabled"
      @click="ClickHandler"
      :class="{ 'cursor-not-allowed': disabled }"
      class="bg-blue-500 hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 text-white font-bold py-2 px-4 rounded"
    >
      {{ label }}
    </button>
  </div>
</template>

<script>
export default {
  props: {
    label: {
      required: true,
      type: String,
    },
    disabled: {
      required: false,
      type: Boolean,
    },
  },
  methods: {
    ClickHandler() {
      this.$emit('submit');
    },
  },
};
</script>

<style></style>
