#!/bin/bash

tag=$1

docker compose pull

for service in $(docker service ls --format "{{.Name}}")
do
    # Extract the image name from the service
    image=$(docker service inspect --format '{{ .Spec.TaskTemplate.ContainerSpec.Image }}' $service | cut -d ":" -f1)

    # Update the service with the latest tag
    docker service update --image $image:latest $service
done
