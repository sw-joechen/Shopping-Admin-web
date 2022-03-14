<template>
  <div class="agentsList p-5">
    <!-- input query -->
    <div class="inputGroup flex">
      <BtnSubmit label="新增" class="pr-2" @submit="onAddHandler" />
      <OptionSelector
        class="px-2"
        :options="options"
        @onChange="onEnabledOptionChangeHandler"
      />
      <BtnSubmit label="搜尋" class="pr-2" @submit="onSearchHandler" />
    </div>

    <!-- 新增帳號dialog -->
    <FormDialog
      v-if="isShowDialog"
      :isShowDialog="isShowDialog"
      title="新增後台帳號"
      @toggle="toggleHandler"
      @submit="onSubmitHandler"
    >
      <form>
        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">帳號</label>
          <input
            v-model="dialogAccount"
            type="text"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none"
            placeholder="請輸入帳號"
            required
            @keypress="changeHandler"
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
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none"
            placeholder="請輸入密碼"
            required
            @keypress="changeHandler"
          />
        </div>
        <p v-show="isInvalid" class="text-red-500 text-xs italic">
          密碼須包含大小寫字母及數字,超過6字元
        </p>
      </form>
    </FormDialog>

    <!-- table -->
    <div class="tableContainer pt-5">
      <TableView :datas="agentsListComputed" />
    </div>
  </div>
</template>

<script>
import BtnSubmit from "@/components/BtnPrimary.vue";
import FormDialog from "@/components/Dialogs/DialogView.vue";
import { getAgentsList, registerAgent } from "@/APIs/Agent";
import TableView from "@/components/Table/TableView.vue";
import OptionSelector from "@/components/OptionSelector.vue";
const EOptions = {
  all: 0,
  enabled: 1,
  disabled: 2,
};

export default {
  name: "agentsList",
  components: {
    BtnSubmit,
    FormDialog,
    TableView,
    OptionSelector,
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
      sourceAgentsList: [],
      queryData: {
        enabled: 0,
      },
      queryEnabled: 0,
      isShowDialog: false,
      dialogAccount: "",
      dialogPwd: "",
      isInvalid: false,
    };
  },
  async mounted() {
    const res = await getAgentsList();
    this.sourceAgentsList = res.data;
  },
  methods: {
    async onSearchHandler() {
      this.queryData.enabled = this.queryEnabled;
      console.log("queryData.enabled=>", this.queryData.enabled);

      this.sourceAgentsList = [];
      const res = await getAgentsList();
      this.sourceAgentsList = res.data;
    },
    onEnabledOptionChangeHandler(payload) {
      this.queryEnabled = payload.value;
    },
    async registerHandler() {
      const res = await registerAgent({
        account: this.dialogAccount,
        pwd: this.dialogPwd,
      });
      if (res.code === 200) {
        this.toggleHandler(false);
        this.dialogAccount = "";
        this.dialogPwd = "";

        // 新增成功後重取清單
        // TODO: need to handle exception
        const res = await getAgentsList();
        this.sourceAgentsList = res.data;
        alert("新增成功");
      } else {
        switch (res.code) {
          case 101: {
            alert("帳號已被註冊過了");
            break;
          }
          default: {
            alert("發生預期外的錯誤");
            break;
          }
        }
      }
    },
    onSubmitHandler() {
      let isValid = false;
      // 帳號規則: 英文開頭, 英數皆可 限6~20字元
      const accountRegex = new RegExp("^[A-Za-z][A-Za-z0-9]{5,19}");

      // 密碼規則: 6 位數以上，並且至少包含大寫字母、小寫字母、數字各一
      const pwdRegex = new RegExp(
        "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$"
      );

      isValid =
        accountRegex.test(this.dialogAccount) && pwdRegex.test(this.dialogPwd);

      if (isValid) {
        this.registerHandler();
      } else {
        this.isInvalid = true;
      }
    },
    changeHandler() {
      this.isInvalid = false;
    },
    onAddHandler() {
      this.isShowDialog = !this.isShowDialog;
    },
    toggleHandler(payload) {
      this.initDialogForm();
      this.isShowDialog = payload;
    },
    initDialogForm() {
      this.dialogAccount = "";
      this.dialogPwd = "";
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
  }
}
</style>
