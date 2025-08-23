# Documentação do Projeto API\_Simulacao\_Hack

## 1. Pré-requisitos

Para rodar o projeto localmente ou via Docker, você precisará das seguintes ferramentas instaladas:

* **.NET 8 SDK** (para desenvolvimento e build)
  Baixe em: [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* **IDE**: Visual Studio 2022 (ou superior) / Visual Studio Code com extensão C#
* **Git** (opcional, caso vá clonar o repositório)
* **Docker** (para rodar a aplicação em container)
  Baixe em: [https://www.docker.com/get-started](https://www.docker.com/get-started)

> Observação: Caso seja necessário alterar as connection strings dos bancos de dados, configure as conexões no `appsettings.json`.

## 2. Estrutura do Projeto

O projeto possui a seguinte estrutura principal:

```
API_Simulacao_Hack/
│  API_Simulacao_Hack.csproj
│  Program.cs
│  appsettings.json
│  Dockerfile
├─ Controllers/
├─ DTO/
├─ Interfaces/
├─ Middleware/
├─ Migration/
├─ Models/
├─ Repositories/
├─ Services/
├─ Util/
├─ Validators/
├─ Wrappers/
└─ Tests/  (XUnit tests)
```

* **Controllers/**: Contém os endpoints da API.
* **DTO/**: Data Transfer Objects usados para requisições e respostas.
* **Interfaces/**: Contém os Interfaces da API.
* **Middleware/**: Contém o Middleware da telemetria da API.
* **Migrations/**: Contém as Migrations do Contexto de Simulação(SimulacaoContext) para criação do arquivo SQLite.
* **Models/**: Contém os modelos e contextos da API.
* **Repositories/**: Contém os Repositories da API.
* **Services/**: Contém os Services da API.
* **Util/**: Contém os Utilitários da API.
* **Validators/**: Contém o Validator do DTO de solicitação de simulação.
* **Wrappers/**: Contém o Wrapper para os envios paginados da API.
* **Tests/**: Testes unitários com XUnit.

## 3. Rodando o projeto localmente

### 3.1 Restaurar pacotes NuGet

No terminal, dentro da pasta do projeto (API_Simulacao_Hack):

```bash
dotnet restore API_Simulacao_Hack.csproj
```

### 3.2 Build do projeto

```bash
dotnet build API_Simulacao_Hack.csproj -c Release
```

### 3.3 Rodar a aplicação

```bash
dotnet run --project API_Simulacao_Hack.csproj
```

> A aplicação irá subir nas portas **7652** e **5652** por padrão.

### 3.4 Testes Unitários

Para rodar os testes XUnit:

No terminal, dentro da pasta (API_Simulacao_Hack.Test)

```bash
dotnet test
```

## 4. Rodando a aplicação com Docker

### 4.1 Build da imagem

Dentro da pasta do projeto (onde está o Dockerfile):

```bash
docker build -t api_simulacao_hack .
```

### 4.2 Rodar o container

Mapeando a porta 8080 do container para a porta 8080 do host:

```bash
docker run -d -p 8080:8080 --name api_simulacao_hack_container api_simulacao_hack
```

* **-d**: roda o container em background
* **-p 8080:8080**: mapeia a porta interna do container (8080) para a porta do host (8080)
* **--name**: nome do container

### 4.3 Conferir logs do container

```bash
docker logs -f api_simulacao_hack_container
```

### 4.4 Testando a API

Após o container estar rodando, você pode acessar a API pelo host em:

```
http://localhost:8080/
```

* Para os endpoints disponíveis, verifique os Controllers do projeto. Por exemplo, para o endpoint `/dadosTelemetria`, você poderá testar:

```
http://localhost:8080/dadosTelemetria
```

### 4.5 Parar e remover o container

```bash
docker stop api_simulacao_hack_container

docker rm api_simulacao_hack_container
```

### 4.6 Reconstruir imagem após alterações

```bash
docker build -t api_simulacao_hack .
docker run -d -p 8080:8080 --name api_simulacao_hack_container api_simulacao_hack
```

## 5. Observações importantes

* Certifique-se de que **nenhum outro serviço está usando a porta 8080** no host.
* O Kestrel dentro do container está configurado para escutar em **http\://+:8080**, garantindo acesso via host.
* Para alterações em `appsettings.json`, é necessário reconstruir a imagem se estiver usando Docker.
* Testes unitários cobrem cálculos PRICE e SAC, incluindo conferência de juros, amortização e saldo devedor final.
* Use `docker ps` para verificar se o container está ativo e mapeando corretamente a porta.
* Na listagem apresentada na rota `listaSimulacoes` o parâmetro `valorTotalParcelas` é baseado na simulação SAC
* Na listagem apresentada na rota `listaSimulacoesPorProdutoEDia` o parâmetro `valorTotalCredito` é baseado na simulação SAC


## 6. Resumo dos Comandos Principais

### Local

```bash
dotnet restore API_Simulacao_Hack.csproj
dotnet build API_Simulacao_Hack.csproj -c Release
dotnet run --project API_Simulacao_Hack.csproj
dotnet test
```

### Docker

```bash
docker build -t api_simulacao_hack .
docker run -d -p 8080:8080 --name api_simulacao_hack_container api_simulacao_hack
docker logs -f api_simulacao_hack_container
docker stop api_simulacao_hack_container
docker rm api_simulacao_hack_container
```

### Autor

Desenvolvido por: Gabriel Azevedo Sousa

*   **GitHub:** https://github.com/gazevedosousa
