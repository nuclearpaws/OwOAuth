<template>
    <form v-on:submit.prevent="navigateTo('')">
        <h1>Login</h1>
        <hr class="my-5" />
        <owo-input
            v-model:value="username"
            type="text"
            id="username"
            text="Username"
        />
        <owo-input
            v-model:value="password"
            type="password"
            id="password"
            text="Password"
        />
        <hr class="my-5" />
        <owo-submit text="Login" v-bind:disabled="disabled" />
    </form>
</template>

<script lang="ts" setup>
definePageMeta({
    layout: "unauthorized",
});

const username = ref<string>("");
const password = ref<string>("");

const disabled = computed<boolean>(() => !(username.value && password.value));
</script>
