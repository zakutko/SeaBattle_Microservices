using GameHistory.API.MassTransit;
using GameHistory.BLL.Interfaces;
using GameHistory.BLL.Services;
using GameHistory.DAL;
using GameHistory.DAL.Data;
using GameHistory.DAL.Interfaces;
using GameHistory.DAL.Models;
using GameHistory.DAL.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GameHistory.API.Extensions
{
    public static class GameHistoryServiceExtensions
    {
        public static IServiceCollection AddGameHistoryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<AppUser>>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddScoped<IGameHistoryService, GameHistoryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<DAL.Models.GameHistory>, Repository<DAL.Models.GameHistory>>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetAllGameHistoriesConsumer>();
                x.AddConsumer<GetTopPlayersConsumer>();

                x.SetKebabCaseEndpointNameFormatter();
                x.AddDelayedMessageScheduler();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(r =>
                    {
                        r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
                    });

                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}