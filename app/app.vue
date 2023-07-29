<template>
    <n-config-provider v-bind:theme="darkTheme">
        <NuxtLayout>
            <NuxtPage />
        </NuxtLayout>
    </n-config-provider>
</template>

<script setup>
import { darkTheme } from "naive-ui";
</script>
