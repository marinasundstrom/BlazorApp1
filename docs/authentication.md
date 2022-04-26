# Authentication

## Identity Server

Lets your app be its own authentication provider.

Provides an OIDC endpoint that utilizes JWTs.

## ASP.NET Core Identity

Provides the interface to register and log in users.

The app stores individual accounts in its database.

## External providers

Users can be authenticated via external authentications providers. (Google, Azure AD etcl) 

You can register external providers in ```Program.cs```. They will show up in at the login page after doing so.