using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Security.Claims;

namespace EasyAbp.Abp.EventBus.CAP
{
    public class AbpCapDashboardAuthenticationHandler :
        AuthenticationHandler<AbpCapDashboardAuthenticationSchemeOptions>
    {
        public static string SchemeName = "AbpCapDashboardScheme";

        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
        private readonly IAuthorizationService _authorizationService;

        public AbpCapDashboardAuthenticationHandler(
            ICurrentPrincipalAccessor currentPrincipalAccessor,
            IAuthorizationService authorizationService,
            IOptionsMonitor<AbpCapDashboardAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder)
        {
            _currentPrincipalAccessor = currentPrincipalAccessor;
            _authorizationService = authorizationService;
            options.CurrentValue.ForwardChallenge = "";
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Options.PermissionName.IsNullOrEmpty())
                {
                    await _authorizationService.CheckAsync(Options.PermissionName);
                }
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail(e);
            }

            return AuthenticateResult.Success(new AuthenticationTicket(_currentPrincipalAccessor.Principal,
                SchemeName));
        }
    }
}