# NomaNova - Ojeda

Open-Source IT and Enterprise Asset Management

| Ojeda Api  | Ojeda Web |
| ------------- | ------------- |
| [![Build Status](https://dev.azure.com/nomanova/ojeda/_apis/build/status/Ojeda%20Api?branchName=main)](https://dev.azure.com/nomanova/ojeda/_build/latest?definitionId=1&branchName=main) | [![Build Status](https://dev.azure.com/nomanova/ojeda/_apis/build/status/Ojeda%20Web?branchName=main)](https://dev.azure.com/nomanova/ojeda/_build/latest?definitionId=2&branchName=main)  |
| [Dockerhub](https://hub.docker.com/r/nomanova/ojeda-api) | [Dockerhub](https://hub.docker.com/r/nomanova/ojeda-web) |

## Try it

Clone the repository or just copy the "docker-compose.yml" file. Run the following docker commands:

```
$ docker compose pull
$ docker compose --project-name ojeda up
```

This will spin up 3 containers: a PostgreSQL database, the Ojeda API project and the Ojeda Web project.

Once all is up, go to http://localhost:8080 in your browser.