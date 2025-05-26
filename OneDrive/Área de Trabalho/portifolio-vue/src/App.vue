<script setup>
import Navbar from "./components/NavbarComponent.vue"
import Footer from "./components/FooterComponent.vue"
</script>

<template>
  <div class="flex flex-col min-h-screen">
    <Navbar />
    <main class="flex-grow">
      <router-view />
    </main>
    <Footer />
  </div>
</template>
