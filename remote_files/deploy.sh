#!/bin/bash

docker compose pull

service_count=$(docker service ls --format "{{.Name}}" | wc -l)

# If no services are running, deploy the stack
if [ "$service_count" -eq 0 ]; then
    docker stack deploy -c docker-compose.yml group_n
else
    for service in $(docker service ls --format "{{.Name}}"); do
        # Extract the image name from the service
        image=$(docker service inspect --format '{{ .Spec.TaskTemplate.ContainerSpec.Image }}' $service | cut -d ":" -f1)

        # Update the service with the latest tag
        docker service update --image $image:latest $service
    done
fi