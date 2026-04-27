using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Product.Common;
using cod.application.SideItem.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.SideItem.Queries.GetSideItem
{
    public class GetSideItemQuery : BaseQuery, IGetSideItemQuery
    {
        public GetSideItemQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<SideItemModel> Execute(int Id)
        {
            var baseQuery = _repository.Query<domain.SalePoint.SideItem>().Where(f => f.Id == Id).Select(f => new
            {
                f.Id,
                f.Name,
                f.IsActive,
            });

            var sideItem = await baseQuery.FirstOrDefaultAsync();

            if (sideItem == null)
                throw new ApplicationException("Complemento no existe", "COMPLEMENTO");

            return new SideItemModel
            {
                Id = sideItem.Id,
                Name = sideItem.Name,
                IsActive = sideItem.IsActive
            };
        }
    }
}
