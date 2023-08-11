<script setup>
const props = defineProps(["activeKey"]);
const navItems = ref([
    {
        name: "Home",
        key: "index",
        path: "/",
    },
    {
        name: "Kek",
        key: "kek",
        path: "/kek",
    },
    {
        name: "Logout",
        key: "logout",
        path: "/login",
    },
]);
</script>

<template>
    <ul class="flex sm:flex-col overflow-hidden content-center justify-between">
        <li v-for="navItem in navItems" v-bind:key="navItem.name">
            <owo-button
                v-bind:class="[navItem.key == activeKey ? 'text-white' : '']"
                v-on:click="navigateTo(navItem.path)"
            >
                {{ navItem.name }}
            </owo-button>
        </li>
    </ul>
</template>
