<template>
  <div class="productsList p-5">
    <!-- input query -->
    <div class="query flex">
      <div class="inputGroup flex">
        <label class="whitespace-nowrap pr-3 leading-[42px]">
          {{ $t('common.tableHeader.name') }}
        </label>
        <input type="text" class="input" v-model="queryData.name" required />
      </div>
      <div class="inputGroup flex">
        <label class="whitespace-nowrap pr-3 leading-[42px]">
          {{ $t('common.tableHeader.type') }}
        </label>
        <input type="text" class="input" v-model="queryData.type" required />
      </div>
      <OptionSelector
        class="px-2"
        :options="options"
        :value.sync="queryData.enabled"
      />

      <BtnPrimary
        :label="$t('common.search')"
        class="pr-2"
        @submit="SearchHandler"
      />
    </div>
    <hr class="my-2" />
    <div class="add flex">
      <BtnPrimary
        :label="$t('common.add')"
        class="pr-2"
        @submit="ToggleAddDialogHandler"
      />
    </div>

    <!-- table -->
    <div class="tableContainer pt-5">
      <table class="w-full">
        <thead>
          <tr
            class="text-left border-t border-b-2 border-grey2 p-2 bg-blue-400 text-white text-xl;"
          >
            <th
              class="py-3 px-1 min-w-[120px]"
              scope="col"
              v-for="col in columns"
              :key="col"
            >
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
              class="py-3 px-1 text-left"
            >
              <div class="max-w-xs">
                <div class="imgContainer" v-if="column === 'picture'">
                  <img class="w-16" :src="item[column]" />
                </div>
                <div
                  class="btnContainer min-w-[64px]"
                  v-else-if="column === 'operation'"
                >
                  <BtnPrimary
                    :label="$t('common.edit')"
                    @submit="EditHandler(item)"
                  />
                </div>
                <span v-else class="break-all">
                  {{ ExtraFormatter(item, column) }}
                </span>
              </div>
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
      @toggle="ToggleEditDialogHandler"
      @submit="SubmitEditFormHandler"
    >
      <div class="wrapper">
        <!-- 商品名稱 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t('common.tableHeader.name') }}
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
            {{ $t('common.tableHeader.description') }}
          </label>
          <textarea
            :class="{ '!border-red-600': isDescWarning }"
            class="textarea"
            v-model="editData.description"
            type="text"
            :placeholder="$t('common.tableHeader.description')"
            @focus="isDescWarning = false"
          />
        </div>

        <!-- 價格 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t('common.tableHeader.price') }}
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
            {{ $t('common.tableHeader.amount') }}
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
            {{ $t('common.tableHeader.type') }}
          </label>
          <input
            :class="{ '!border-red-600': isTypeWarning }"
            v-model="editData.type"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.type')"
            @focus="isTypeWarning = false"
          />
        </div>

        <!-- 狀態 -->
        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">禁用</label>
          <SwtichView
            :toggle="editData.enabled"
            @toggle="ToggleEditDialogSwitch"
          />
          <label class="whitespace-nowrap pr-3 leading-[42px]">啟用</label>
        </div>

        <!-- 照片 -->
        <div class="imgUploader">
          <div class="inputGroup flex">
            <label class="label whitespace-nowrap mr-3 leading-[42px]">
              {{ $t('common.tableHeader.picture') }}
            </label>
            <input
              :class="{ '!border-red-600': isFileWarning }"
              accept="image/*"
              type="file"
              class="input"
              :placeholder="$t('common.tableHeader.picture')"
              @change="UploadImage($event, 'edit')"
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
      @toggle="ToggleAddDialogHandler"
      @submit="SubmitAddFormHandler"
      :title="$t('productList.addFormTitle')"
    >
      <div class="wrapper">
        <!-- 商品名稱 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t('common.tableHeader.name') }}
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
            {{ $t('common.tableHeader.description') }}
          </label>
          <textarea
            :class="{ '!border-red-600': isDescWarning }"
            v-model="addData.desc"
            type="text"
            class="textarea"
            :placeholder="$t('common.tableHeader.description')"
            @focus="isDescWarning = false"
          />
        </div>

        <!-- 價格 -->
        <div class="inputGroup flex">
          <label class="label whitespace-nowrap mr-3 leading-[42px]">
            {{ $t('common.tableHeader.price') }}
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
            {{ $t('common.tableHeader.amount') }}
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
            {{ $t('common.tableHeader.type') }}
          </label>
          <input
            :class="{ '!border-red-600': isTypeWarning }"
            v-model="addData.type"
            type="text"
            class="input"
            :placeholder="$t('common.tableHeader.type')"
            @focus="isTypeWarning = false"
          />
        </div>

        <!-- 照片 -->
        <div class="imgUploader">
          <div class="inputGroup flex">
            <label class="label whitespace-nowrap mr-3 leading-[42px]">
              {{ $t('common.tableHeader.picture') }}
            </label>
            <input
              :class="{ '!border-red-600': isFileWarning }"
              accept="image/*"
              type="file"
              class="input"
              :placeholder="$t('common.tableHeader.picture')"
              @change="UploadImage($event, 'add')"
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
import BtnPrimary from '../components/BtnPrimary.vue';
import OptionSelector from '../components/OptionSelector.vue';
import FormDialog from '../components/Dialogs/DialogView.vue';
import SwtichView from '../components/SwtichView.vue';
// import TableView from "../components/Table/TableView.vue";

import { GetProductsList, AddProduct, UpdateProduct } from '../APIs/Product';
import { DateTime } from 'luxon';
import ErrorCodeList from '@/ErrorCodeList';
import { IsContaineSpecialCharaters, IsPureNumber } from '@/Utils/validators';
const EOptions = {
  all: 0,
  enabled: 1,
  disabled: 2,
};

export default {
  name: 'productsList',
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
          label: '全部',
          value: EOptions.all,
        },
        {
          label: '啟用',
          value: EOptions.enabled,
        },
        {
          label: '禁用',
          value: EOptions.disabled,
        },
      ],
      columns: [
        'id',
        'name',
        'description',
        'price',
        'picture',
        'amount',
        'type',
        'enabled',
        'updatedDate',
        'operation',
      ],
      btnDisabledList: [],
      queryData: {
        enabled: EOptions.all,
        name: '',
        type: '',
      },
      productList: [],
      isShowAddDialog: false,
      isShowEditDialog: false,
      addData: {
        name: '',
        desc: '',
        price: null,
        amount: null,
        type: '',
        picture: null,
        file: null,
      },
      previewImage: null,
      isNameWarning: false,
      isPriceWarning: false,
      isAmountWarning: false,
      isDescWarning: false,
      isTypeWarning: false,
      isFileWarning: false,
      editData: {
        id: '',
        name: '',
        description: '',
        price: null,
        amount: null,
        type: '',
        enabled: null,
        file: null,
      },
    };
  },
  computed: {
    forceInit() {
      return this.$store.state.forceInit;
    },
  },
  watch: {
    forceInit: {
      immediate: true,
      handler: function (val) {
        if (val) {
          this.InitQueryData();
          this.SearchHandler();
          this.$store.commit('setForceInit', false);
        }
      },
    },
  },
  created() {
    this.SearchHandler();
  },
  methods: {
    InitQueryData() {
      this.queryData.enabled = EOptions.all;
      this.queryData.name = '';
      this.queryData.type = '';
    },
    ToggleEditDialogSwitch(value) {
      this.editData.enabled = value;
    },
    async SubmitEditFormHandler() {
      const isNameValid = this.CheckString(this.editData.name);
      if (!isNameValid) this.isNameWarning = true;

      const isDescValid = this.CheckString(this.editData.description);
      if (!isDescValid) this.isDescWarning = true;

      const isPriceValid = this.CheckNumber(this.editData.price);
      if (!isPriceValid) this.isPriceWarning = true;

      const isAmountValid = this.CheckNumber(this.editData.amount);
      if (!isAmountValid) this.isAmountWarning = true;

      const isTypeValid = this.CheckString(this.editData.type);
      if (!isTypeValid) this.isTypeWarning = true;

      if (
        isNameValid &&
        isDescValid &&
        isPriceValid &&
        isAmountValid &&
        isTypeValid
      ) {
        const fd = new FormData();
        fd.append('id', this.editData.id);
        fd.append('name', this.editData.name);
        fd.append('price', this.editData.price);
        fd.append('amount', this.editData.amount);
        fd.append('description', this.editData.description);
        fd.append('type', this.editData.type);
        fd.append('picture', this.editData.picture);
        fd.append('enabled', this.editData.enabled);
        if (this.editData.file !== null) fd.append('file', this.editData.file);

        const res = await UpdateProduct(fd);
        if (res.code === 200) {
          this.SearchHandler();
          this.$store.commit('eventBus/Push', {
            type: 'success',
            content: this.$t('common.success'),
          });
        }

        this.ClearEditData();
        this.previewImage = null;
        this.ToggleEditDialogHandler();
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: this.$t('productList.checkInputsPlz'),
        });
      }
    },
    async EditHandler(payload) {
      const { id } = payload;
      const fd = new FormData();
      fd.append('id', id);
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
      this.ToggleEditDialogHandler();
    },
    ToggleEditDialogHandler() {
      this.ClearFormWarning();
      this.isShowEditDialog = !this.isShowEditDialog;
    },
    ClearEditData() {
      this.editData = {
        name: '',
        desc: '',
        price: null,
        amount: null,
        type: '',
        picture: null,
        enabled: null,
      };
    },
    ClearFormWarning() {
      this.isNameWarning = false;
      this.isPriceWarning = false;
      this.isAmountWarning = false;
      this.isDescWarning = false;
      this.isTypeWarning = false;
      this.isFileWarning = false;
    },
    UploadImage(event, type) {
      const input = event.target;
      if (input.files.length) {
        this.previewImage = URL.createObjectURL(input.files[0]);
        if (type === 'add') {
          this.addData.file = input.files[0];
        } else if (type === 'edit') {
          this.editData.file = input.files[0];
        }
      } else {
        this.addData.file = null;
        this.editData.file = null;
        this.previewImage = null;
      }
    },
    async SubmitAddFormHandler() {
      const isNameValid = this.CheckString(this.addData.name);
      if (!isNameValid) this.isNameWarning = true;

      const isDescValid = this.CheckString(this.addData.desc);
      if (!isDescValid) this.isDescWarning = true;

      const isPriceValid = this.CheckNumber(this.addData.price);
      if (!isPriceValid) this.isPriceWarning = true;

      const isAmountValid = this.CheckNumber(this.addData.amount);
      if (!isAmountValid) this.isAmountWarning = true;

      const isTypeValid = this.CheckString(this.addData.type);
      if (!isTypeValid) this.isTypeWarning = true;

      const isFileExist = this.CheckFile(this.addData.file);
      if (!isFileExist) this.isFileWarning = true;

      if (
        isNameValid &&
        isDescValid &&
        isPriceValid &&
        isAmountValid &&
        isTypeValid &&
        isFileExist
      ) {
        const fd = new FormData();
        fd.append('name', this.addData.name);
        fd.append('price', this.addData.price);
        fd.append('amount', this.addData.amount);
        fd.append('description', this.addData.desc);
        fd.append('type', this.addData.type);
        fd.append('picture', this.addData.picture);
        fd.append('file', this.addData.file);

        const res = await AddProduct(fd);

        if (res.code === 200) {
          this.$store.commit('eventBus/Push', {
            type: 'success',
            content: this.$t('common.success'),
          });
          this.SearchHandler();
        } else {
          this.$store.commit('eventBus/Push', {
            type: 'error',
            content: ErrorCodeList[res.code],
          });
        }
        this.ToggleAddDialogHandler();
      } else {
        // 輸入欄位異常通知
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: this.$t('productList.checkInputsPlz'),
        });
      }
    },
    ToggleAddDialogHandler() {
      this.isShowAddDialog = !this.isShowAddDialog;
      this.ClearAddData();
      this.ClearFormWarning();
    },
    ClearAddData() {
      this.addData = {
        name: '',
        desc: '',
        price: null,
        amount: null,
        type: '',
        file: null,
      };
      this.previewImage = null;
    },
    ExtraFormatter(data, column) {
      // 將狀態做i18n
      if (column === 'enabled') {
        return data[column]
          ? this.$t('common.tableHeader.enabled')
          : this.$t('common.tableHeader.disabled');
      } else if (column === 'createdDate' || column === 'updatedDate') {
        // 將日期做格式化
        const date = new Date(data[column]);
        const dt = DateTime.fromJSDate(date);
        return dt.toFormat('yyyy/MM/dd HH:mm:ss');
      } else {
        return data[column];
      }
    },
    async SearchHandler() {
      const fd = new FormData();
      if (this.queryData.enabled !== 0) {
        fd.append(
          'enabled',
          this.queryData.enabled === EOptions.enabled ? true : false
        );
      }

      if (this.queryData.name.length) {
        fd.append('name', this.queryData.name);
      }

      if (this.queryData.type.length) {
        fd.append('type', this.queryData.type);
      }

      const res = await GetProductsList(fd);
      if (res.code === 200) {
        this.productList = res.data;
      } else {
        this.productList = [];
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: ErrorCodeList[res.code],
        });
      }
    },

    // 檢查字串不包含特殊字元, 空白
    CheckString(str) {
      if (!str.length) {
        return false;
      }
      if (IsContaineSpecialCharaters(str)) {
        return false;
      }
      return true;
    },

    // 檢查數字不包含小數點, 空白
    CheckNumber(number) {
      if (IsPureNumber(number)) {
        return true;
      }
      return false;
    },

    // 檢查file
    CheckFile(file) {
      return file !== null;
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
    .textarea {
      @apply w-full h-20 p-2 resize-none bg-white border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 outline-none;
    }
  }
}
</style>
