# Terraform de infraestrutura

Esta pasta provisiona os datastores do projeto em dois modos:

- `development`: usa o provider AWS apontando para o `floci`
- `production`: usa a AWS real

## Recursos criados

- `RDS PostgreSQL`
- `ElastiCache Redis`

## Pré-requisitos

- `terraform`
- Docker, para o fluxo `development`
- Credenciais AWS válidas, para o fluxo `production`

## Development com Floci

1. Suba o Floci:

```bash
docker compose -f docker-compose.floci.yml up -d
```

2. Inicialize o Terraform:

```bash
terraform -chdir=infra/terraform init
```

3. Use um workspace separado para development:

```bash
terraform -chdir=infra/terraform workspace new development
```

4. Aplique a infraestrutura local:

```bash
terraform -chdir=infra/terraform apply -var-file=environments/development.tfvars
```

5. Use a API com o ambiente `Floci`:

```bash
$env:ASPNETCORE_ENVIRONMENT="Floci"
dotnet run --project Api
```

O `appsettings.Floci.json` já está alinhado com as portas expostas pelo `floci`:

- PostgreSQL: `localhost:7001`
- Redis: `localhost:6379`

## Production na AWS

1. Copie `environments/production.tfvars.example` para `environments/production.tfvars`
2. Preencha `subnet_ids`, `vpc_security_group_ids`, senha e demais valores reais
3. Selecione um workspace separado:

```bash
terraform -chdir=infra/terraform workspace new production
```

4. Aplique:

```bash
terraform -chdir=infra/terraform apply -var-file=environments/production.tfvars
```

## Outputs

Após o `apply`, os principais outputs são:

- `postgres_connection_string`
- `redis_connection_string`
- `rds_endpoint`
- `elasticache_endpoint`

Para visualizar:

```bash
terraform -chdir=infra/terraform output
```

## Observação importante

Não reutilize o mesmo state/workspace para `development` e `production`. Use workspaces separados ou backends separados por ambiente.
