provider "aws" {
  alias      = "development"
  region     = var.aws_region
  access_key = "test"
  secret_key = "test"

  skip_credentials_validation = true
  skip_metadata_api_check     = true
  skip_region_validation      = true
  skip_requesting_account_id  = true

  endpoints {
    elasticache = var.floci_endpoint
    rds         = var.floci_endpoint
  }
}

provider "aws" {
  alias   = "production"
  region  = var.aws_region
  profile = var.aws_profile
}
