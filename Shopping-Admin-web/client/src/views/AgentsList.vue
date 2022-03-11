<template>
  <div class="agentsList p-5">
    <!-- input query -->
    <div class="inputGroup flex">
      <BtnSubmit label="新增" class="pr-2" @submit="onAddHandler" />
      <OptionSelector class="px-2" :options="options" @onChange="onEnabledOptionChangeHandler" />
      <BtnSubmit label="搜尋" class="pr-2" @submit="onSearchHandler" />
    </div>

    <!-- 新增帳號dialog -->
    <FormDialog
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
        <p v-show="isInvalid" class="text-red-500 text-xs italic">帳號需以英文開頭,英數字皆可,介於6~20字元</p>

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
        <p v-show="isInvalid" class="text-red-500 text-xs italic">密碼須包含大小寫字母及數字,超過6字元</p>
      </form>
    </FormDialog>

    <!-- table -->
    <div class="tableContainer pt-5">
      <CustomTable :data="agentsListComputed" />
    </div>
  </div>
</template>

<script setup lang="ts">
// @ts-ignore
import { getAgentsList, IAgent, registerAgent } from "@/APIs/Agents";
import { computed, onMounted, reactive, ref, } from "@vue/composition-api";
import BtnSubmit from "@/components/BtnSubmit.vue";
import OptionSelector from "@/components/OptionSelector.vue"
import FormDialog from '@/components/Dialogs/FormDialog.vue'
import CustomTable from '@/components/Table/Table.vue'

enum EOptions {
  all = 0,
  enabled = 1,
  disabled = 2
}
const options = ref([
  {
    label: "全部",
    value: EOptions.all
  },
  {
    label: "啟用",
    value: EOptions.enabled
  },
  {
    label: "禁用",
    value: EOptions.disabled
  }
])

let sourceAgentsList = $ref<IAgent[]>([])

// queryData
let queryData = reactive<{ enabled: number }>({
  enabled: 0
})
let queryEnabled = $ref<number>(0)
let isInitFinish = $ref(false)

const agentsListComputed = computed(() => {
  if (sourceAgentsList && sourceAgentsList.length) {
    if (queryData.enabled === EOptions.all) {
      return sourceAgentsList
    }
    return sourceAgentsList.filter(item => {
      if (queryData.enabled === EOptions.enabled) {
        return item.enabled
      } else if (queryData.enabled === EOptions.disabled) {
        return item.enabled === 0
      }
    })
  }
  return []
})

onMounted(async () => {
  isInitFinish = false
  const res = await getAgentsList();
  sourceAgentsList = res.data
  isInitFinish = true
});

const onSearchHandler = async () => {
  queryData.enabled = queryEnabled
  console.log('queryData.enabled=>', queryData.enabled);

  sourceAgentsList = []
  const res = await getAgentsList();
  sourceAgentsList = res.data
}

const onEnabledOptionChangeHandler = (payload: { value: number }) => {
  queryEnabled = payload.value
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

    // 新增成功後重取清單
    isInitFinish = false
    // TODO: need to handle exception
    const res = await getAgentsList();
    sourceAgentsList = res.data
    isInitFinish = true
    alert('新增成功')
  } else {
    switch (res.code) {
      case 101: {
        alert('帳號已被註冊過了')
        break
      }
      default: {
        alert('發生預期外的錯誤')
        break
      }
    }
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
  initDialogForm()
  isShowDialog.value = payload
}
const initDialogForm = () => {
  dialogAccount = ""
  dialogPwd = ""
  isInvalid = false
}

</script>

<style lang="scss">
.agentsList {
  .inputGroup {
    @apply p-3 pl-0;
  }
}
</style>
