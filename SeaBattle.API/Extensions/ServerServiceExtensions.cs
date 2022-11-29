using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SeaBattle.Contracts.Dtos;
using System.Text;

namespace SeaBattle.API.Extensions
{
    public static class ServerServiceExtensions
    {
        public static IServiceCollection AddServerServices(this IServiceCollection services, IConfiguration configuration)
        {
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

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();

                x.AddRequestClient<GetCurrUserRequest>();
                x.AddRequestClient<LoginRequest>();
                x.AddRequestClient<RegisterRequest>();
                x.AddRequestClient<GameListRequest>();
                x.AddRequestClient<CreateGameRequest>();
                x.AddRequestClient<IsGameOwnerRequest>();
                x.AddRequestClient<DeleteGameRequest>();
                x.AddRequestClient<JoinSecondPlayerRequest>();
                x.AddRequestClient<CellListRequest>();
                x.AddRequestClient<CreateShipRequest>();
                x.AddRequestClient<IsPlayerReadyRequest>();
                x.AddRequestClient<IsTwoPlayersReadyRequest>();
                x.AddRequestClient<ShootRequest>();
                x.AddRequestClient<HitRequest>();
                x.AddRequestClient<IsEndOfTheGameRequest>();
                x.AddRequestClient<ClearingDBRequest>();
                x.AddRequestClient<GameHistoryRequest>();
                x.AddRequestClient<TopPlayersRequest>();
            });

            return services;
        }
    }
}