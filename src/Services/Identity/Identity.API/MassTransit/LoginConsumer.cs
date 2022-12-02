using Identity.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Identity.API.MassTransit
{
    public class LoginConsumer : IConsumer<LoginRequest>
    {
        private readonly IIdentityService _identityService;

        public LoginConsumer(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task Consume(ConsumeContext<LoginRequest> context)
        {
            var result = await _identityService.Login(context.Message);

            await context.RespondAsync(result);
        }
    }
}