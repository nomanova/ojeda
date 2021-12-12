# NomaNova - Ojeda

Open-Source IT and Enterprise Asset Management

| Ojeda Api  | Ojeda Web |
| ------------- | ------------- |
| ![Build Api](https://github.com/nomanova/ojeda/actions/workflows/build_api.yml/badge.svg?branch=main) | ![Build Web](https://github.com/nomanova/ojeda/actions/workflows/build_web.yml/badge.svg?branch=main) |
| [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nomanova_ojeda_api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=nomanova_ojeda_api) | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nomanova_ojeda_web&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=nomanova_ojeda_web) |
| [Dockerhub](https://hub.docker.com/r/nomanova/ojeda-api) | [Dockerhub](https://hub.docker.com/r/nomanova/ojeda-web) |

## Try it

Clone the repository or just copy the "docker-compose.yml" file. Run the following docker commands:

```
$ docker compose pull
$ docker compose --project-name ojeda up
```

This will spin up 3 containers: a PostgreSQL database, the Ojeda API project and the Ojeda Web project.

Once all is up, go to http://localhost:8080 in your browser.