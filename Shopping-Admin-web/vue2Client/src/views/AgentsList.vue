<template>
  <div class="agentsList p-5">
    <!-- input query -->
    <div class="inputGroup flex">
      <BtnSubmit label="新增" class="pr-2" @submit="AddHandler" />
      <OptionSelector
        class="px-2"
        :options="options"
        @onChange="EnabledOptionChangeHandler"
      />
      <BtnSubmit label="搜尋" class="pr-2" @submit="SearchHandler" />
    </div>

    <!-- 新增帳號dialog -->
    <FormDialog
      v-if="isShowAddDialog"
      :isShowDialog="isShowAddDialog"
      title="新增後台帳號"
      @toggle="ToggleAddDialogHandler"
      @submit="SubmitAddFormHandler"
    >
      <form>
        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">帳號</label>
          <input
            v-model="dialogAccount"
            type="text"
            class="input"
            placeholder="請輸入帳號"
            required
            @keypress="KeypressHandler"
          />
        </div>
        <p v-show="isInvalid" class="text-red-500 text-xs italic">
          帳號需以英文開頭,英數字皆可,介於6~20字元
        </p>

        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">密碼</label>
          <input
            v-model="dialogPwd"
            type="password"
            class="input"
            placeholder="請輸入密碼"
            required
            @keypress="KeypressHandler"
          />
        </div>
        <p v-show="isInvalid" class="text-red-500 text-xs italic">
          密碼須包含大小寫字母及數字,超過6字元
        </p>
      </form>
    </FormDialog>

    <!-- table -->
    <div class="tableContainer pt-5">
      <TableView
        :datas="agentsListComputed"
        :btnDisabledList="btnDisabledList"
        @edit="EditHandler"
        @unlockCompleted="UnlockCompletedHandler"
      />
    </div>

    <!-- 編輯帳號dialog -->
    <FormDialog
      v-if="isShowEditDialog"
      :isShowDialog="isShowEditDialog"
      title="編輯帳號"
      @toggle="ToggleEditDialogHandler"
      @submit="SubmitEditFormHandler"
    >
      <div class="wrapper">
        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">帳號</label>
          <input
            type="text"
            class="input"
            readonly
            :placeholder="oldAccountInfo.account"
          />
        </div>
        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">禁用</label>
          <SwtichView
            :toggle="dialogEditEnabled"
            @toggle="ToggleEditDialogSwitch"
          />
          <label class="whitespace-nowrap pr-3 leading-[42px]">啟用</label>
        </div>
      </div>
    </FormDialog>
  </div>
</template>

<script>
import BtnSubmit from '@/components/BtnPrimary.vue';
import FormDialog from '@/components/Dialogs/DialogView.vue';
import { GetAgentsList, RegisterAgent, UpdateAgent } from '@/APIs/Agent';
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
        enabled: 0,
      },
      queryEnabled: 0,
      isShowAddDialog: false,
      dialogAccount: '',
      dialogPwd: '',
      isInvalid: false,
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
  async mounted() {
    const res = await GetAgentsList();
    if (res && res.code === 200) this.sourceAgentsList = res.data;
    else {
      this.$store.commit('eventBus/Push', {
        type: 'error',
        content: errorList[res.code],
      });
    }
  },
  methods: {
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
        const response = await GetAgentsList();
        if (response.code === 200) {
          this.$store.commit('eventBus/Push', {
            type: 'success',
            content: this.$t('common.success'),
          });
          this.sourceAgentsList = response.data;
        } else {
          this.$store.commit('eventBus/Push', {
            type: 'error',
            content: errorList[res.code],
          });
        }
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

      // 取得欲編輯帳號的完整帳號資訊
      const res = await GetAgentsList({
        account: this.oldAccountInfo.account,
      });
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
      this.queryData.enabled = this.queryEnabled;
      this.sourceAgentsList = [];
      const res = await GetAgentsList();
      if (res.code === 200) this.sourceAgentsList = res.data;
      else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: errorList[res.code],
        });
      }
    },
    EnabledOptionChangeHandler(payload) {
      this.queryEnabled = payload.value;
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
        const res = await GetAgentsList();
        if (res.code === 200) {
          this.sourceAgentsList = res.data;
          this.$store.commit('eventBus/Push', {
            type: 'success',
            content: '新增成功',
          });
        } else {
          this.$store.commit('eventBus/Push', {
            type: 'error',
            content: errorList[res.code],
          });
        }
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: errorList[res.code],
        });
      }
    },
    SubmitAddFormHandler() {
      let isValid = false;

      isValid =
        IsAccountValid(this.dialogAccount) && IsPwdValid(this.dialogPwd);

      if (isValid) {
        this.RegisterHandler();
      } else {
        this.isInvalid = true;
      }
    },
    KeypressHandler() {
      this.isInvalid = false;
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
      this.isInvalid = false;
    },
  },
  computed: {
    agentsListComputed() {
      if (this.sourceAgentsList && this.sourceAgentsList.length) {
        if (this.queryData.enabled === EOptions.all) {
          return this.sourceAgentsList;
        }
        return this.sourceAgentsList.filter((item) => {
          if (this.queryData.enabled === EOptions.enabled) {
            return item.enabled;
          } else if (this.queryData.enabled === EOptions.disabled) {
            return item.enabled === 0;
          }
        });
      }
      return [];
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
