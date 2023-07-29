<template>
    <n-button v-on:click="navigateTo('')">Login</n-button>
</template>

<script setup>
definePageMeta({
    layout: "unauthorized",
});
</script>
