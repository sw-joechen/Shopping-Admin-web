<template>
  <tr class="tableHeader">
    <th class="th" v-for="(el, index) in header" :key="index" :header="el">
      {{ $t(`common.tableHeader.${el.key}`) }}
    </th>
  </tr>
</template>

<script>
export default {
  name: 'tableHeader',
  props: {
    header: {
      required: true,
      type: Array,
    },
  },
};
</script>

<style lang="scss">
.tableHeader {
  @apply text-left border-t border-b-2 border-grey2 p-2 bg-blue-400 text-white text-xl;
  .th {
    @apply p-3;
  }
}
</style>
