<template>
  <nav
    class="nav bg-stone-500 text-white py-4 px-8 flex justify-between items-center max-w-7x1 mx-auto w-full"
  >
    <h1 class="text-lg font-bold">Portifólio</h1>
    <ul class="flex gap-6">
      <li>
        <router-link to="/" class="hover:no-underline">Início</router-link>
      </li>
      <li>
        <router-link to="/about" class="hover:no-underline">Sobre</router-link>
      </li>
      <li>
        <router-link to="/contact" class="hover:no-underline">Contato</router-link>
      </li>
    </ul>
  </nav>
</template>
