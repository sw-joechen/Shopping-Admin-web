<template>
  <div class="btnSubmit">
    <button
      @click="ClickHandler"
      class="bg-blue-500 hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 text-white font-bold py-2 px-4 rounded"
    >{{ label }}</button>
  </div>
</template>

<script setup lang="ts">
const props = defineProps({
  label: {
    required: true,
    type: String,
  }
});

const emits = defineEmits(["submit"]);

const ClickHandler = () => {
  emits("submit")
}
</script>

<style >
</style>
