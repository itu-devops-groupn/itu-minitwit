# Risk Identification

## A. Assets

- web application
- web api
- grafana dashboard
- prometheus instance
- database

## Threat Sources

- ddos attacks due to no request limits
- cookie manipulation
- root-user on docker images
- automated bot registrations to lack of human verification

## Risk Scenarios

- Attacker performs ddos attacks on our application bringing the application down.
- Attacker sends a lot of requests bringing our application to a halt due to many requests and potentially breaking the application or api.
- Attacker registers a user and changes the username-cookie to someone else which changes the users identity that could lead to unauthorized access to personal information.
- Attacker manages to break out from the docker container giving the attacker root access to the host machine potentially bringing down the application or breaking the infrastructure.

# B. Risk Analysis

## Likelihood

- Ddos attacks and sending a lot of requests are likely to happen should an attack occur. They are the easiest to perform as we have no limit on requests to our application.
- Cookie manipulation could be likely to happen should an attacker know the possibility of it, and if they know the format for the username.
- Breaking out from the container and exploiting root-user access is very unlikely to happen. It would require an attacker with a high level of skill and knowledge to perform such an attack.

## Impact

- Bringing down our application is troublesome and would require the service to be restarted, but due to us using Docker Swarm this would happen automatically if a container crashes. But, should the host-machine be brought down it would have higher impact because it would need a manual restart.
- One could potentially use another ones identity on our application with cookie manipulation which could mean that someone could send messages as someone else and harm someones reputation.
- Breaking out from the container could lead to us loosing ownership of our application and could mean that secrets could be exposed. One could also access our database if they broke our from a container due to us storing the connection string on the host machines in plain text.

## Risk Matrix

|               | Low Likelihood                  | Medium Likelihood               | High Likelihood       |
| ------------- | ------------------------------- | ------------------------------- | --------------------- |
| Low Impact    | Incorrect data entry            | Incorrect user input validation | Sending many requests |
| Medium Impact | Unauthorized access             | Cookie manipulation             | Bot spam              |
| High Impact   | Breaking out from the container | Data breach                     | DDOS attacks          |

## Reflection
