# Orleans Playground

## Devcontainer

Running devcontainer starts background services

* [Grafana](https://grafana.com/blog/2025/07/08/observability-in-under-5-seconds-reflecting-on-a-year-of-grafana/otel-lgtm/) - https://localhost:3000
* [Localstack](https://www.localstack.cloud/) 

## Running

Run Silo

```sh
dotnet run Silo/Silo.csproj
```

Use Client

```
dotnet run Client/Client.csproj
```

type `help` to list client commands
