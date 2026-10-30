using MeetingManager.Core.DTO.UserDTOs;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services.UserServices
{
    public class UserTimeOffAdderService : IUserTimeOffAdderService
    {
        private readonly IUserTimeOffRepository _userTimeOffRepository;

        public UserTimeOffAdderService(IUserTimeOffRepository userTimeOffRepository)
        {
            _userTimeOffRepository = userTimeOffRepository;
        }

        public async Task<UserTimeOffResponse> AddUserTimeOffAsync(Guid userId, UserTimeOffAddRequest request, CancellationToken ct = default)
        {
            if (request.StartTime >= request.EndTime)
                throw new ValidationException("StartTime must be earlier than EndTime.");

            if (request.Date < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ValidationException("Given date is expired.");

            var existing = await _userTimeOffRepository.GetUserTimeOffByDateAsync(userId, request.Date, ct);

            if (existing.Any(uto => uto.StartTime < request.EndTime && uto.EndTime > request.StartTime))
            {
                throw new ConflictException("Given data conflicts with already existing time offs.");
            }

            var newTimeOff = new UserTimeOff
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = request.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsAvailable = request.IsAvailable
            };

            _userTimeOffRepository.Add(newTimeOff);
            await _userTimeOffRepository.SaveChangesAsync(ct);

            return newTimeOff.ToUserTimeOffResponse();
        }
    }
}
