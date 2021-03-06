# Church Manager

## Running Locally

### Database

*Migrations*

1. In `Package manager console` change to `DbMigrations` project

2.
	- `Add-Migration InitialDbMigration -Context ChurchManagerDbContext -o Migrations` 

3. 
	- `Update-Database -Context ChurchManagerDbContext`

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
aws ecr create-repository --repository-name frontend/angular --image-scanning-configuration scanOnPush=true --image-tag-mutability IMMUTABLE  --region us-east-1 --profile personal
```

2. Build the docker image
3. Tag the docker image

```
docker tag church-manager-ui:latest 977844596384.dkr.ecr.us-east-1.amazonaws.com/frontend/angular:local
```

4. Push the image

`docker push 977844596384.dkr.ecr.us-east-1.amazonaws.com/frontend/angular:local`