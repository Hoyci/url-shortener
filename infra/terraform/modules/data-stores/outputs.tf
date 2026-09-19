output "postgres_connection_string" {
  sensitive = true
  value     = "Host=${aws_db_instance.this.address};Port=${aws_db_instance.this.port};Database=${var.rds_db_name};Username=${var.rds_username};Password=${var.rds_password}"
}

output "redis_connection_string" {
  value = "${aws_elasticache_replication_group.this.primary_endpoint_address}:${aws_elasticache_replication_group.this.port}"
}

output "rds_endpoint" {
  value = {
    address = aws_db_instance.this.address
    port    = aws_db_instance.this.port
  }
}

output "elasticache_endpoint" {
  value = {
    address = aws_elasticache_replication_group.this.primary_endpoint_address
    port    = aws_elasticache_replication_group.this.port
  }
}
