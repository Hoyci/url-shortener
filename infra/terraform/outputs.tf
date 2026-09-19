output "postgres_connection_string" {
  description = "PostgreSQL connection string for the selected environment."
  sensitive   = true
  value = var.environment == "development"
    ? module.development[0].postgres_connection_string
    : module.production[0].postgres_connection_string
}

output "redis_connection_string" {
  description = "Redis connection string for the selected environment."
  value = var.environment == "development"
    ? module.development[0].redis_connection_string
    : module.production[0].redis_connection_string
}

output "rds_endpoint" {
  description = "RDS endpoint metadata."
  value = var.environment == "development"
    ? module.development[0].rds_endpoint
    : module.production[0].rds_endpoint
}

output "elasticache_endpoint" {
  description = "ElastiCache endpoint metadata."
  value = var.environment == "development"
    ? module.development[0].elasticache_endpoint
    : module.production[0].elasticache_endpoint
}
