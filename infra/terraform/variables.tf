variable "environment" {
  description = "Environment to provision. Use development for Floci and production for AWS."
  type        = string

  validation {
    condition     = contains(["development", "production"], var.environment)
    error_message = "environment must be development or production."
  }
}

variable "project_name" {
  description = "Project name used in resource names and tags."
  type        = string
  default     = "urlshortener"
}

variable "aws_region" {
  description = "AWS region used in both Floci and production."
  type        = string
  default     = "us-east-1"
}

variable "aws_profile" {
  description = "Optional AWS profile for production applies."
  type        = string
  default     = null
  nullable    = true
}

variable "floci_endpoint" {
  description = "Floci endpoint used by Terraform in development."
  type        = string
  default     = "http://localhost:4566"
}

variable "rds_instance_identifier" {
  description = "RDS instance identifier."
  type        = string
}

variable "rds_db_name" {
  description = "PostgreSQL database name."
  type        = string
}

variable "rds_username" {
  description = "RDS master username."
  type        = string
}

variable "rds_password" {
  description = "RDS master password."
  type        = string
  sensitive   = true
}

variable "rds_instance_class" {
  description = "RDS instance class."
  type        = string
  default     = "db.t3.micro"
}

variable "rds_allocated_storage" {
  description = "RDS allocated storage in GB."
  type        = number
  default     = 20
}

variable "elasticache_replication_group_id" {
  description = "ElastiCache replication group identifier."
  type        = string
}

variable "elasticache_node_type" {
  description = "ElastiCache node type."
  type        = string
  default     = "cache.t3.micro"
}

variable "elasticache_port" {
  description = "ElastiCache port."
  type        = number
  default     = 6379
}

variable "subnet_ids" {
  description = "Subnet IDs used in production subnet groups."
  type        = list(string)
  default     = []
}

variable "vpc_security_group_ids" {
  description = "Security group IDs attached in production."
  type        = list(string)
  default     = []
}

variable "tags" {
  description = "Additional tags applied to resources."
  type        = map(string)
  default     = {}
}
