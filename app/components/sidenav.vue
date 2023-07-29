<template>
    <n-menu
        v-model:value="props.activeKey"
        v-bind:collapsed="collapsed"
        v-bind:collapsed-width="64"
        v-bind:collapsed-icon-size="22"
        v-bind:options="menuOptions"
    />
</template>

<script setup>
const props = defineProps(["activeKey"]);
const menuOptions = ref([
    {
        label: () =>
            h("a", {
                href: "/",
                innerHTML: "Home",
            }),
        key: "index",
    },
    {
        label: () =>
            h("a", {
                href: "/kek",
                innerHTML: "Kek",
            }),
        key: "kek",
    },
]);
</script>
