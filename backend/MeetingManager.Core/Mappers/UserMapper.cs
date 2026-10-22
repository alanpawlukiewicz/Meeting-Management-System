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
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName!
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

        public static UserAvailabilityDto ToUserAvailabilityDto(this UserAvailability userAvailability)
        {
            return new UserAvailabilityDto
            {
                DayOfWeek = userAvailability.DayOfWeek,
                StartTime = userAvailability.StartTime,
                EndTime = userAvailability.EndTime
            };
        }
    }
}
