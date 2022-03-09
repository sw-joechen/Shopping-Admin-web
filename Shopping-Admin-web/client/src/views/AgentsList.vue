<template>
  <div class="agentsList p-5">
    <!-- input query -->
    <div class="inputGroup flex">
      <BtnSubmit label="增加" class="pr-2" @submit="onAddHandler" />
      <OptionSelector class="px-2" :options="options" @onChange="onOptionSelectorChange" />
    </div>

    <FormDialog
      :isShowDialog="isShowDialog"
      title="新增後台帳號"
      @toggle="toggleHandler"
      @submit="onSubmitHandler"
    >
      <form>
        <input
          v-model="dialogAccount"
          type="text"
          class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none"
          placeholder="請輸入帳號"
          required
          @keypress="changeHandler"
        />
        <p v-show="isInvalid" class="text-red-500 text-xs italic mb-3">帳號需以英文開頭,英數字皆可,介於6~20字元</p>

        <input
          v-model="dialogPwd"
          type="password"
          class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none"
          placeholder="請輸入密碼"
          required
          @keypress="changeHandler"
        />
        <p v-show="isInvalid" class="text-red-500 text-xs italic mb-3">密碼須包含大小寫字母及數字,超過6字元</p>
      </form>
    </FormDialog>

    <!-- table -->
    <!-- <div class="container">
      <table class="table">
        <thead></thead>
        <tbody>
          <tr v-for="(item, key) in sourceAgentsList" :key="key">
            <td>{{ item. }}</td>
            <td>{{ item.cash }}</td>
            <td>{{ item.icash }}</td>
          </tr>
        </tbody>
      </table>
    </div>-->
  </div>
</template>

<script setup lang="ts">
import { getAgentsList, registerAgent } from "@/APIs/Agents";
import { onMounted, reactive, ref, } from "@vue/composition-api";
import BtnSubmit from "@/components/BtnSubmit.vue";
import OptionSelector from "@/components/OptionSelector.vue"
import FormDialog from '@/components/Dialogs/FormDialog.vue'

const options = ref([
  {
    label: "啟用",
    value: true
  },
  {
    label: "禁用",
    value: false
  }
])

// const sourceAgentsList: Ref<IAgent[]> = ref([])


// queryData
const queryEnabled = ref(true)

onMounted(async () => {
  const res = await getAgentsList();
  // sourceAgentsList.value = res
});

const onOptionSelectorChange = (payload: {
  value: boolean
}) => {
  queryEnabled.value = payload.value
}

const registerHandler = async () => {
  const res = await registerAgent({
    account: dialogAccount,
    pwd: dialogPwd
  })
  if (res.code === 200) {
    toggleHandler(false)
    dialogAccount = ""
    dialogPwd = ""
  }
};

// dialogForm
const isShowDialog = ref(false)
let dialogAccount = $ref("")
let dialogPwd = $ref("")
let isInvalid = $ref(false)

const onSubmitHandler = () => {
  let isValid = false
  // 帳號規則: 英文開頭, 英數皆可 限6~20字元
  const accountRegex = new RegExp("^[A-Za-z][A-Za-z0-9]{5,19}");

  // 密碼規則: 6 位數以上，並且至少包含大寫字母、小寫字母、數字各一
  const pwdRegex = new RegExp("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$");

  isValid = accountRegex.test(dialogAccount) && pwdRegex.test(dialogPwd)

  if (isValid) {
    registerHandler()
  } else {
    isInvalid = true
  }
}

const changeHandler = () => {
  isInvalid = false
}

// 控制popup
const onAddHandler = () => {
  isShowDialog.value = !isShowDialog.value
}
const toggleHandler = (payload: boolean) => {
  isShowDialog.value = payload
}

</script>

<style lang="scss">
.agentsList {
  input {
    @apply mb-2;
  }
}
</style>
