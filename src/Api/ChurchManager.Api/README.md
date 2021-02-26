# Church Manager

## Running Locally

### Database

*Migrations*

1. In `Package manager console` change to `DbMigrations` project

2.
	- `Add-Migration InitialDbMigration -Context ChurchManagerDbContext -o Migrations` 

3. 
	- `Update-Database -Context ChurchManagerDbContext`



	## Configurations

	AWS Cognito IDP
	> https://cognito-idp.us-east-1.amazonaws.com/us-east-1_i6pWJxu8q/.well-known/openid-configuration

