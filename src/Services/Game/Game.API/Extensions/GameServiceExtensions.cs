using Game.API.MassTransit;
using Game.BLL.Interfaces;
using Game.BLL.Services;
using Game.DAL.Interfaces;
using Game.DAL.Models;
using Game.DAL.Repositories;
using Game.DAL.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Game.DAL;
using Game.BLL.Helpers;

namespace Game.API.Extensions
{
    public static class GameServiceExtensions
    {
        public static IServiceCollection AddGameServices(this IServiceCollection services, IConfiguration configuration)
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

            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGameServiceHelper, GameServiceHelper>();
            services.AddScoped<IRepository<DAL.Models.Game>, Repository<DAL.Models.Game>>();
            services.AddScoped<IRepository<PlayerGame>, Repository<PlayerGame>>();
            services.AddScoped<IRepository<AppUser>, Repository<AppUser>>();
            services.AddScoped<IRepository<GameState>, Repository<GameState>>();
            services.AddScoped<IRepository<Field>, Repository<Field>>();
            services.AddScoped<IRepository<GameField>, Repository<GameField>>();
            services.AddScoped<IRepository<ShipWrapper>, Repository<ShipWrapper>>();
            services.AddScoped<IRepository<Ship>, Repository<Ship>>();
            services.AddScoped<IRepository<Position>, Repository<Position>>();
            services.AddScoped<IRepository<Cell>, Repository<Cell>>();
            services.AddScoped<IRepository<Direction>, Repository<Direction>>();
            services.AddScoped<IRepository<GameHistory>, Repository<GameHistory>>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetAllGamesConsumer>();
                x.AddConsumer<CreateGameConsumer>();
                x.AddConsumer<IsGameOwnerConsumer>();
                x.AddConsumer<DeleteGameConsumer>();
                x.AddConsumer<JoinSecondPlayerConsumer>();
                x.AddConsumer<GetAllCellsConsumer>();
                x.AddConsumer<CreateShipConsumer>();
                x.AddConsumer<IsPlayerReadyConsumer>();
                x.AddConsumer<IsTwoPlayersReadyConsumer>();
                x.AddConsumer<GetAllCellsForSecondPlayerConsumer>();
                x.AddConsumer<ShootConsumer>();
                x.AddConsumer<PriorityConsumer>();
                x.AddConsumer<IsEndOfTheGameConsumer>();
                x.AddConsumer<ClearingDbConsumer>();

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