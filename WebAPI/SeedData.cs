﻿using System.Security.Claims;

using BlazorApp1.Domain;
using BlazorApp1.Infrastructure.Persistence;

using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace BlazorApp1.WebAPI;

public class SeedData
{
    public static async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedData>>();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //await context.Database.EnsureDeletedAsync();
            //context.Database.Migrate();
            await context.Database.EnsureCreatedAsync();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            var adminRole = await roleManager.FindByNameAsync("Administrator");
            if(adminRole is null) 
            {
                adminRole = new IdentityRole() 
                {
                    Name = "Administrator"
                };

                await roleManager.CreateAsync(adminRole);
            }

            var userRole = await roleManager.FindByNameAsync("User");
            if(userRole is null) 
            {
                userRole = new IdentityRole() 
                {
                    Name = "User"
                };

                await roleManager.CreateAsync(userRole);
            }

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
           
            var alice = userMgr.FindByNameAsync("alice@email.com").Result;
            if (alice == null)
            {
                alice = new ApplicationUser
                {
                    UserName = "alice@email.com",
                    Email = "alice@email.com",
                    EmailConfirmed = true,
                };
                var result = userMgr.CreateAsync(alice, "Abc123!?").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                await userMgr.AddToRoleAsync(alice, adminRole.Name);

                logger.LogDebug("alice created");
            }
            else
            {
                logger.LogDebug("alice already exists");
            }

            var bob = userMgr.FindByNameAsync("bob@email.com").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    UserName = "bob@email.com",
                    Email = "bob@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(bob, "Abc123!?").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                await userMgr.AddToRoleAsync(bob, userRole.Name);

                logger.LogDebug("bob created");
            }
            else
            {
                logger.LogDebug("bob already exists");
            }
        }
    }
}