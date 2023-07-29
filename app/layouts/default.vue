<template>
    <n-space vertical size="large">
        <n-layout>
            <n-layout-header bordered content-style="padding: 24px;">
                <span>HEADER</span>
                <n-divider vertical />
                <n-button quaternary v-on:click="navigateTo('login')"
                    >Logout</n-button
                >
            </n-layout-header>
            <n-layout-content bordered>
                <n-layout has-sider>
                    <n-layout-sider
                        bordered
                        collapse-mode="width"
                        v-bind:collapsed-width="64"
                        v-bind:width="240"
                        v-bind:collapsed="collapsed"
                        show-trigger
                        v-on:collapse="collapsed = true"
                        v-on:expand="collapsed = false"
                    >
                        <sidenav v-model:activeKey="route.name" />
                    </n-layout-sider>
                    <n-layout-content bordered content-style="padding: 24px;">
                        <slot />
                    </n-layout-content>
                </n-layout>
            </n-layout-content>
            <n-layout-footer bordered>FOOTER</n-layout-footer>
        </n-layout>
    </n-space>
</template>

<script setup>
const route = useRoute();
const collapsed = ref(false);
</script>
