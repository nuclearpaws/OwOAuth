<template>
    <div class="w-full p-4">
        <main
            role="main"
            class="w-full flex flex-col h-screen content-center justify-center"
        >
            <div class="w-full sm:w-1/2 lg:w-1/3 rounded-xl m-auto">
                <div class="rounded shadow px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
                    <slot />
                </div>
            </div>
        </main>
    </div>
</template>
