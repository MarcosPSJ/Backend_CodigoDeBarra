<template>
  <div class="min-h-screen bg-white flex items-center justify-center px-6 py-16">
    <div class="w-full max-w-3xl bg-gray-100 rounded-xl shadow-lg p-10">

      <h2 class="text-3xl font-bold text-gray-800 text-center mb-6">Entre em Contato</h2>
      <p class="text-gray-600 text-center mb-8">Preencha o formulário abaixo para me enviar uma mensagem.</p>

      <form @submit.prevent="handleSubmit" class="space-y-6">

        <div>
          <label class="block mb-1 font-medium text-gray-700" for="name">Nome</label>
          <input
            v-model="form.name"
            type="text"
            id="name"
            required
            placeholder="Seu nome"
            class="w-full border border-gray-300 rounded-md p-3 focus:outline-none focus:ring-2 focus:ring-gray-700"
          />
        </div>


        <div>
          <label class="block mb-1 font-medium text-gray-700" for="email">E-mail</label>
          <input
            v-model="form.email"
            type="email"
            id="email"
            required
            placeholder="seuemail@exemplo.com"
            class="w-full border border-gray-300 rounded-md p-3 focus:outline-none focus:ring-2 focus:ring-gray-700"
          />
        </div>

        <div>
          <label class="block mb-1 font-medium text-gray-700" for="message">Mensagem</label>
          <textarea
            v-model="form.message"
            id="message"
            rows="5"
            required
            placeholder="Escreva sua mensagem aqui..."
            class="w-full border border-gray-300 rounded-md p-3 focus:outline-none focus:ring-2 focus:ring-gray-700"
          ></textarea>
        </div>


        <div class="text-center">
          <button
            type="submit"
            class="bg-gray-800 text-white font-semibold px-6 py-3 rounded-md hover:bg-gray-900 transition"
          >
            Enviar Mensagem
          </button>
        </div>
      </form>

      <p v-if="successMessage" class="mt-6 text-green-600 text-center font-medium">
        {{ successMessage }}
      </p>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue'

const form = reactive({
  name: '',
  email: '',
  message: ''
})

const successMessage = ref('')

function handleSubmit() {
  console.log('Formulário enviado:', form)
  successMessage.value = 'Mensagem enviada com sucesso!'
  

  form.name = ''
  form.email = ''
  form.message = ''
}
</script>
