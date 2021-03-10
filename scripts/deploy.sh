#!/bin/bash
echo Executing after success scripts on branch $TRAVIS_BRANCH

# install necessary dependency for ecs-deploy
sudo add-apt-repository ppa:eugenesan/ppa -y
sudo apt-get update
sudo apt-get install jq -y

# install AWS SDK
sudo pip install --upgrade pip
sudo pip install --upgrade --user awscli
export PATH=~/.local/bin:$PATH
aws --version

# install ecs-deploy
sudo wget https://raw.githubusercontent.com/silinternational/ecs-deploy/master/ecs-deploy 
sudo mv ecs-deploy /usr/bin/ecs-deploy
sudo chmod +x /usr/bin/ecs-deploy

# Push image to ECR
###################
# it expects aws access key and secret set in environmental vars
$(aws ecr get-login --no-include-email)

echo Building Docker image using branch $TRAVIS_BRANCH

# update latest version
IMAGE_TAG=${TRAVIS_BUILD_NUMBER:=latest}
IMAGE_URI="$ECR_REPOSITORY_URI:$IMAGE_TAG"

docker build -t $ECR_REPOSITORY_URI:latest .
docker tag $ECR_REPOSITORY_URI:latest $ECR_REPOSITORY_URI:$IMAGE_TAG

echo Pushing the Docker images...$IMAGE_URI
# push new version
docker push $ECR_REPOSITORY_URI:latest
docker push $ECR_REPOSITORY_URI:$IMAGE_TAG

case "$TRAVIS_BRANCH" in
  "master")
    echo -----------------------------------------------
    echo Deploying $SERVICE_NAME to $CLUSTER_NAME
    echo -----------------------------------------------
    ecs-deploy -c $CLUSTER_NAME -n prod-$SERVICE_NAME-service -i $IMAGE_URI
    ;;
esac

################################################
### Reference: https://blog.travis-ci.com/docker
################################################