﻿using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class CreateShipConsumer : IConsumer<CreateShipRequest>
    {
        private readonly IGameService _gameService;

        public CreateShipConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<CreateShipRequest> context)
        {
            try
            {
                _gameService.CreateShipOnField(context.Message);
                await context.RespondAsync(new CreateShipResponse { Message = "Create ship was successful!" });
            }
            catch
            {
                await context.RespondAsync(new CreateShipResponse { Message = "Create ship failed!" });
            }
        }
    }
}
