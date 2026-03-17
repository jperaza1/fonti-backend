using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.SideItem.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SideItem.Queries.GetSideItems
{
    public class GetSideItemsQuery : BaseQuery, IGetSideItemsQuery
    {
        public GetSideItemsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<SideItemModel>> Execute()
        {
            var baseQuery = _repository.Query<domain.SalePoint.SideItem>().Select(f => new
            {
                f.Id,
                f.Name,
                f.IsActive
            });

            return await baseQuery.Select(f => new SideItemModel
            {
                Id = f.Id,
                Name = f.Name,
                IsActive = f.IsActive
            }).ToListAsync();
        }
    }
}
