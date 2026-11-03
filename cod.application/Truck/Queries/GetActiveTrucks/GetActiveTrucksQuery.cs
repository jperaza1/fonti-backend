using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Truck.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Truck.Queries.GetActiveTrucks
{
    public class GetActiveTrucksQuery : BaseQuery, IGetActiveTrucksQuery
    {
        public GetActiveTrucksQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<TruckModel>> Execute()
        {
            return await _repository.QueryActive<domain.Data.Truck>()
                .Where(f => f.TruckStatus == domain.Features.Enums.DomainEnums.TruckStatus.Available)
                .Select(f => new TruckModel
            {
                Id = f.Id,
                Name = f.Name,
                Plate = f.Plate,
                TruckStatus = f.TruckStatus,
                TruckStatusName = f.TruckStatus.GetEnumDescription(),
                IsActive = f.IsActive,
            }).OrderBy(f => f.Name).ToListAsync();
        }
    }
}
