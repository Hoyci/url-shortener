# UrlShortener

API em `.NET` para encurtamento e redirecionamento de URLs, com persistencia em `PostgreSQL` e cache/contador em `Redis`.

## Visao geral

O projeto expoe dois fluxos principais:

- criar uma URL curta a partir de uma URL original
- redirecionar um codigo curto para a URL original

Armazenamento usado hoje:

- `PostgreSQL`: persistencia das URLs
- `Redis`: contador de IDs e cache de redirecionamento

## Como funciona

1. Ao criar uma URL, a API verifica se a URL original ja existe.
2. Se nao existir, gera um codigo curto a partir de um contador no `Redis`.
3. A URL e salva no `PostgreSQL`.
4. Ao redirecionar, a API primeiro consulta o `Redis`.
5. Se nao encontrar no cache, busca no `PostgreSQL` e popula o cache.

## Stack

- `.NET 10`
- `ASP.NET Core Web API`
- `PostgreSQL`
- `Redis`
- `Swagger`
- `Docker Compose`
- `Floci`
- `Terraform`

## Estrutura

```text
Api/             API HTTP, DI, Swagger e bootstrap da aplicacao
BuildingBlocks/  conexao com banco e migracoes
CreateURL/       criacao e validacao de URLs curtas
RedirectURL/     redirecionamento e cache
IDGenerator/     geracao de codigos curtos via contador + permutacao
infra/terraform/ infraestrutura para development e production
```

## Endpoints

Base route: `api/v1/url`

### Criar URL curta

`POST /api/v1/url`

Exemplo de body:

```json
{
  "longURL": "https://example.com/artigo-importante",
  "customAlias": "meu-link",
  "expirationDate": "2026-12-31T23:59:59Z"
}
```

Resposta de sucesso:

```json
"abc123"
```

Observacoes:

- se a `longURL` ja existir, a API retorna o codigo existente
- `expirationDate` nao pode ser uma data passada
- `customAlias` nao pode ter mais de `10` caracteres

### Redirecionar URL

`GET /api/v1/url/{code}`

Resposta:

- `302 Found` com redirecionamento para a URL original
- `404 Not Found` quando o codigo nao existe

## Swagger

Ao subir a API, o Swagger fica disponivel em:

- `http://localhost:5112/swagger`
- ou `https://localhost:7139/swagger`

As portas acima vem do arquivo `Api/Properties/launchSettings.json`.

## Configuracao da aplicacao

Arquivo padrao:

- `Api/appsettings.json`

Conexoes padrao:

- `Postgres`: `Host=localhost;Port=5432;Database=urlshortener;Username=postgres;Password=postgres`
- `Redis`: `localhost:6379`

Configuracao para usar com `floci`:

- `Api/appsettings.json`

Conexoes no modo `Floci`:

- `Postgres`: `Host=localhost;Port=7001;Database=urlshortener;Username=postgres;Password=postgres`
- `Redis`: `localhost:6379`

## Rodando localmente com Docker Compose

Esse modo sobe `PostgreSQL` e `Redis` diretamente, sem emulacao AWS.

### Pre-requisitos

- `Docker`
- `.NET SDK` compativel com `net10.0`

## Rodando com Floci + Terraform

Esse modo e util quando voce quer provisionar `RDS` e `ElastiCache` localmente com a mesma interface da AWS.

### 1. Suba o Floci

```bash
docker compose -f docker-compose.yml up -d
```

### 2. Provisione a infraestrutura local

```bash
terraform -chdir=infra/terraform init
terraform -chdir=infra/terraform workspace new development
terraform -chdir=infra/terraform apply -var-file=environments/development.tfvars
```

### 3. Rode a API usando o ambiente Floci

PowerShell:

```powershell
$env:ASPNETCORE_ENVIRONMENT="Floci"
dotnet run --project Api
```

## Terraform por ambiente

A pasta `infra/terraform` suporta dois cenarios:

- `development`: usa provider AWS apontando para `http://localhost:4566` no `floci`
- `production`: usa AWS real

Arquivos principais:

- `infra/terraform/environments/development.tfvars`
- `infra/terraform/environments/production.example`

Para `production`:

1. copie `production.tfvars.example` para `production.tfvars`
2. preencha `subnet_ids`, `vpc_security_group_ids`, credenciais e valores reais
3. aplique com um workspace separado

Exemplo:

```bash
terraform -chdir=infra/terraform init
terraform -chdir=infra/terraform workspace new production
terraform -chdir=infra/terraform apply -var-file=environments/production.tfvars
```

Mais detalhes em `infra/terraform/README.md`.

## Banco de dados

A migracao atual cria a tabela abaixo:

```sql
CREATE TABLE IF NOT EXISTS urls (
    id UUID PRIMARY KEY,
    long_url TEXT NOT NULL,
    code TEXT NOT NULL UNIQUE,
    custom_alias TEXT,
    created_at TIMESTAMPTZ NOT NULL,
    expiration_date TIMESTAMPTZ
);
```

## Detalhes de implementacao

- o contador de IDs fica no `Redis` na chave `id_counter`
- o codigo curto e gerado em base `62`
- a API aplica migracoes na inicializacao
- o cache de redirecionamento e preenchido sob demanda

## Comandos uteis

Subir dependencias locais:

```bash
docker compose up -d
```

Subir Floci:

```bash
docker compose -f docker-compose.yml up -d
```

Rodar a API:

```bash
dotnet run --project Api
```

## Proximos pontos naturais

- adicionar testes automatizados para os fluxos de criacao e redirecionamento
- expor resposta mais estruturada no `POST /api/v1/url`
- documentar ou concluir o comportamento funcional de `customAlias`
- adicionar observabilidade e metricas de redirecionamento
