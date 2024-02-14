# Commands used - Docker:

To build a Docker container with an image of our application (run this in the same directory as the Dockerfile):
```bash
docker build -t ${USER}/itu-minitwit .
```

To run the newly created Docker-image:
```bash
docker run -d -p 8080:8080 ${USER}/itu-minitwit
```

To run using docker compose:
```bash
docker compose up
```

This spins up a new Docker-container with a random name, that runs in the background (-d) and on the port 8080 (-p). 


To view running Docker-containers:
```bash
docker ps -a
```

# Sources used today:

* https://stackoverflow.com/questions/62125794/docker-run-p-what-are-this-two-port-numbers-and-what-they-represents (used to figure out what -p does in docker run)
* https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish (dotnet publish help)
* https://stackify.com/a-start-to-finish-guide-to-docker-for-net/#dockerfile (dockerfile help)
* https://www.docker.com/blog/9-tips-for-containerizing-your-net-application/ (dockerfile help 2)
* https://stackoverflow.com/questions/26020/what-is-the-best-way-to-connect-and-use-a-sqlite-database-from-c-sharp (sqlite c# connection)
