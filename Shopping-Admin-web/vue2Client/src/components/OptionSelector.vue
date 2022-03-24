<template>
  <div class="optionSelector">
    <div class="relative w-64">
      <select
        @change="ChangeHandler"
        v-model="value"
        class="block appearance-none w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 pr-8 rounded shadow leading-snug focus:outline-none focus:shadow-outline"
      >
        <option
          :key="opt.value"
          :selected="opt.value === 0"
          v-for="opt in options"
          :value="opt.value"
        >
          {{ opt.label }}
        </option>
      </select>
      <div
        class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700"
      >
        <svg
          class="fill-current h-4 w-4"
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 20 20"
        >
          <path
            d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z"
          />
        </svg>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    options: {
      required: true,
      type: Array,
    },
  },
  data: () => {
    return {
      value: 0,
    };
  },
  methods: {
    ChangeHandler() {
      this.$emit('onChange', {
        value: this.value,
      });
    },
  },
};
</script>

<style></style>
