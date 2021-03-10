#!/bin/bash
echo Executing after success scripts on branch $TRAVIS_BRANCH

# install necessary dependency for ecs-deploy
add-apt-repository ppa:eugenesan/ppa
apt-get update
apt-get install jq -y

# install AWS SDK
pip install --upgrade pip
sudo pip install --user awscli
export PATH=$PATH:$HOME/.local/bin

# install ecs-deploy
wget https://raw.githubusercontent.com/silinternational/ecs-deploy/master/ecs-deploy 
sudo mv ecs-deploy /usr/bin/ecs-deploy
sudo chmod +x /usr/bin/ecs-deploy

# Use this for AWS ECR
eval $(aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 977844596384.dkr.ecr.us-east-1.amazonaws.com)
# Use this for Docker Hub
#docker login --username $DOCKER_HUB_USER --password $DOCKER_HUB_PSW

echo Building Docker image using branch $TRAVIS_BRANCH

IMAGE_TAG=${TRAVIS_BUILD_NUMBER:=latest}
IMAGE_URI="$ECR_REPOSITORY_URI:$IMAGE_TAG"

docker build -t $ECR_REPOSITORY_URI:latest .
docker tag $ECR_REPOSITORY_URI:latest $ECR_REPOSITORY_URI:$IMAGE_TAG

echo Pushing the Docker images...$IMAGE_URI

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