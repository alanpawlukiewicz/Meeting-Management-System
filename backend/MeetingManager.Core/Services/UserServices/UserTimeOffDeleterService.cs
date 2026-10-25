using MeetingManager.Core.Exceptions;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services.UserServices
{
    public class UserTimeOffDeleterService : IUserTimeOffDeleterService
    {
        private readonly IUserTimeOffRepository _userTimeOffRepository;
        private readonly ICurrentUserService _currentUserService;

        public UserTimeOffDeleterService(IUserTimeOffRepository userTimeOffRepository, ICurrentUserService currentUserService)
        {
            _userTimeOffRepository = userTimeOffRepository;
            _currentUserService = currentUserService;
        }

        public async Task DeleteTimeOffAsync(Guid timeOffId, CancellationToken ct = default)
        {
            var matchingTimeOff = await _userTimeOffRepository.FindUserTimeOffAsync(timeOffId, ct);

            if (matchingTimeOff == null || matchingTimeOff.UserId != _currentUserService.UserId)
            {
                throw new NotFoundException("Time off record could not be found.");
            }

            _userTimeOffRepository.Remove(matchingTimeOff);
            await _userTimeOffRepository.SaveChangesAsync(ct);
        }
    }
}
