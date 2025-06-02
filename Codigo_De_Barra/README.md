
# 📦 Código de Barras - API em .NET 8

Este projeto é uma API RESTful desenvolvida em .NET 8 que permite gerir produtos, clientes e pedidos. Ideal para sistemas de inventário, gestão de encomendas ou aplicações comerciais.

## 🚀 Funcionalidades

- ✅ Gestão de produtos (CRUD)
- ✅ Gestão de clientes (CRUD)
- ✅ Gestão de pedidos com associação de produtos e clientes
- ✅ Suporte a base de dados com Entity Framework Core
- ✅ Upload de imagens de produtos
- ✅ Endpoints RESTful

## 🛠️ Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- Entity Framework Core
- ASP.NET Core Web API
- SQL Server (ou outro via configuração)
- Swagger (para documentação da API)

## 📦 Estrutura do Projeto

```
/Controllers      -> Controladores da API (Produto, Cliente, Pedido)
/Models           -> Modelos de dados
/DTO              -> Objetos de Transferência de Dados
/Database         -> Contexto da Base de Dados e Migrações
/wwwroot/produtos -> Armazenamento de imagens dos produtos
```

## 🔧 Como Executar Localmente

1. Clone este repositório:
```bash
git clone https://github.com/seu-usuario/seu-repositorio.git
```

2. Acesse a pasta do projeto:
```bash
cd Codigo_De_Barra
```

3. Restaure os pacotes:
```bash
dotnet restore
```

4. Aplique as migrações à base de dados:
```bash
dotnet ef database update
```

5. Execute a aplicação:
```bash
dotnet run
```

6. Aceda ao Swagger:
```
https://localhost:5001/swagger
```

## 📄 Endpoints Principais

- `/api/produto` - Gestão de produtos
- `/api/cliente` - Gestão de clientes
- `/api/pedido`  - Gestão de pedidos

## ✍️ Contribuição

Contribuições são bem-vindas! Por favor, abra uma issue ou envie um pull request.

## 📜 Licença

Este projeto está licenciado sob a Licença MIT.
