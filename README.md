# BlazorApp sample

Simple app including everything needed for a enterprise-grade Blazor app that is hosted in an ASP.NET Core app together with a Web API.

The main purpose is to show how to set all this up.

This project started off as a merger of the Blazor Hosted with Individual Auth template and the MudBlazor Hosted template.
## Contents

The app comes with this:

* MudBlazor component library
* Web API
  * Open API (w. Swagger)
* Entity Framework Core
* Authentication
  * Identity Server (OIDC endpoint)
  * ASP.NET Core Identity (individual accounts)
  * Support for external providers (Google, Azure AD etc.)
* SignalR
* Localization
* CQRS with MediatR
* .NET Tye - Development orchestrator

There is also a Worker service, using MassTransit with RabbitMQ as transport.

## Prerequisites

The project requires these dependencies to be built and to run:

* [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [.NET Tye CLI tools](https://github.com/dotnet/tye)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)

And then an IDE of your preference, such as [VS Code](https://code.visualstudio.com/), full Visual Studio for Mac or Windows, or Rider.

## Run the app

Run this in the solution folder.

```sh
$ tye run
```

This will launch the app and SQL Server running in a container.

To make the services restart on changes:

```sh
$ tye run --watch
```

## Tye

Tye is an orchestrator that can be used to develop distributed applications. It orchestrates services - projects and containers, without developers having to think about configuration. Declare dependencies and run. It also handles *service discovery*.

Services are declared in ```tye.yaml```.

It also makes it easy to deploy services to Kubernetes.