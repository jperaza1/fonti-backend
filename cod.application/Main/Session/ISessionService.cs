using cod.domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Main.Session
{
    public interface ISessionService
    {
        Task<string> GetUserId();
        Task<ApplicationUserProfile> GetUserProfile();
    }
}
