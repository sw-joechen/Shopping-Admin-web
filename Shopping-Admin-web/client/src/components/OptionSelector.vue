<template>
    <div class="optionSelector">
        <div class="inline-block relative w-64">
            <select
                @change="onChangeHandler"
                v-model="value"
                class="block appearance-none w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 pr-8 rounded shadow leading-snug focus:outline-none focus:shadow-outline"
            >
                <option
                    :selected="opt.value === null"
                    :disabled="opt.value === null"
                    v-for="opt in options"
                    :value="opt.value"
                >{{ opt.label }}</option>
            </select>
            <div
                class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700"
            >
                <svg
                    class="fill-current h-4 w-4"
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 20 20"
                >
                    <path
                        d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z"
                    />
                </svg>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref } from "@vue/composition-api";

interface Option {
    label: string,
    value: boolean | null
}
const props = defineProps({
    options: {
        required: true,
        type: Array as () => Array<Option>,
        default: []
    }
});

const emits = defineEmits(["onChange"])

const value = $ref<boolean | null>(null)

const onChangeHandler = () => {
    emits('onChange', {
        value: value
    })
}
</script>

<style >
</style>
