# Authentication

## Identity Server

Lets your app be its own authentication provider.

Provides an OIDC endpoint that utilizes JWTs.

## ASP.NET Core Identity

Provides the interface to register and log in users.

The app stores individual accounts in its database. Using Entity Framework Core.

If you want to override the default UI, views can be scaffolded in Visual Studio, or using the .NET CLI.

## External providers

Users can be authenticated via external authentications providers. (Google, Azure AD etcl) 

You can register external providers in ```Program.cs```. They will show up in at the login page after doing so.
