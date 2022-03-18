<template>
  <div class="productsList p-5">
    <!-- input query -->
    <div class="query flex">
      <div class="inputGroup flex">
        <label class="whitespace-nowrap pr-3 leading-[42px]">
          {{ $t("common.tableHeader.name") }}
        </label>
        <input type="text" class="input" v-model="queryData.name" required />
      </div>
      <div class="inputGroup flex">
        <label class="whitespace-nowrap pr-3 leading-[42px]">
          {{ $t("common.tableHeader.type") }}
        </label>
        <input type="text" class="input" v-model="queryData.type" required />
      </div>
      <OptionSelector
        class="px-2"
        :options="options"
        @onChange="onEnabledOptionChangeHandler"
      />

      <BtnPrimary
        :label="$t('common.search')"
        class="pr-2"
        @submit="searchHandler"
      />
    </div>
    <hr class="my-2" />
    <div class="add flex">
      <BtnPrimary
        :label="$t('common.add')"
        class="pr-2"
        @submit="toggleAddDialogHandler"
      />
    </div>

    <!-- table -->
    <div class="tableContainer pt-5">
      <table class="w-full">
        <thead>
          <tr
            class="text-left border-t border-b-2 border-grey2 p-2 bg-blue-400 text-white text-xl;"
          >
            <th class="p-3" scope="col" v-for="col in columns" :key="col">
              {{ $t(`common.tableHeader.${col}`) }}
            </th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(item, index) in productList"
            :key="index"
            class="even:bg-gray-300 hover:bg-gray-400 even:border-b odd:bg-white border-gray-400 p-2"
          >
            <td
              v-for="(column, indexColumn) in columns"
              :key="indexColumn"
              class="p-3 text-left"
            >
              <div class="btnContainer" v-if="column === 'operation'">
                <BtnPrimary :label="$t('common.edit')" />
              </div>
              <span v-else>
                {{ extraFormatter(item, column) }}
              </span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <FormDialog
      v-if="isShowAddDialog"
      :isShowDialog="isShowAddDialog"
      @toggle="toggleAddDialogHandler"
      @submit="onSubmitAddDialogHandler"
      :title="$t('productList.addFormTitle')"
    >
      <div class="wrapper">
        <!-- 商品名稱 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t("common.tableHeader.name") }}
          </label>
          <input
            v-model="addData.name"
            type="text"
            class="input"
            :class="{ '!border-red-600': isNameWarning }"
            :placeholder="$t('common.tableHeader.name')"
            @focus="isNameWarning = false"
          />
        </div>

        <!-- 描述 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t("common.tableHeader.description") }}
          </label>
          <input
            v-model="addData.desc"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.description')"
          />
        </div>

        <!-- 價格 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t("common.tableHeader.price") }}
          </label>
          <input
            :class="{ '!border-red-600': isPriceWarning }"
            v-model="addData.price"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.price')"
          />
        </div>

        <!-- 數量 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t("common.tableHeader.amount") }}
          </label>
          <input
            v-model="addData.amount"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.amount')"
          />
        </div>

        <!-- 類型 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t("common.tableHeader.type") }}
          </label>
          <input
            v-model="addData.type"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.type')"
          />
        </div>

        <!-- 照片 -->
        <div class="imgUploader">
          <div class="inputGroup flex">
            <label class="label whitespace-nowrap mr-3 leading-[42px]">
              {{ $t("common.tableHeader.picture") }}
            </label>
            <input
              accept="image/*"
              type="file"
              class="input"
              :placeholder="$t('common.tableHeader.picture')"
              @change="uploadImage($event)"
            />
          </div>
          <div class="preview flex justify-center items-center">
            <!-- <template v-if="addData.preview"> -->
            <img class="img" :src="addData.preview" />
            <!-- </template> -->
          </div>
        </div>
      </div>
    </FormDialog>
  </div>
</template>

<script>
import BtnPrimary from "../components/BtnPrimary.vue";
import OptionSelector from "../components/OptionSelector.vue";
import FormDialog from "../components/Dialogs/DialogView.vue";
// import TableView from "../components/Table/TableView.vue";

import { GetProductsList } from "../APIs/Product";
import { DateTime } from "luxon";
// import ErrorCodeList from "../ErrorCodeList";
const EOptions = {
  all: 0,
  enabled: 1,
  disabled: 2,
};

export default {
  name: "productsList",
  components: {
    BtnPrimary,
    OptionSelector,
    FormDialog,
    // TableView,
  },
  data: () => {
    return {
      options: [
        {
          label: "全部",
          value: EOptions.all,
        },
        {
          label: "啟用",
          value: EOptions.enabled,
        },
        {
          label: "禁用",
          value: EOptions.disabled,
        },
      ],
      columns: [
        "id",
        "name",
        "description",
        "price",
        "picture",
        "amount",
        "type",
        "enabled",
        "createdDate",
        "updatedDate",
        "operation",
      ],
      btnDisabledList: [],
      queryData: {
        enabled: EOptions.all,
        name: "",
        type: "",
      },
      productList: [],
      isShowAddDialog: false,
      addData: {
        name: "",
        desc: "",
        price: null,
        amount: null,
        type: "",
        pic: null,
        image: "",
      },
      preview: null,
      isNameWarning: false,
      isPriceWarning: false,
      isAmountWarning: false,
    };
  },
  methods: {
    uploadImage(event) {
      const input = event.target;
      console.log("input: ", input);

      if (input.files && input.files[0]) {
        // create a new FileReader to read this image and convert to base64 format
        var reader = new FileReader();
        // Define a callback function to run, when FileReader finishes its job
        reader.onload = (e) => {
          this.preview = e.target.result;
        };
        // Start the reader job - read file as a data url (base64 format)
        reader.readAsDataURL(input.files[0]);
        console.log("preview: ", this.preview);
      }

      // ====
      // const input = event.target;
      // if (input.files) {
      //   this.preview = URL.createObjectURL(input.files[0]);
      //   this.addData.image = input.files[0];
      //   console.log("image: ", this.addData.image);
      //   console.log("preview: ", this.preview);
      // }
    },
    onSubmitAddDialogHandler() {
      // 檢查名稱不可為空
      if (!this.addData.name.length) {
        this.isNameWarning = true;
        return this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
      }
      // 檢查price
      const isPriceNum = /^\d+$/.test(this.addData.price);
      if (!isPriceNum) {
        this.isPriceWarning = true;
        return this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
      }

      // 檢查amount
      const isAmountNum = /^\d+$/.test(this.addData.amount);
      if (!isAmountNum) {
        this.isAmountWarning = true;
        return this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
      }
    },
    toggleAddDialogHandler() {
      this.isShowAddDialog = !this.isShowAddDialog;
      this.clearAddData();
    },
    clearAddData() {
      this.addData = {
        name: "",
        desc: "",
        price: null,
        amount: null,
        type: "",
      };
    },
    extraFormatter(data, column) {
      // 將狀態做i18n
      if (column === "enabled") {
        return data[column]
          ? this.$t("common.tableHeader.enabled")
          : this.$t("common.tableHeader.disabled");
      } else if (column === "createdDate" || column === "updatedDate") {
        // 將日期做格式化
        const date = new Date(data[column]);
        const dt = DateTime.fromJSDate(date);
        return dt.toFormat("yyyy/MM/dd HH:mm:ss");
      } else {
        return data[column];
      }
    },
    async searchHandler() {
      const res = await GetProductsList();
      this.productList = res.data;
      console.log("productList=> ", this.productList);
    },
    onEnabledOptionChangeHandler(payload) {
      this.queryData.enabled = payload.value;
    },
  },
};
</script>

<style lang="scss">
.productsList {
  .inputGroup {
    .label {
      @apply min-w-[60px] text-left;
    }
    @apply mr-3 mb-2;
    .input {
      @apply bg-white border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none;
    }
  }
  .tableContainer {
  }
}
</style>
