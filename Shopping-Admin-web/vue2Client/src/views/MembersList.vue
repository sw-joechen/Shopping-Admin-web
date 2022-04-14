<template>
  <div class="membersList p-5">
    <!-- input query -->
    <div class="query flex">
      <!-- 會員帳號 -->
      <div class="inputGroup flex">
        <label class="whitespace-nowrap pr-3 leading-[42px]">
          {{ $t('common.tableHeader.account') }}
        </label>
        <input type="text" class="input" v-model="queryData.account" />
      </div>

      <!-- 啟用/禁用 -->
      <OptionSelector
        class="px-2"
        :options="options"
        :value.sync="queryData.enabled"
      />
      <BtnSubmit
        :label="$t('common.search')"
        class="pr-2"
        @submit="SearchHandler"
      />
    </div>

    <hr class="mb-2" />

    <TableView
      :datas="memberList"
      @editComplete="EditCompleteHandler"
      @editDeposit="EditDepositHandler"
    />

    <FormDialog
      v-if="isShowEditDepositDialog"
      title="儲值"
      :isShowDialog="isShowEditDepositDialog"
      @toggle="ToggleEditDepositDialogHandler"
      @submit="SubmitEditDepositFormHandler"
    >
      <!-- 帳號 -->
      <div class="inputGroup relative flex">
        <label class="label whitespace-nowrap mr-3 leading-[42px]">
          {{ $t('common.tableHeader.account') }}
        </label>
        <input
          v-model="editData.account"
          class="input !bg-[#efefef] !opacity-30"
          type="text"
          disabled
          readonly
        />
      </div>

      <!-- 餘額 -->
      <div class="inputGroup relative flex">
        <label class="label whitespace-nowrap mr-3 leading-[42px]">
          {{ $t('common.tableHeader.balance') }}
        </label>
        <input
          v-model="editData.balance"
          class="input !bg-[#efefef] !opacity-30"
          type="text"
          disabled
          readonly
        />
      </div>

      <!-- 儲值金額 -->
      <div class="inputGroup relative flex">
        <label class="label whitespace-nowrap mr-3 leading-[42px]">
          {{ $t('memberList.depositCash') }}
        </label>
        <input
          :class="{ '!border-red-600': isCashWarning }"
          v-model="editData.cash"
          class="input"
          type="number"
          @keyup="keypressHandler"
          @focus="isCashWarning = false"
          @paste="NumberInputPasteHandler"
        />
        <p
          v-show="isCashWarning"
          class="text-red-500 text-xs italic absolute bottom-0 right-0"
        >
          輸入格式錯誤
        </p>
      </div>
    </FormDialog>
  </div>
</template>

<script>
import BtnSubmit from '@/components/BtnPrimary.vue';
import OptionSelector from '@/components/OptionSelector.vue';
import TableView from '@/components/Table/MemberListTable.vue';
import FormDialog from '@/components/Dialogs/DialogView.vue';

import { GetMembersList, Deposit } from '@/APIs/Member';
import ErrorCodeList from '@/ErrorCodeList';
import { IsPureNumber } from '@/Utils/validators';

const EOptions = {
  all: 0,
  enabled: 1,
  disabled: 2,
};

export default {
  name: 'membersList',
  components: {
    BtnSubmit,
    OptionSelector,
    TableView,
    FormDialog,
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
      queryData: {
        account: '',
        enabled: 0,
      },
      memberList: [],
      isShowEditDepositDialog: false,
      editData: {
        account: '',
        balance: null,
        cash: null,
      },
      isCashWarning: false,
    };
  },
  created() {
    this.SearchHandler();
  },
  methods: {
    async SubmitEditDepositFormHandler() {
      const isCashValid = this.CheckNumber(this.editData.cash);
      if (!isCashValid) {
        this.isCashWarning = true;
        return;
      }

      const isCashPositive = this.editData.cash > 0;
      if (!isCashPositive) this.isCashWarning = true;

      if (isCashValid && isCashPositive) {
        const fd = new FormData();
        fd.append('account', this.editData.account);
        fd.append('cash', this.editData.cash);
        try {
          const res = await Deposit(fd);
          if (res.code === 200) {
            this.SearchHandler();
            this.$store.commit('eventBus/Push', {
              type: 'success',
              content: this.$t('common.success'),
            });
          }
        } catch (e) {
          console.log({ e });
          this.$store.commit('eventBus/Push', {
            type: 'error',
            content: ErrorCodeList[101],
          });
        }

        this.ClearEditData();
        this.ToggleEditDepositDialogHandler(!this.isShowEditDepositDialog);
      }
    },
    ClearEditData() {
      this.editData.account = '';
      this.editData.cash = null;
      this.editData.balance = null;
    },
    keypressHandler(event) {
      if (event.target.value.search(/^0/) != -1) {
        alert('數字禁止0開頭');
      }
      return event.charCode >= 48 && event.charCode <= 57
        ? true
        : event.preventDefault();
    },
    NumberInputPasteHandler(e) {
      if (e.clipboardData.getData('Text').match(/[^\d]/)) {
        e.target.blur();
        this.isCashWarning = true;
        e.preventDefault();
      }
    },
    async EditDepositHandler(accountInfo) {
      this.ToggleEditDepositDialogHandler(!this.isShowEditDepositDialog);
      this.editData.account = accountInfo.account;

      const fd = new FormData();
      fd.append('account', this.editData.account);
      // 取得欲編輯帳號的完整帳號資訊
      const res = await GetMembersList(fd);
      if (res.code === 200 && res.data.length) {
        this.editData.balance = res.data[0].balance;
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: ErrorCodeList[res.code],
        });
      }
    },
    ToggleEditDepositDialogHandler(payload) {
      this.isShowEditDepositDialog = payload;
    },
    async SearchHandler() {
      const fd = new FormData();
      if (this.queryData.enabled !== 0) {
        fd.append('enabled', this.queryData.enabled === EOptions.enabled);
      }

      if (this.queryData.account.length) {
        fd.append('account', this.queryData.account);
      }

      const res = await GetMembersList(fd);
      if (res.code === 200) {
        this.memberList = res.data;
      } else {
        this.memberList = [];
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: ErrorCodeList[res.code],
        });
      }
    },
    EditCompleteHandler() {
      this.SearchHandler();
    },

    // 檢查數字不包含小數點, 空白
    CheckNumber(number) {
      return IsPureNumber(number);
    },
  },
};
</script>

<style lang="scss">
.membersList {
  input::-webkit-outer-spin-button,
  input::-webkit-inner-spin-button {
    -webkit-appearance: none;
    margin: 0;
  }
  .inputGroup {
    @apply mr-3 pb-5;
    .label {
      @apply min-w-[60px] text-left;
    }
    .input {
      @apply bg-white border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none;
    }
  }
}
</style>
