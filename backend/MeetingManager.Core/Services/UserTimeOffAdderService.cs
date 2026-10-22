using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Services
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

            var existing = await _userTimeOffRepository.FindUserTimeOffByDateAsync(userId, request.Date, ct);

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
