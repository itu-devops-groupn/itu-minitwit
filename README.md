
## Download dev database to /tmp

   ```bash
   curl -L -o /tmp/minitwit.db https://raw.github.com/itu-devops-groupn/itu-minitwit/c212879cd1fbdeaa6597a06ccb28068a1291e8c6/minitwit.db 
   ```

## How to run in docker

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

## Setup test suite

1. Create empty test db
   ```bash
   touch /tmp/test-minitwit.db &&
   sqlite3 /tmp/test-minitwit.db < schema.sql
   ```

2. Start your application in docker

3. Make sure you have downloaded 'minitwit_simulator.py' and 'minitwit_scenario.csv'.
   Run following commands from repo root directory (itu-minitwit):
   ```bash
   curl -L -o test/minitwit_simulator.py https://raw.githubusercontent.com/itu-devops/lecture_notes/master/sessions/session_03/API_Spec/minitwit_simulator.py
   ```
   ```bash
   curl -L -o test/minitwit_scenario.csv https://raw.githubusercontent.com/itu-devops/lecture_notes/master/sessions/session_03/API_Spec/minitwit_scenario.csv  
   ```

4. Test
   ```bash
   cd test && 
   python3 minitwit_simulator.py "http://localhost:5050"
   ```
   Only errors are displayed in the output

5. Verify that /tmp/test-minitwit.db is now filled up with data
