<template>
  <div class="agentsList p-5">
    <!-- input query -->
    <div class="inputGroup flex">
      <BtnSubmit :label="$t('common.add')" class="pr-2" @submit="AddHandler" />
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

    <!-- 新增帳號dialog -->
    <FormDialog
      v-if="isShowAddDialog"
      :isShowDialog="isShowAddDialog"
      :title="$t('agentList.addFormTitle')"
      @toggle="ToggleAddDialogHandler"
      @submit="SubmitAddFormHandler"
    >
      <form>
        <!-- 帳號 -->
        <div class="inputGroup flex relative !pb-9">
          <label class="whitespace-nowrap pr-3 leading-[42px]">
            {{ $t('common.tableHeader.account') }}
          </label>
          <input
            :class="{ '!border-red-600': isAccountWarning }"
            v-model="dialogAccount"
            type="text"
            class="input"
            :placeholder="$t('agentList.accountPlaceholder')"
            required
            @focus="isAccountWarning = false"
          />
          <p
            v-show="isAccountWarning"
            class="text-red-500 text-xs italic absolute bottom-0"
          >
            {{ $t('agentList.accountWarning') }}
          </p>
        </div>

        <!-- 密碼 -->
        <div class="inputGroup flex relative !pb-6">
          <label class="whitespace-nowrap pr-3 leading-[42px]">
            {{ $t('common.pwd') }}
          </label>
          <input
            :class="{ '!border-red-600': isPwdWarning }"
            v-model="dialogPwd"
            type="password"
            class="input"
            :placeholder="$t('agentList.pwdPlaceholder')"
            required
            @focus="isPwdWarning = false"
          />
          <p
            v-show="isPwdWarning"
            class="text-red-500 text-xs italic absolute bottom-0"
          >
            {{ $t('agentList.pwdWarning') }}
          </p>
        </div>
      </form>
    </FormDialog>

    <!-- table -->
    <div class="tableContainer pt-5">
      <TableView
        :datas="sourceAgentsList"
        :btnDisabledList="btnDisabledList"
        @edit="EditHandler"
        @unlockCompleted="UnlockCompletedHandler"
      />
    </div>

    <!-- 編輯帳號dialog -->
    <FormDialog
      v-if="isShowEditDialog"
      :isShowDialog="isShowEditDialog"
      :title="$t('agentList.editFormTitle')"
      @toggle="ToggleEditDialogHandler"
      @submit="SubmitEditFormHandler"
    >
      <div class="wrapper">
        <!-- 帳號 -->
        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">
            {{ $t('common.tableHeader.account') }}
          </label>
          <input
            type="text"
            class="input"
            readonly
            :placeholder="oldAccountInfo.account"
          />
        </div>

        <!-- 啟用 -->
        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">
            {{ $t('common.tableHeader.enabled') }}
          </label>
          <SwtichView
            :toggle="dialogEditEnabled"
            @toggle="ToggleEditDialogSwitch"
          />
        </div>
      </div>
    </FormDialog>
  </div>
</template>

<script>
import BtnSubmit from '@/components/BtnPrimary.vue';
import FormDialog from '@/components/Dialogs/DialogView.vue';
import {
  GetAgentByAccount,
  RegisterAgent,
  UpdateAgent,
  GetAgentsListByStatus,
} from '@/APIs/Agent';
import TableView from '@/components/Table/TableView.vue';
import OptionSelector from '@/components/OptionSelector.vue';
import SwtichView from '@/components/SwtichView.vue';
import { IsAccountValid, IsPwdValid } from '../Utils/validators';
import errorList from '../ErrorCodeList';
const EOptions = {
  all: 0,
  enabled: 1,
  disabled: 2,
};

export default {
  name: 'agentsList',
  components: {
    BtnSubmit,
    FormDialog,
    TableView,
    OptionSelector,
    SwtichView,
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
      sourceAgentsList: [],
      queryData: {
        enabled: EOptions.all,
      },
      isShowAddDialog: false,
      dialogAccount: '',
      dialogPwd: '',
      isAccountWarning: false,
      isPwdWarning: false,
      isShowEditDialog: false,

      dialogEditPwd: '',
      dialogEditEnabled: true,
      oldAccountInfo: {
        account: '',
        enabled: null,
        pwd: '',
        role: '',
      },
      btnDisabledList: [],
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
  async mounted() {
    this.SearchHandler();
  },
  methods: {
    InitQueryData() {
      this.queryData.enabled = EOptions.all;
    },
    async UnlockCompletedHandler(payload) {
      const res = this.sourceAgentsList.find(
        (agent) => agent.account === payload.account
      );
      if (res) {
        res.count = 0;
      }
    },
    ClearEditDialog() {
      this.oldAccountInfo = {};
      this.dialogEditPwd = '';
      this.dialogEditEnabled = true;
    },
    ToggleEditDialogHandler(payload) {
      this.isShowEditDialog = payload;
    },
    async SubmitEditFormHandler() {
      const res = await UpdateAgent({
        account: this.oldAccountInfo.account,
        enabled: this.dialogEditEnabled,
        role: this.oldAccountInfo.role,
      });
      if (res.code === 200) {
        this.SearchHandler();
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: errorList[res.code],
        });
      }

      this.ToggleEditDialogHandler(!this.isShowEditDialog);
      this.ClearEditDialog();
    },
    ToggleEditDialogSwitch(value) {
      this.dialogEditEnabled = value;
    },
    async EditHandler(accountInfo) {
      this.ToggleEditDialogHandler(!this.isShowEditDialog);
      this.oldAccountInfo.account = accountInfo.account;

      const fd = new FormData();
      fd.append('account', this.oldAccountInfo.account);
      // 取得欲編輯帳號的完整帳號資訊
      const res = await GetAgentByAccount(fd);

      if (res.code === 200) {
        this.oldAccountInfo.pwd = res.data[0].pwd;
        this.oldAccountInfo.enabled = res.data[0].enabled;
        this.oldAccountInfo.role = res.data[0].role;
        this.ToggleEditDialogSwitch(!!res.data[0].enabled);
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: errorList[res.code],
        });
      }
    },

    async SearchHandler() {
      const fd = new FormData();
      if (this.queryData.enabled !== 0) {
        fd.append('enabled', this.queryData.enabled === EOptions.enabled);
      }
      this.sourceAgentsList = [];
      const res = await GetAgentsListByStatus(fd);
      if (res.code === 200) this.sourceAgentsList = res.data;
      else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: errorList[res.code],
        });
      }
    },
    async RegisterHandler() {
      const res = await RegisterAgent({
        account: this.dialogAccount,
        pwd: this.dialogPwd,
      });
      if (res.code === 200) {
        this.ToggleAddDialogHandler(false);
        this.dialogAccount = '';
        this.dialogPwd = '';

        // 新增成功後重取清單
        this.SearchHandler();
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: errorList[res.code],
        });
      }
    },
    SubmitAddFormHandler() {
      const isAccountValid = IsAccountValid(this.dialogAccount);
      if (!isAccountValid) this.isAccountWarning = true;

      const isPwdValid = IsPwdValid(this.dialogPwd);
      if (!isPwdValid) this.isPwdWarning = true;

      if (isAccountValid && isPwdValid) this.RegisterHandler();
    },
    AddHandler() {
      this.isShowAddDialog = !this.isShowAddDialog;
    },
    ToggleAddDialogHandler(payload) {
      this.InitAddDialogForm();
      this.isShowAddDialog = payload;
    },
    InitAddDialogForm() {
      this.dialogAccount = '';
      this.dialogPwd = '';
      this.isAccountWarning = false;
      this.isPwdWarning = false;
    },
  },
};
</script>

<style lang="scss">
.agentsList {
  .inputGroup {
    @apply p-3 pl-0;
    .input {
      @apply bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none;
    }
  }
}
</style>
