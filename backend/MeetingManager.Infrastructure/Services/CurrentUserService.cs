using MeetingManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MeetingManager.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User
                    .FindFirst(ClaimTypes.NameIdentifier)
                    ?? throw new UnauthorizedAccessException("No logged in user available.");

                return Guid.Parse(claim.Value);
            }
        }
    }
}
