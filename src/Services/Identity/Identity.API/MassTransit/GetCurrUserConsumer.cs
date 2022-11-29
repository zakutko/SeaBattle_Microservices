using Identity.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Identity.API.MassTransit
{
    public class GetCurrUserConsumer : IConsumer<GetCurrUserRequest>
    {
        private readonly IIdentityService _identityService;

        public GetCurrUserConsumer(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task Consume(ConsumeContext<GetCurrUserRequest> context)
        {
            var result = _identityService.GetCurrUser(context.Message);

            await context.RespondAsync(result);
        }
    }
}