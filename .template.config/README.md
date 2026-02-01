# Yo Orleans Init - .NET Template

This directory contains the .NET template configuration for creating new Orleans playground projects.

## Quick Start

### 1. Install the Template

From the repository root directory:

```bash
dotnet new install .
```

Or install from a specific path:

```bash
dotnet new install /path/to/orleans-playground
```

### 2. Create a New Solution

```bash
# Create a new Orleans solution in current directory
dotnet new yo-orleans-init -n MyOrleansApp

# Or specify output directory
dotnet new yo-orleans-init -n MyOrleansApp -o ./MyOrleansApp
```

This creates:
- `MyOrleansApp/Demo.sln` - Solution file
- `MyOrleansApp/Silo/` - Server project
- `MyOrleansApp/Client/` - Client project
- `MyOrleansApp/Contracts/` - Shared interfaces

### 3. Build and Run

```bash
cd MyOrleansApp
dotnet build Demo.sln
dotnet run --project Silo/Silo.csproj
```

## Using the Template

### Basic Usage

Create a new project with default settings (no examples, bare infrastructure):

```bash
dotnet new yo-orleans-init -n MyOrleansApp
```

This creates a bare Orleans project with just the infrastructure - no example grains or commands.

### With HelloWorld Example

Create a project with the basic HelloWorld example:

```bash
dotnet new yo-orleans-init -n MyOrleansApp --examples minimal
```

### Custom Configuration

Customize ports, cluster settings, and service names:

```bash
dotnet new yo-orleans-init -n MyOrleansApp \
  --cluster-id production-cluster \
  --service-id payment-service \
  --silo-port 22222 \
  --gateway-port 33333 \
  --dynamo-table production-storage \
  --silo-service-name PaymentSilo \
  --client-service-name PaymentClient
```

## Available Parameters

### Core Orleans Settings

- `--cluster-id` / `-c`: Orleans cluster identifier (default: "my-cluster")
- `--service-id` / `-s`: Orleans service identifier (default: "my-service")
- `--silo-port`: Silo communication port (default: 11111)
- `--gateway-port`: Client gateway port (default: 30001)

### Storage & AWS Settings

- `--dynamo-table`: DynamoDB table name (default: "default-table")
- `--aws-region`: AWS region for DynamoDB (default: "eu-west-1")
- `--aws-service-region`: AWS service region for connection strings (default: "us-east-1")

### Observability Settings

- `--grafana-endpoint`: OpenTelemetry OTLP endpoint (default: "http://grafana:4318")
- `--silo-service-name`: Service name for silo observability (default: "MySilo")
- `--client-service-name`: Service name for client observability (default: "MyClient")
- `--silo-activity-source`: ActivitySource name for silo (default: "my-silo")
- `--client-activity-source`: ActivitySource name for client (default: "my-client")

### Example Selection

- `--examples` / `-e`: Choose which examples to include
  - `none`: No examples - bare Orleans infrastructure only (default)
  - `minimal`: HelloWorld example only

## Examples

### Development Setup with Example

```bash
dotnet new yo-orleans-init -n DevPlayground --examples minimal
cd DevPlayground
dotnet build
dotnet run --project Silo/Silo.csproj
```

### Production-like Configuration

```bash
dotnet new yo-orleans-init -n ProductionService \
  --cluster-id prod-cluster \
  --service-id order-service \
  --silo-port 11000 \
  --gateway-port 30000 \
  --dynamo-table orders-storage \
  --aws-region us-west-2
```

### Custom Observability

```bash
dotnet new yo-orleans-init -n MonitoredService \
  --grafana-endpoint http://localhost:4317 \
  --silo-service-name OrderSilo \
  --client-service-name OrderClient \
  --silo-activity-source orders-silo-trace \
  --client-activity-source orders-client-trace
```

## Uninstalling the Template

```bash
dotnet new uninstall Yo.Orleans.Init.Template
```

Or uninstall by path:

```bash
dotnet new uninstall /path/to/orleans-playground
```

## Building and Packaging the Template

### Testing Locally

Install the template locally for development and testing:

```bash
# From repository root
dotnet new install .

# Verify installation
dotnet new list yo-orleans-init

# Test by creating a sample project
dotnet new yo-orleans-init -n TestApp -o /tmp/test-output
cd /tmp/test-output
dotnet build Demo.sln
```

### Packaging as NuGet

To distribute the template, package it as a NuGet package:

**1. Create a nuspec file** (optional, for fine control):

```xml
<?xml version="1.0" encoding="utf-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
  <metadata>
    <id>Yo.Orleans.Init.Template</id>
    <version>1.0.0</version>
    <authors>m4tchl0ck</authors>
    <description>Orleans playground template with Silo, Client, and Contracts</description>
    <packageTypes>
      <packageType name="Template" />
    </packageTypes>
  </metadata>
</package>
```

**2. Pack the template:**

```bash
dotnet pack -o ./artifacts
```

**3. Install from package:**

```bash
dotnet new install ./artifacts/Yo.Orleans.Init.Template.1.0.0.nupkg
```

**4. Publish to NuGet.org:**

```bash
dotnet nuget push ./artifacts/Yo.Orleans.Init.Template.1.0.0.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

## What Gets Generated

### None Mode (default)

Projects and files:
- `Silo`: Orleans server (no example grains)
- `Client`: CliFx CLI application (no example commands)
- `Contracts`: Shared interfaces library (no example interfaces)
- Configuration files with your custom parameters
- Devcontainer setup with Grafana and Localstack

Ready for you to add your own grains and commands!

### Minimal Mode

Includes everything from none mode plus:
- `Contracts/IHelloWorld.cs`: Basic grain interface
- `Silo/HelloWorld.cs`: Simple grain with state
- `Client/InteractiveCommand.cs`: Interactive CLI mode
- `Client/Commands/SayHelloCommand.cs`: Command to call HelloWorld grain

## Template Structure

The template uses:
- **Parameter substitution**: Replace hardcoded values with your parameters
- **Conditional file inclusion**: Exclude example files when using minimal mode
- **Preprocessor directives**: Conditionally compile command registrations in minimal mode

## After Generation

1. Build the solution:
   ```bash
   dotnet build
   ```

2. Run the silo:
   ```bash
   dotnet run --project Silo/Silo.csproj
   ```

3. In another terminal, run the client:
   ```bash
   dotnet run --project Client/Client.csproj say-hello -g "YourName"
   ```

4. For interactive mode:
   ```bash
   dotnet run --project Client/Client.csproj interactive
   ```

## Troubleshooting

### Template not found
- Make sure you ran `dotnet new install` from the correct directory
- Check installed templates: `dotnet new list yo-orleans-init`
- List all template sources: `dotnet new list`

### Build errors in minimal mode
- The minimal mode excludes example files but keeps directory structure
- If you see missing type errors, ensure you're not referencing excluded examples

### Port conflicts
- Use `--silo-port` and `--gateway-port` to avoid conflicts with other services
- Default ports: 11111 (silo), 30001 (gateway)

### Verifying Installation

Check that the template is properly installed:

```bash
# List the template
dotnet new list yo-orleans-init

# View help for all parameters
dotnet new yo-orleans-init --help

# Create a test project in a temp directory
dotnet new yo-orleans-init -n VerifyTest -o /tmp/verify-test --examples minimal
cd /tmp/verify-test
dotnet build Demo.sln
```

If the build succeeds, the template is working correctly.

## Template Files

- `template.json`: Main template configuration with parameters and conditional logic
- `dotnetcli.host.json`: CLI-specific settings for better command-line experience
- `README.md`: This file
