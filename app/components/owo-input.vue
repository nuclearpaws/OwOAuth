<script lang="ts" setup>
interface Props {
    value: string;
    id: string;
    text: string;
    type: "text" | "password";
    required?: boolean;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:value"]);

const value = computed({
    get: () => props.value,
    set: (value) => emit("update:value", value),
});
</script>

<template>
    <div>
        <label
            v-bind:for="id"
            class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
            >{{ text }}</label
        >
        <input
            v-model="value"
            v-bind:type="type"
            v-bind:id="id"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
            v-bind:placeholder="text"
            v-bind:required="required ?? false"
        />
    </div>
</template>
