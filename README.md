# Church Manager

## Dependencies 

	- RabbitMQ
	- PostgreSQL
	- Seq
	- Jaeger

## Running Locally

### Prerequisites

1. Run all the docker commands in the `docker containers` file in the root directory.

> docker compose -f docker-compose.dependencies.yml up

### Database

*Migrations*

1. In `Package manager console` change to `ChurchManager.Infrastructure.Persistence` project

2.
	- `Add-Migration InitialDbMigration -Context ChurchManagerDbContext -o Migrations -StartupProject ChurchManager.Api` 

3. 
	- `Update-Database -Context ChurchManagerDbContext -StartupProject ChurchManager.Api`

#### Troubleshooting

`Cannot delete because connections are not closed`
	
	- in the docker container execute:   `psql -U admin`
	- `SELECT * FROM pg_stat_activity WHERE pg_stat_activity.datname='churchmanager_db';`  shows open connections
	- `SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'churchmanager_db';`   close open connections
	- `DROP DATABASE churchmanager_db;`

## Environment settings

Most settings for production e.g. database connection will come from `AWS Parameter store`.

## Configurations

AWS Cognito IDP
> https://cognito-idp.us-east-1.amazonaws.com/us-east-1_i6pWJxu8q/.well-known/openid-configuration


### Manual Deploy

> set AWS_DEFAULT_PROFILE=personal

```
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 977844596384.dkr.ecr.us-east-1.amazonaws.com
```

1. Create ECR Respository

```
aws ecr create-repository --repository-name frontend-angular --image-scanning-configuration scanOnPush=true --image-tag-mutability IMMUTABLE  --region us-east-1 --profile personal
```

2. Build the docker image
3. Tag the docker image

```
docker tag church-manager-ui:latest 977844596384.dkr.ecr.us-east-1.amazonaws.com/frontend-angular:local
```

4. Push the image

`docker push 977844596384.dkr.ecr.us-east-1.amazonaws.com/frontend-angular:local`


