## How to Run

To run the program, follow these steps:

1. **Build Docker Images:**

   ```bash
   docker-compose build
   ```

2. **Start Containers:**

   ```bash
   docker-compose up --remove-orphans
   ```

   This command will start the necessary containers. The `--remove-orphans` option removes any containers for services not defined in the Compose file.

## Viewing Logs

The docker-compose.yml contains a service 'clidownload' that sends a curl request, and greps for a headline. It should return <strong>&lt;h1&gt;MiniTwit&lt;/h1&gt;</strong>
You can view the logs of the `clidownload` service using the following command:

```bash
docker-compose logs -f clidownload
```


## Additional Information

- The `docker-compose.yml` file contains the configuration for the Docker services used in this program.
- Make sure you have Docker and Docker Compose installed on your system before running the commands.
