# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

This is an Orleans playground project demonstrating Microsoft Orleans virtual actor model. It includes:
- **Silo**: Orleans server hosting grains (actors)
- **Client**: CLI application using CliFx to interact with grains
- **Contracts**: Shared grain interfaces and DTOs

## Commands

### Build
```sh
dotnet build Demo.sln
```

### Run Silo
```sh
dotnet run --project Silo/Silo.csproj
```

### Run Client
```sh
dotnet run --project Client/Client.csproj
```
Type `help` in the client to list available commands.

### Restore Dependencies
```sh
dotnet restore Demo.sln
```

## Architecture

### Project Structure

**Contracts**: Grain interfaces (must inherit from `IGrainWithStringKey` or similar Orleans interfaces) and shared DTOs. Examples:
- `IHelloWorld`: Basic grain with state
- `IConsumer`: Stream consumer grain
- `IThrowingGrain`: Exception handling examples

**Silo/Grains**: Grain implementations that implement interfaces from Contracts.
- `HelloWorld`: Simple grain maintaining state with `Grain<HelloWorldState>` pattern
- `Consumer`: Stream consumer with `[ImplicitStreamSubscription]` for multiple namespaces
- State examples: Grains demonstrating different state storage patterns (class, struct, record, private fields)

**Client/Commands**: CliFx command implementations. Each command corresponds to a specific grain interaction. Registered in `CliFxInitializer.cs`.

### Orleans Configuration

Orleans is configured in `OrleansInitializer.cs` files:

**Silo** (Silo/OrleansInitializer.cs):
- Localhost clustering (port 11111 silo, 30001 gateway)
- Dashboard enabled at localhost:8080
- DynamoDB persistence for grain storage (via Localstack)
- SQS streams with `ImplicitOnly` pub/sub
- In-memory grain storage as default

**Client** (Client/OrleansInitializer.cs):
- Connects to localhost gateway (port 30001)
- SQS streams support for publishing events

Both use DynamoDB and SQS via Localstack (configured in appsettings.json with Service="us-east-1").

### Configuration Templates

The Silo uses a custom configuration template system (`ConfigurationTemplates.cs`) that replaces placeholders in appsettings.json using `{{key:path}}` syntax.

Example in appsettings.json:
```json
"My-AWS": {
    "Region": "-{{AWS_REGION}}-{{ClusterOptions:ServiceId}}-"
}
```

This resolves `{{AWS_REGION}}` and `{{ClusterOptions:ServiceId}}` from configuration values at runtime.

### Streams and Implicit Subscriptions

The codebase demonstrates Orleans streams with implicit subscriptions. The `Consumer` grain subscribes to multiple stream namespaces using:

```csharp
[ImplicitStreamSubscription(StreamConstants.Namespace1)]
[ImplicitStreamSubscription(StreamConstants.Namespace2)]
```

When events are published to these namespaces, the Consumer grain is automatically activated and receives events. The grain implements `IStreamSubscriptionObserver` and type-specific observers (`IAsyncObserver<SomeEvent1>`, `IAsyncObserver<SomeEvent2>`).

Stream configuration uses SQS (via Localstack) with 1 partition and `ImplicitOnly` pub/sub mode.

### Grain State Management

The codebase includes examples of different state storage patterns:
- State as class (reference type)
- State as struct (value type)
- State as record
- State with private fields

All use `Grain<TState>` base class and persist to configured storage (DynamoDB or in-memory).

### Observability

OpenTelemetry is configured in `ObservabilityInitializer.cs`:
- OTLP exporter pointing to Grafana (http://grafana:4318)
- Metrics: Runtime, ASP.NET Core, HTTP client, Orleans meters
- Traces: ASP.NET Core, HTTP client, Orleans sources, custom ActivitySource
- Logs: OpenTelemetry logging with scopes and formatted messages

Custom activity tracking is implemented using `ActivitySource.cs` for distributed tracing.

## Devcontainer

Running the devcontainer starts background services:
- Grafana (localhost:3000): Observability dashboard with LGTM stack
- Localstack: AWS services emulation (DynamoDB, SQS)

## .NET Template

This project is configured as a .NET template (`yo-orleans-init`), allowing you to generate new Orleans playground projects with customizable parameters.

### Quick Start

**1. Install the template:**
```sh
dotnet new install .
```

**2. Create a new solution:**
```sh
# Creates MyOrleansApp/Demo.sln with Silo, Client, and Contracts projects
dotnet new yo-orleans-init -n MyOrleansApp -o ./MyOrleansApp
```

**3. Build and run:**
```sh
cd MyOrleansApp
dotnet build Demo.sln
dotnet run --project Silo/Silo.csproj
```

### Creating a New Project

**Bare infrastructure (no examples - default):**
```sh
dotnet new yo-orleans-init -n MyOrleansApp
```

**With HelloWorld example:**
```sh
dotnet new yo-orleans-init -n MyOrleansApp --examples minimal
```

**Custom configuration:**
```sh
dotnet new yo-orleans-init -n MyOrleansApp \
  --cluster-id production \
  --service-id my-service \
  --silo-port 22222 \
  --gateway-port 33333
```

### Template Parameters

The template supports customizing:
- **Orleans settings**: cluster-id, service-id, silo-port, gateway-port
- **AWS settings**: dynamo-table, aws-region, aws-service-region
- **Observability**: grafana-endpoint, service names, activity sources
- **Examples**: Choose `none` (bare infrastructure - default) or `minimal` (HelloWorld only)

See `.template.config/README.md` for complete parameter documentation.

### Template Files

- `.template.config/template.json`: Main template configuration
- `.template.config/dotnetcli.host.json`: CLI experience settings
- `.template.config/README.md`: Detailed template usage guide

### How It Works

The template uses:
1. **Parameter substitution**: Hardcoded values are replaced with your parameters
2. **Conditional file inclusion**: When `--Examples minimal`, example files are excluded
3. **Preprocessor directives**: Command registrations are conditionally compiled based on examples choice
