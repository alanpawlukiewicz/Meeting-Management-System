using MeetingManager.Core.Exceptions;
using MeetingManager.Core.ServiceContracts.MeetingServiceContracts;
using MeetingManager.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Services.MeetingServices
{
    public class MeetingDeleterService : IMeetingDeleterService
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingDeleterService(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        public async Task DeleteMeetingAsync(Guid userId, Guid meetingId, CancellationToken ct = default)
        {
            var matchingRepository = await _meetingRepository.FindMeetingAsync(meetingId, ct)
                ?? throw new NotFoundException("Meeting could not be found");

            if (matchingRepository.CreatorId != userId)
                throw new ValidationException("Only creator can delete this meeting.");

            _meetingRepository.Remove(matchingRepository);
            await _meetingRepository.SaveChangesAsync(ct);
        }
    }
}
