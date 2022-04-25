# BlazorApp sample

Simple app including everything needed for a enterprise-grade Blazor app that is hosted in an ASP.NET Core app together with a Web API.

The main purpose is to show how to set all this up.

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
* .NET Tye - Development orchestrator

## Prerequisites

The project requires these dependencies to be built:

* .NET 6 SDK
* .NET Tye CLI tools
* Docket Desktop

And then an IDE of your preference, such as VS Code.

## Run the app

Run this in the solution folder.

```sh
$ tye run
```

This will launch the app and SQL Server running in a container.

## Tye

Tye is an orchestrator that can be used to develop distributed applications. It orchestrates services - projects and containers, without developers having to think about configuration. It also handles *service discovery*.

It also makes it easy to deploy services to Kubernetes.