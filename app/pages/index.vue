<template>
    <span>CONTENT</span>
    <hr />
    <button v-on:click="onClick">Hello World</button>
</template>

<script setup>
function onClick() {
    alert("Hello!");
}
</script>
