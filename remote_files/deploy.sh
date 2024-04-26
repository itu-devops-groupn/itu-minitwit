#!/bin/bash

tag=$1

docker pull dsmillard/minitwit-web:$tag
docekr pull dsmillard/minitwit-api:$tag

docker service update --image dsmillard/minitwit-web:$tag vagrant_minitwit-web-image
docker service update --image dsmillard/minitwit-api:$tag vagrant_minitwit-api-image

docker service update --image grafana/grafana vagrant_grafana
docker service update --image prom/prometheus vagrant_prometheus
