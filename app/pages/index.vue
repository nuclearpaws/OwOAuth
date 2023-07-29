<template>
    <span>CONTENT</span>
    <hr />
    <n-button v-on:click="onClick">Hello World</n-button>
</template>

<script setup>
function onClick() {
    alert("Hello!");
}
</script>
