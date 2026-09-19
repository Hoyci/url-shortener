locals {
  create_networking_resources = var.environment == "production"
}

module "development" {
  count  = var.environment == "development" ? 1 : 0
  source = "./modules/data-stores"

  providers = {
    aws = aws.development
  }

  project_name                      = var.project_name
  environment                       = var.environment
  create_networking_resources       = false
  rds_instance_identifier           = var.rds_instance_identifier
  rds_db_name                       = var.rds_db_name
  rds_username                      = var.rds_username
  rds_password                      = var.rds_password
  rds_instance_class                = var.rds_instance_class
  rds_allocated_storage             = var.rds_allocated_storage
  elasticache_replication_group_id  = var.elasticache_replication_group_id
  elasticache_node_type             = var.elasticache_node_type
  elasticache_port                  = var.elasticache_port
  subnet_ids                        = []
  vpc_security_group_ids            = []
  tags                              = var.tags
}

module "production" {
  count  = var.environment == "production" ? 1 : 0
  source = "./modules/data-stores"

  providers = {
    aws = aws.production
  }

  project_name                      = var.project_name
  environment                       = var.environment
  create_networking_resources       = local.create_networking_resources
  rds_instance_identifier           = var.rds_instance_identifier
  rds_db_name                       = var.rds_db_name
  rds_username                      = var.rds_username
  rds_password                      = var.rds_password
  rds_instance_class                = var.rds_instance_class
  rds_allocated_storage             = var.rds_allocated_storage
  elasticache_replication_group_id  = var.elasticache_replication_group_id
  elasticache_node_type             = var.elasticache_node_type
  elasticache_port                  = var.elasticache_port
  subnet_ids                        = var.subnet_ids
  vpc_security_group_ids            = var.vpc_security_group_ids
  tags                              = var.tags
}
