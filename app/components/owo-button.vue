<script lang="ts" setup>
interface Props {
    text?: string;
}

const props = defineProps<Props>();
const emit = defineEmits(["click"]);
</script>

<template>
    <button
        class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        v-on:click="emit('click')"
    >
        <slot>{{ text }}</slot>
    </button>
</template>
