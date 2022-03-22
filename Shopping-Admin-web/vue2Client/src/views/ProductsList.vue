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
              <div class="imgContainer" v-if="column === 'picture'">
                <img class="w-16" :src="item[column]" />
              </div>
              <div class="btnContainer" v-else-if="column === 'operation'">
                <BtnPrimary
                  :label="$t('common.edit')"
                  @submit="editHandler(item)"
                />
              </div>
              <span v-else>
                {{ extraFormatter(item, column) }}
              </span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- 編輯商品dialog -->
    <FormDialog
      v-if="isShowEditDialog"
      :isShowDialog="isShowEditDialog"
      :title="$t('productList.editFormTitle')"
      @toggle="toggleEditDialogHandler"
      @submit="onSubmitEditDialogHandler"
    >
      <div class="wrapper">
        <!-- 商品名稱 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t("common.tableHeader.name") }}
          </label>
          <input
            v-model="editData.name"
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
            :class="{ '!border-red-600': isDescWarning }"
            v-model="editData.description"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.description')"
            @focus="isDescWarning = false"
          />
        </div>

        <!-- 價格 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t("common.tableHeader.price") }}
          </label>
          <input
            :class="{ '!border-red-600': isPriceWarning }"
            v-model="editData.price"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.price')"
            @focus="isPriceWarning = false"
          />
        </div>

        <!-- 數量 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t("common.tableHeader.amount") }}
          </label>
          <input
            :class="{ '!border-red-600': isAmountWarning }"
            v-model="editData.amount"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.amount')"
            @focus="isAmountWarning = false"
          />
        </div>

        <!-- 類型 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t("common.tableHeader.type") }}
          </label>
          <input
            v-model="editData.type"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.type')"
          />
        </div>

        <!-- 狀態 -->
        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">禁用</label>
          <SwtichView :toggle="editData.enabled" @toggle="onDialogEditToggle" />
          <label class="whitespace-nowrap pr-3 leading-[42px]">啟用</label>
        </div>

        <!-- 照片 -->
        <div class="imgUploader">
          <div class="inputGroup flex">
            <label class="label whitespace-nowrap mr-3 leading-[42px]">
              {{ $t("common.tableHeader.picture") }}
            </label>
            <input
              :class="{ '!border-red-600': isFileWarning }"
              accept="image/*"
              type="file"
              class="input"
              :placeholder="$t('common.tableHeader.picture')"
              @change="uploadImage($event, 'edit')"
              @focus="isFileWarning = false"
            />
          </div>
          <div class="preview flex justify-center items-center">
            <img class="img w-[200px]" :src="previewImage" />
          </div>
        </div>
      </div>
    </FormDialog>

    <!-- 新增商品dialog -->
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
            :class="{ '!border-red-600': isDescWarning }"
            v-model="addData.desc"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.description')"
            @focus="isDescWarning = false"
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
            @focus="isPriceWarning = false"
          />
        </div>

        <!-- 數量 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t("common.tableHeader.amount") }}
          </label>
          <input
            :class="{ '!border-red-600': isAmountWarning }"
            v-model="addData.amount"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.amount')"
            @focus="isAmountWarning = false"
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
              :class="{ '!border-red-600': isFileWarning }"
              accept="image/*"
              type="file"
              class="input"
              :placeholder="$t('common.tableHeader.picture')"
              @change="uploadImage($event, 'add')"
              @focus="isFileWarning = false"
            />
          </div>
          <div class="preview flex justify-center items-center">
            <img class="img w-[200px]" :src="previewImage" />
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
import SwtichView from "../components/SwtichView.vue";
// import TableView from "../components/Table/TableView.vue";

import { GetProductsList, AddProduct, UpdateProduct } from "../APIs/Product";
import { DateTime } from "luxon";
import ErrorCodeList from "@/ErrorCodeList";
// import axios from "axios";
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
    SwtichView,
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
      isShowEditDialog: false,
      addData: {
        name: "",
        desc: "",
        price: null,
        amount: null,
        type: "",
        picture: null,
        file: null,
      },
      previewImage: null,
      isNameWarning: false,
      isPriceWarning: false,
      isAmountWarning: false,
      isDescWarning: false,
      isFileWarning: false,
      editData: {
        id: "",
        name: "",
        description: "",
        price: null,
        amount: null,
        type: "",
        enabled: null,
        file: null,
      },
    };
  },
  methods: {
    onDialogEditToggle(value) {
      this.editData.enabled = value;
    },
    async onSubmitEditDialogHandler() {
      const checkParams =
        this.checkName("edit") &&
        this.checkDescription("edit") &&
        this.checkPrice("edit") &&
        this.checkAmount("edit") &&
        this.checkFile("edit");

      if (checkParams) {
        const fd = new FormData();
        fd.append("id", this.editData.id);
        fd.append("name", this.editData.name);
        fd.append("price", this.editData.price);
        fd.append("amount", this.editData.amount);
        fd.append("description", this.editData.description);
        fd.append("type", this.editData.type);
        fd.append("picture", this.editData.picture);
        fd.append("enabled", this.editData.enabled);
        if (this.editData.file !== null) fd.append("file", this.editData.file);

        const res = await UpdateProduct(fd);
        if (res.code === 200) {
          this.searchHandler();
          this.$store.commit("eventBus/Push", {
            type: "success",
            content: this.$t("common.success"),
          });
        }

        this.clearEditData();
        this.previewImage = null;
        this.toggleEditDialogHandler();
      }
    },
    async editHandler(payload) {
      const { id } = payload;
      const fd = new FormData();
      fd.append("id", id);
      const res = await GetProductsList(fd);

      if (res.code === 200 && res.data.length) {
        this.editData.id = res.data[0].id;
        this.editData.name = res.data[0].name;
        this.editData.description = res.data[0].description;
        this.editData.price = res.data[0].price;
        this.editData.amount = res.data[0].amount;
        this.editData.type = res.data[0].type;
        this.editData.picture = res.data[0].picture;
        this.editData.enabled = res.data[0].enabled;
      }

      this.previewImage = this.editData.picture;
      this.toggleEditDialogHandler();
    },
    toggleEditDialogHandler() {
      this.clearFormWarning();
      this.isShowEditDialog = !this.isShowEditDialog;
    },
    clearEditData() {
      this.editData = {
        name: "",
        desc: "",
        price: null,
        amount: null,
        type: "",
        picture: null,
        enabled: null,
      };
    },
    clearFormWarning() {
      this.isNameWarning = false;
      this.isPriceWarning = false;
      this.isAmountWarning = false;
      this.isDescWarning = false;
      this.isFileWarning = false;
    },
    uploadImage(event, type) {
      const input = event.target;
      console.log("checkpointA=> ", input.files);
      if (input.files.length) {
        this.previewImage = URL.createObjectURL(input.files[0]);
        if (type === "add") {
          this.addData.file = input.files[0];
        } else if (type === "edit") {
          this.editData.file = input.files[0];
        }
      } else {
        this.addData.file = null;
        this.editData.file = null;
        this.previewImage = null;
      }
    },
    async onSubmitAddDialogHandler() {
      const checkParams =
        this.checkName("add") &&
        this.checkDescription("add") &&
        this.checkPrice("add") &&
        this.checkAmount("add") &&
        this.checkFile("add");

      if (checkParams) {
        const fd = new FormData();
        fd.append("name", this.addData.name);
        fd.append("price", this.addData.price);
        fd.append("amount", this.addData.amount);
        fd.append("description", this.addData.desc);
        fd.append("type", this.addData.type);
        fd.append("picture", this.addData.picture);
        fd.append("file", this.addData.file);

        const res = await AddProduct(fd);

        if (res.code === 200) {
          this.$store.commit("eventBus/Push", {
            type: "success",
            content: this.$t("common.success"),
          });
          this.searchHandler();
        } else {
          this.$store.commit("eventBus/Push", {
            type: "error",
            content: ErrorCodeList[res.code],
          });
        }
        this.toggleAddDialogHandler();
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
        file: null,
      };
      this.previewImage = null;
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
      const fd = new FormData();
      if (this.queryData.enabled !== 0) {
        fd.append(
          "enabled",
          this.queryData.enabled === EOptions.enabled ? true : false
        );
      }

      if (this.queryData.name.length) {
        fd.append("name", this.queryData.name);
      }

      if (this.queryData.type.length) {
        fd.append("type", this.queryData.type.length);
      }

      const res = await GetProductsList(fd);
      if (res.code === 200) {
        this.productList = res.data;
      } else {
        this.productList = [];
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: ErrorCodeList[res.code],
        });
      }
    },
    onEnabledOptionChangeHandler(payload) {
      this.queryData.enabled = payload.value;
    },
    checkName(srcType) {
      // 檢查名稱不可為空
      if (srcType === "add") {
        if (this.addData.name.length) {
          return true;
        }

        this.isNameWarning = true;
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
        return false;
      } else {
        if (this.editData.name.length) {
          return true;
        }

        this.isNameWarning = true;
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
        return false;
      }
    },

    checkDescription(srcType) {
      // 檢查描述不可為空
      if (srcType === "add") {
        if (this.addData.desc.length) {
          return true;
        }

        this.isDescWarning = true;
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
        return false;
      } else {
        if (this.editData.description.length) {
          return true;
        }

        this.isDescWarning = true;
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
        return false;
      }
    },
    checkPrice(srcType) {
      // 檢查price
      if (srcType === "add") {
        const isPriceNum = /^\d+$/.test(this.addData.price);
        if (isPriceNum) {
          return true;
        }

        this.isPriceWarning = true;
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
        return false;
      } else {
        const isPriceNum = /^\d+$/.test(this.editData.price);
        if (isPriceNum) {
          return true;
        }

        this.isPriceWarning = true;
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
        return false;
      }
    },
    checkAmount(srcType) {
      // 檢查amount
      if (srcType === "add") {
        const isAmountNum = /^\d+$/.test(this.addData.amount);
        if (isAmountNum) {
          return true;
        }

        this.isAmountWarning = true;
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
        return false;
      } else {
        const isAmountNum = /^\d+$/.test(this.editData.amount);
        if (isAmountNum) {
          return true;
        }

        this.isAmountWarning = true;
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
        return false;
      }
    },
    checkFile(srcType) {
      // 檢查file
      if (srcType === "add") {
        if (this.addData.file) {
          return true;
        }

        this.isFileWarning = true;
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
        return false;
      } else {
        if (this.editData.file) {
          return true;
        }

        this.isFileWarning = true;
        this.$store.commit("eventBus/Push", {
          type: "error",
          content: this.$t("productList.checkInputsPlz"),
        });
        return false;
      }
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
