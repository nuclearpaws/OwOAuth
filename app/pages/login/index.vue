<template>
    <button v-on:click="navigateTo('')">Login</button>
</template>

<script setup>
definePageMeta({
    layout: "unauthorized",
});
</script>
