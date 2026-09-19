locals {
  name_prefix = "${var.project_name}-${var.environment}"

  default_tags = merge(var.tags, {
    Project     = var.project_name
    Environment = var.environment
    ManagedBy   = "terraform"
  })
}

resource "aws_db_subnet_group" "this" {
  count = var.create_networking_resources && length(var.subnet_ids) > 0 ? 1 : 0

  name       = "${local.name_prefix}-rds-subnets"
  subnet_ids = var.subnet_ids
  tags       = local.default_tags
}

resource "aws_elasticache_subnet_group" "this" {
  count = var.create_networking_resources && length(var.subnet_ids) > 0 ? 1 : 0

  name       = "${local.name_prefix}-cache-subnets"
  subnet_ids = var.subnet_ids
}

resource "aws_db_instance" "this" {
  identifier             = var.rds_instance_identifier
  db_name                = var.rds_db_name
  engine                 = "postgres"
  instance_class         = var.rds_instance_class
  allocated_storage      = var.rds_allocated_storage
  username               = var.rds_username
  password               = var.rds_password
  skip_final_snapshot    = true
  apply_immediately      = true
  db_subnet_group_name   = length(aws_db_subnet_group.this) > 0 ? aws_db_subnet_group.this[0].name : null
  vpc_security_group_ids = var.vpc_security_group_ids
  tags                   = local.default_tags
}

resource "aws_elasticache_replication_group" "this" {
  replication_group_id       = var.elasticache_replication_group_id
  description                = "${local.name_prefix} cache"
  engine                     = "redis"
  node_type                  = var.elasticache_node_type
  num_cache_clusters         = 1
  port                       = var.elasticache_port
  apply_immediately          = true
  subnet_group_name          = length(aws_elasticache_subnet_group.this) > 0 ? aws_elasticache_subnet_group.this[0].name : null
  security_group_ids         = var.vpc_security_group_ids
  automatic_failover_enabled = false
  tags                       = local.default_tags
}
