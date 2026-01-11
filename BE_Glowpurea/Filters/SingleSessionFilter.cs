using BE_Glowpurea.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BE_Glowpurea.Filters
{
    public class SingleSessionFilter : IAsyncActionFilter
    {
        private readonly IAccountRepository _accountRepo;

        public SingleSessionFilter(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;

            if (!user.Identity!.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var tokenJti = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(tokenJti))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var account = await _accountRepo.GetByEmailAsync(email);

            if (account == null || account.CurrentJti != tokenJti)
            {
                context.Result = new UnauthorizedObjectResult(
                    "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại."
                );
                return;
            }

            await next();
        }
    }
}
