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
      @submit="registerHandler"
    >
      <input
        v-model="dialogAccount"
        type="text"
        class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none"
        placeholder="請輸入帳號"
        required
      />

      <input
        v-model="dialogPwd"
        type="password"
        class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none"
        placeholder="請輸入密碼"
        required
      />
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

const isShowDialog = ref(false)
// const sourceAgentsList: Ref<IAgent[]> = ref([])

// dialogForm
let dialogAccount = $ref("")
let dialogPwd = $ref("")

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
