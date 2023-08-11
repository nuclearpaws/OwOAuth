<template>
    <div>
        <span>Authorized...</span>
        <hr />
        <ul>
            <li>
                <button v-on:click="navigateTo('/')">Home</button>
            </li>
            <li>
                <button v-on:click="navigateTo('kek')">Kek</button>
            </li>
            <li>
                <button v-on:click="navigateTo('login')">Logout</button>
            </li>
        </ul>
        <hr />
        <slot />
    </div>
</template>

<script setup></script>
