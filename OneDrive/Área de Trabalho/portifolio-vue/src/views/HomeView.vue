<template>
  <div class="min-h-screen bg-white px-6 py-16 md:px-20 flex flex-col-reverse lg:flex-row items-center justify-between">
    <!-- Texto principal -->
    <div class="max-w-xl text-center lg:text-left">
      <p class="text-gray-600 mb-2">Olá pessoas,</p>
      <h1 class="text-4xl md:text-5xl font-extrabold text-gray-800 mb-4 leading-tight">
        EU SOU UM <br />
        PROGRAMADOR
      </h1>
      <p class="text-gray-500 text-lg mb-6">Seja bem-vindo ao meu portfólio website</p>

      <router-link to="/about" class="bg-gray-800 text-white font-bold py-3 px-6 rounded hover:bg-gray-900 transition">
        Saiba mais sobre mim
      </router-link>

      <div class="flex space-x-6 mt-10 justify-center lg:justify-start">
        <a href="https://github.com/MarcosPSJ" aria-label="GitHub" class="text-gray-700 hover:text-black text-2xl">
          <i class="fab fa-github"></i>
        </a>
        <a href="https://www.linkedin.com/in/marcos-paulo-sant-ana-j%C3%BAnior-090611208/" aria-label="LinkedIn" class="text-gray-700 hover:text-black text-2xl">
          <i class="fab fa-linkedin"></i>
        </a>
        <a href="https://www.instagram.com/markin_psj" aria-label="Instagram" class="text-gray-700 hover:text-black text-2xl">
          <i class="fab fa-instagram"></i>
        </a>
      </div>
    </div>

    <div class="w-full max-w-md mb-10 lg:mb-0">
      <img src="/src/assets/img/Developers-1024x651.webp" alt="Ilustração de programador" class="w-full h-auto" />
    </div>
  </div>
</template>

<script setup></script>

<style scoped>
@import url('https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css');
</style>
