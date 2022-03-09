<template>
    <div class="formDialog">
        <!-- overlay -->
        <div
            class="overlay fixed z-[9998] top-0 left-0 w-full h-full bg-black1"
            v-if="isShowDialog"
            @click="toggleHandler"
        ></div>

        <!-- modal -->
        <div
            class="modal relative max-w-[600px] z-[9999] my-0 mx-auto py-5 px-[30px] bg-white"
            v-if="isShowDialog"
        >
            <button class="close absolute top-[10px] right-[10px]" @click="toggleHandler">x</button>
            <div class="mb-5 text-2xl">{{ title }}</div>
            <div class="form">
                <slot></slot>
            </div>
            <div class="bottom">
                <BtnSubmit label="確定" @submit="submitHandler" />
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

<style >
</style>
