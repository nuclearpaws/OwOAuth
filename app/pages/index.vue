<template>
    <span>CONTENT</span>
    <hr />
    <owo-input
        v-model:value="message"
        type="text"
        id="message"
        text="Message"
    />
    <owo-button v-on:click="onClick" text="Broadcast" />
</template>

<script lang="ts" setup>
const message = ref<string>("");
const password = ref<string>("");

function onClick() {
    alert(message.value);
    message.value = "";
}
</script>
