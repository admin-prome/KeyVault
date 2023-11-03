using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Minimal API configuration
app.MapGet("/", () => "Hello, World!");

// Azure Key Vault configuration
var keyVaultUrl = new Uri("https://tecnokeys.vault.azure.net");

app.MapGet("/get-key", async () =>
{
    // Acquire access token for Azure Key Vault
    var credential = new DefaultAzureCredential();

    // Log Key Vault URL and access token
    Console.WriteLine($"Azure Key Vault URL: {keyVaultUrl}");

    // Initialize the SecretClient with the access token
    var secretClient = new SecretClient(keyVaultUrl, credential);

    string secretName = "Test";

    try
    {
        KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName);

        Debug.WriteLine($"Secret Name: {secret.Name}");
        return $"Secret Value: {secret.Value}";
    }
    catch (RequestFailedException ex)
    {
        return $"Error getting secret: {ex.Message}";
    }
});

app.Run();
