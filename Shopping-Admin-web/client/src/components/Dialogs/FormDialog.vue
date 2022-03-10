<template>
    <div class="formDialog fixed inset-0 pointer-events-none">
        <!-- overlay -->
        <div
            class="overlay fixed top-0 left-0 w-full h-full bg-black1 pointer-events-auto"
            v-if="isShowDialog"
            @click="toggleHandler"
        ></div>

        <!-- modal -->
        <div
            class="dialog_inner flex justify-center items-center p-6 pointer-events-none fixed inset-0"
        >
            <div
                class="dialog_card relative max-w-[560px] rounded-[4px] bg-white p-4 pointer-events-auto"
                v-if="isShowDialog"
            >
                <div class="mb-5 text-2xl">{{ title }}</div>
                <div class="form">
                    <slot></slot>
                </div>
                <div class="bottom flex justify-center p-3">
                    <BtnSubmit label="確定" @submit="submitHandler" />
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import BtnSubmit from '../BtnSubmit.vue'

const props = defineProps({
    isShowDialog: {
        type: Boolean,
        required: true
    },
    title: {
        type: String,
        required: true
    }
})

const emits = defineEmits(['toggle', 'submit'])

const toggleHandler = () => {
    emits("toggle", !props.isShowDialog)
}

const submitHandler = () => {
    emits('submit')
}
</script>

<style lang="scss">
.formDialog {
    .dialog_inner {
        .dialog_card {
            max-height: calc(100vh - 48px);
            box-shadow: 0 2px 4px -1px #0003, 0 4px 5px #00000024,
                0 1px 10px #0000001f;
        }
    }
}
</style>
