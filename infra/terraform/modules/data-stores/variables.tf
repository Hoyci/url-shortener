variable "project_name" {
  type = string
}

variable "environment" {
  type = string
}

variable "create_networking_resources" {
  type = bool
}

variable "rds_instance_identifier" {
  type = string
}

variable "rds_db_name" {
  type = string
}

variable "rds_username" {
  type = string
}

variable "rds_password" {
  type      = string
  sensitive = true
}

variable "rds_instance_class" {
  type = string
}

variable "rds_allocated_storage" {
  type = number
}

variable "elasticache_replication_group_id" {
  type = string
}

variable "elasticache_node_type" {
  type = string
}

variable "elasticache_port" {
  type = number
}

variable "subnet_ids" {
  type = list(string)
}

variable "vpc_security_group_ids" {
  type = list(string)
}

variable "tags" {
  type = map(string)
}
