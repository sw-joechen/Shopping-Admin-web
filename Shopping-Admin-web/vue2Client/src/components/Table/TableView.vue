<template>
  <div class="tableView w-full">
    <table class="w-full">
      <TableHeader :header="tableHeaders" v-if="tableHeaders" />
      <TableRow
        v-for="(row, index) in tableRows"
        :key="index"
        :row="row"
        @edit="editHandler"
        @unlock="unlockHandler($event, index)"
        :isEditBtnRequired="isEditBtnRequired"
        :isUnlockBtnRequired="isUnlockBtnRequired"
        :diabledList="btnDisabledProcesser(index)"
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
    isEditBtnRequired: {
      required: false,
      default: true,
    },
    isUnlockBtnRequired: {
      required: false,
      default: true,
    },
    btnDisabledList: {
      type: Array,
    },
  },
  methods: {
    unlockHandler(payload, idx) {
      const obj = {};
      payload.forEach((el) => {
        obj[el.key] = el.value;
      });

      this.$emit("unlock", {
        idx,
        ...obj,
      });
    },
    editHandler(payload) {
      this.$emit("edit", payload);
    },
    btnDisabledProcesser(rowIdx) {
      let result = [];
      if (this.btnDisabledList) {
        this.btnDisabledList.forEach((btn) => {
          if (btn.idx === rowIdx) {
            result = btn.btnType;
            return true;
          }
        });
      }
      return result;
    },
  },
  computed: {
    // TODO: 要改成自定義欄位順序, 吃headersPlaceHolder
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
