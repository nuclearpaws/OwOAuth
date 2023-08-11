<script lang="ts" setup>
interface Props {
    text: string;
    disabled?: boolean;
}

const props = defineProps<Props>();

const disabled = computed<boolean>(() => props.disabled ?? false);
</script>

<template>
    <input
        class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        type="submit"
        v-bind:value="text"
        v-bind:disabled="disabled"
    />
</template>
