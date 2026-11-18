using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Inventory.Services
{
    public class WarehouseDirectory : IWarehouseDirectory
    {
        private readonly IRepository _repository;

        public WarehouseDirectory(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<domain.Operation.Warehouse>> EnsureAll()
        {
            var rows = await _repository.Query<domain.Operation.Warehouse>().ToListAsync();
            var created = false;

            foreach (WarehouseKind kind in Enum.GetValues(typeof(WarehouseKind)))
            {
                if (rows.Any(row => row.Kind == kind))
                    continue;

                var warehouse = new domain.Operation.Warehouse
                {
                    Name = Name(kind),
                    Kind = kind
                };
                _repository.InsertAsync(warehouse);
                rows.Add(warehouse);
                created = true;
            }

            if (created)
                await _repository.CommitAsync();

            return rows
                .Where(row => row.IsActive)
                .OrderBy(row => row.Kind)
                .ToList();
        }

        public async Task<domain.Operation.Warehouse> Require(WarehouseKind kind)
        {
            var warehouse = await _repository.QueryActive<domain.Operation.Warehouse>()
                .FirstOrDefaultAsync(row => row.Kind == kind);

            if (warehouse == null)
                throw new ApplicationException("No existe la bodega " + Name(kind), "INVENTARIO");

            return warehouse;
        }

        public async Task<domain.Operation.Warehouse> Get(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.QueryActive<domain.Operation.Warehouse>()
                .FirstOrDefaultAsync(row => row.Id == id);
        }

        public bool Allows(WarehouseKind kind, ProductType type)
        {
            switch (kind)
            {
                case WarehouseKind.Principal:
                    return type == ProductType.RawMaterial || type == ProductType.Purchase;
                case WarehouseKind.Production:
                    return type == ProductType.RawMaterial || type == ProductType.Purchase || type == ProductType.Sale;
                case WarehouseKind.Finished:
                    return type == ProductType.Sale;
                default:
                    return false;
            }
        }

        public static string Name(WarehouseKind kind)
        {
            switch (kind)
            {
                case WarehouseKind.Principal:
                    return "Principal";
                case WarehouseKind.Production:
                    return "Producción";
                case WarehouseKind.Finished:
                    return "Terminado";
                default:
                    return kind.ToString();
            }
        }
    }
}
