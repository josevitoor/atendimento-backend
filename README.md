# Projeto Atendimento Backend

## Instalação e Configuração

### 1. Instalar Dependências do Projeto

Certifique-se de ter o .NET SDK instalado em sua máquina. Em seguida, instale as dependências do projeto executando os seguintes comandos:

```bash
dotnet restore
dotnet build
```

### 2. Subir o container do Postgres

Certifique-se de ter o Docker instalado em sua máquina. Na raiz do projeto, execute o seguinte comando para subir os containers:

```bash
docker-compose up -d
```

### 3. Atualizar o Banco de Dados

Após os containers estarem em execução, atualize a migração do banco de dados com o seguinte comando:

```bash
dotnet ef database update
```
