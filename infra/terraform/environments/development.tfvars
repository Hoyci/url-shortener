environment = "development"

project_name = "urlshortener"
aws_region   = "us-east-1"

rds_instance_identifier          = "urlshortener-postgres"
rds_db_name                      = "urlshortener"
rds_username                     = "postgres"
rds_password                     = "postgres"
rds_instance_class               = "db.t3.micro"
rds_allocated_storage            = 20
elasticache_replication_group_id = "urlshortener-redis"
elasticache_node_type            = "cache.t3.micro"
elasticache_port                 = 6379

tags = {
  Stack = "floci"
}
