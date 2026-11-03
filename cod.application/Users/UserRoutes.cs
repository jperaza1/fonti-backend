using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;

namespace cod.application.Users
{
    public static class UserRoutes
    {
        public static List<int> Normalize(IEnumerable<int> routes)
        {
            if (routes == null)
                return new List<int>();

            return routes.Where(id => id > 0).Distinct().ToList();
        }

        public static string Serialize(DomainEnums.UserType userType, IEnumerable<int> routes)
        {
            if (userType != DomainEnums.UserType.Vendor)
                return string.Empty;

            List<int> routeIds = Normalize(routes);
            return routeIds.Any() ? string.Join(",", routeIds) : string.Empty;
        }

        public static List<int> Parse(string routes)
        {
            if (string.IsNullOrWhiteSpace(routes))
                return new List<int>();

            return routes
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(value => int.Parse(value.Trim()))
                .ToList();
        }

        public static async Task Validate(IRepository repository, IExceptionFactory exceptionFactory, DomainEnums.UserType userType, IEnumerable<int> routes)
        {
            if (userType != DomainEnums.UserType.Vendor)
                return;

            List<int> routeIds = Normalize(routes);
            if (!routeIds.Any())
            {
                exceptionFactory.Add("routes", exceptionFactory.GetMessage("Routes", ExceptionEnums.ExceptionCode.Required));
                return;
            }

            int activeCount = await repository.QueryActive<cod.domain.Data.Route>()
                .CountAsync(route => routeIds.Contains(route.Id));

            if (activeCount != routeIds.Count)
                exceptionFactory.Add("routes", exceptionFactory.GetMessage("Una o más rutas no existen o están inactivas", ExceptionEnums.ExceptionCode.Default));
        }
    }
}
