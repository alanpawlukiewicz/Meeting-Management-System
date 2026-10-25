using MeetingManager.Core.DTO;
using MeetingManager.Domain.Entities;

namespace MeetingManager.Core.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName!,
                RoleNames = user.UserRoles.Select(ur => ur.Role!.Name).ToList()
            };
        }

        public static UserProfile ToUserProfile(this User user)
        {
            return new UserProfile
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = user.UserRoles.Select(ur => ur.Role!.Name).ToList(),
                UserName = user.UserName
            };
        }

        public static UserResponse ToUserResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName
            };
        }

        public static UserAvailabilityDto ToUserAvailabilityDto(this UserAvailability userAvailability)
        {
            return new UserAvailabilityDto
            {
                DayOfWeek = userAvailability.DayOfWeek,
                StartTime = userAvailability.StartTime,
                EndTime = userAvailability.EndTime
            };
        }

        public static UserTimeOffResponse ToUserTimeOffResponse(this UserTimeOff userTimeOff)
        {
            return new UserTimeOffResponse
            {
                Id = userTimeOff.Id,
                Date = userTimeOff.Date,
                StartTime = userTimeOff.StartTime,
                EndTime = userTimeOff.EndTime,
                IsAvailable = userTimeOff.IsAvailable
            };
        }
    }
}
