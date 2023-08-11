<script lang="ts" setup>
interface Props {
    id: string;
    text: string;
    options: Array<Option>;
    value?: string | number | symbol;
}

interface Option {
    value: string | number | symbol;
    text: string;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:value"]);

// overrides access to prop "value":
const value = computed({
    get: () => props.value,
    set: (value) => emit("update:value", value),
});
</script>

<template>
    <label
        v-bind:for="id"
        class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
        >{{ text }}</label
    >
    <select
        v-bind:id="id"
        class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
        v-model="value"
    >
        <option
            v-for="option in options"
            v-bind:key="option.value"
            v-bind:value="option.value"
        >
            {{ option.text }}
        </option>
    </select>
</template>
