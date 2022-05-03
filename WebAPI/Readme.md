# Server

## Configure additional services

Tye makes it easy to add additional servies to your app.

This project comes preconfigured with MassTransit on RabbitMQ and Redis. You just have to turn the services on.

In order to turn them on, uncomment the lines for earch service in ```tye.yaml```, ```BlazorApp1.Server.csproj```, and ```Program.cs```.

You have to restart Tye and all servies for changes to get applied.

## Notice about Duende Identity Server

This code includes a dependency on Duende IdentityServer.
This is an open source product with a reciprocal license agreement. If you plan to use Duende IdentityServer in production this may require a license fee.
To see how to use Azure Active Directory for your identity please see https://aka.ms/aspnetidentityserver
To see if you require a commercial license for Duende IdentityServer please see https://aka.ms/identityserverlicense
