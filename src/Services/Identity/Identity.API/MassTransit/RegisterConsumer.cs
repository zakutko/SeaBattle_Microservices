using Identity.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Identity.API.MassTransit
{
    public class RegisterConsumer : IConsumer<RegisterRequest>
    {
        private readonly IIdentityService _identityService;

        public RegisterConsumer(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task Consume(ConsumeContext<RegisterRequest> context)
        {
            var result = await _identityService.Register(context.Message);

            await context.RespondAsync<UserResponse>(new { result.Token, result.Username }); 
        }
    }
}
