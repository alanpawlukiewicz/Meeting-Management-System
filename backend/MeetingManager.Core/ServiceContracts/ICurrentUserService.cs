using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.ServiceContracts
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
    }
}
