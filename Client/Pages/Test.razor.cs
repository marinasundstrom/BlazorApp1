﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using MudBlazor;
using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Client.Pages
{
    public partial class Test
    {
        HubConnection hubConnection = null !;

        [Required]
        public string Name { get; set; } = null !;

        async Task OnSubmit()
        {
            await hubConnection.InvokeAsync("SayHi", Name);
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                hubConnection = new HubConnectionBuilder().WithUrl($"{NavigationManager.BaseUri}hubs/test", options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var results = await AccessTokenProvider.RequestAccessToken(new AccessTokenRequestOptions()
                        {Scopes = new[]{"BlazorApp1.ServerAPI"}});
                        if (results.TryGetToken(out var accessToken))
                        {
                            return accessToken.Value;
                        }

                        return null !;
                    };
                }).WithAutomaticReconnect().Build();
                hubConnection.On<string>("Responded", OnReponded);
                hubConnection.Closed += (error) =>
                {
                    if (error is not null)
                    {
                        Snackbar.Add($"{error.Message}", Severity.Error);
                    }

                    Snackbar.Add("Connection closed");
                    return Task.CompletedTask;
                };
                hubConnection.Reconnected += (error) =>
                {
                    Snackbar.Add("Reconnected");
                    return Task.CompletedTask;
                };
                hubConnection.Reconnecting += (error) =>
                {
                    Snackbar.Add("Reconnecting");
                    return Task.CompletedTask;
                };
                await hubConnection.StartAsync();
                Snackbar.Add("Connected");
            }
            catch (Exception exc)
            {
                Snackbar.Add(exc.Message.ToString(), Severity.Error);
            }
        }

        Task OnReponded(string message)
        {
            Snackbar.Add(message);
            StateHasChanged();
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            await hubConnection.DisposeAsync();
        }
    }
}