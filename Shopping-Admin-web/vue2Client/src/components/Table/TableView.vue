<template>
  <div class="tableView w-full">
    <table class="w-full">
      <TableHeader :header="tableHeaders" v-if="tableHeaders" />
      <TableRow
        v-for="(row, index) in tableRows"
        :key="index"
        :row="row"
        @edit="editHandler"
      />
    </table>
  </div>
</template>

<script>
import TableHeader from "./children/TableHeader.vue";
import TableRow from "./children/TableRow.vue";

export default {
  name: "tableView",
  components: {
    TableHeader,
    TableRow,
  },
  props: {
    datas: {
      required: true,
      type: Array,
      // default: () => [],
    },
    headersPlaceholder: {
      required: true,
      type: Array,
    },
    isOperational: {
      required: false,
      type: Boolean,
      default: true,
    },
  },
  methods: {
    editHandler(payload) {
      this.$emit("edit", payload);
    },
  },
  computed: {
    tableHeaders() {
      if (this.datas.length) {
        let lastIndex = 0;
        const result = Object.keys(this.datas[0]).map((header, index) => {
          lastIndex = index;
          return {
            index: index,
            key: header,
          };
        });

        // 新增操作(編輯)標頭
        if (this.isOperational) {
          result.push({
            index: lastIndex++,
            key: "operation",
          });
        }
        return result;
      } else {
        return this.headersPlaceholder.map((el, index) => {
          return {
            index: index,
            key: el,
          };
        });
      }
    },
    tableRows() {
      return this.datas.map((obj) => {
        const result = Object.keys(obj).map((key, idx2) => {
          return {
            key: key,
            value: Object.values(obj)[idx2],
          };
        });
        if (this.isOperational) {
          result.push({
            key: "operation",
            value: "",
          });
        }
        return result;
      });
    },
  },
};
</script>

<style></style>
