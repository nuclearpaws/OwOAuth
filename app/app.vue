<script lang="ts" setup>
const colorMode = useColorMode();

const colorModeOptions = ref([
    { value: "system", text: "System" },
    { value: "light", text: "Light" },
    { value: "dark", text: "Dark" },
]);
</script>

<template>
    <div>
        <NuxtLayout>
            <NuxtPage />
        </NuxtLayout>
        <footer class="mt-auto">
            <owo-select
                id="colorMode"
                text="Choose Theme"
                v-model:value="colorMode.preference"
                v-bind:options="colorModeOptions"
            />
        </footer>
    </div>
</template>

<style lang="postcss">
body {
    @apply min-h-screen bg-white dark:bg-gray-800 dark:text-gray-200;
}
</style>
