using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.DispatchControl.Commom;
using cod.application.DispatchControl.Services;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Data;
using cod.domain.Features;
using DocuSign.eSign.Model;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.DispatchControl.Queries.GenerateReportDispatchControl
{
    public class GenerateReportDispatchControlQuery : BaseQuery, IGenerateReportDispatchControlQuery
    {
        private readonly AuthService _authService;
        private readonly IDispatchTransferSettings _transferSettings;
        public GenerateReportDispatchControlQuery(
            IRepository repository,
            AuthService authService,
            IDispatchTransferSettings transferSettings) : base(repository)
        {
            _authService = authService;
            _transferSettings = transferSettings;
        }

        public async Task<byte[]> Execute(DispatchControlFilterModel filterModel)
        {
            var timezone = _authService.ClientTimezone;

            Expression<Func<domain.Operation.DispatchControl, bool>> byDate = GetDispatchControlByDate(filterModel, timezone);

            var baseQuery = await _repository.Query<domain.Operation.DispatchControl>()
                .Where(byDate)
                .Select(f => new
                {
                    f.Id,
                    f.RouteId,
                    RouteName = f.Route.Name,
                    f.Vendor,
                    f.TruckId,
                    Truck = f.TruckId.HasValue ? f.Truck.Plate : string.Empty,
                    f.DispatchControlDate,
                    f.CreationTime,
                    f.CreatorUser,
                    TotalReload = f.DispatchControlDetails.Where(f => f.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.Reload).Count(),
                    DispatchControlDetails = f.DispatchControlDetails.Select(g => new
                    {
                        g.Id,
                        g.DispatchControlId,
                        g.BottleQuantity,
                        g.CrateQuantity,
                        g.IsActive,
                        g.DispatchControlType,
                        DispatchControlTypeName = g.DispatchControlType.GetEnumDescription(),
                        g.MissingBottleQuantity,
                        CounterpartRoute = g.CounterpartRoute.Name,
                        ProductMovements = g.ProductMovements.Select(x => new
                        {
                            x.Id,
                            x.ProductId,
                            x.Product.Name,
                            x.Quantity,
                            x.DispatchControlDetailId,
                            ProductName = x.Product.Name,

                        }).ToList()
                    }).ToList()
                }).ToListAsync();

            var maxReload = baseQuery.Max(f => f.TotalReload);

            using (MemoryStream memoryStream = new())
            {
                using (ExcelPackage package = new(memoryStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Hoja1");

                    worksheet.Cells["A1:U1"].Merge = true;
                    worksheet.Cells["A1"].Value = "CONTROL DE SALIDAS";
                    worksheet.Cells["A1"].Style.Font.Bold = true;
                    worksheet.Cells["A1"].Style.Font.Size = 16;
                    worksheet.Cells["A1"].Style.HorizontalAlignment =
                        ExcelHorizontalAlignment.Center;

                    // Información general
                    worksheet.Cells["A2"].Value = "Ruta:";
                    //worksheet.Cells["B2"].Value = report.Date;
                    //worksheet.Cells["B2"].Style.Numberformat.Format = "dd/MM/yyyy";

                    worksheet.Cells["B2"].Value = "Vendedor:";
                    worksheet.Cells["C2"].Value = "Fecha:";
                    //worksheet.Cells["E2:H2"].Merge = true;
                    //worksheet.Cells["E2"].Value = report.Seller;

                    CreateGroupHeader(worksheet, 4,6, "Salida por la manana");

                    int reloadStartColumn = 7;
                    int columnsPerReload = 5;

                    for (int group = 0; group < maxReload; group++)
                    {
                        int startColumn = reloadStartColumn + (group * columnsPerReload);
                        int endColumn = startColumn + columnsPerReload - 1;

                        CreateGroupHeader(
                            worksheet,
                            startColumn,
                            endColumn,
                            $"Recarga {group + 1}"
                        );
                    }

                    // Retorno lleno después de la última recarga
                    int returnStartColumn =
                        reloadStartColumn + (maxReload * columnsPerReload);

                    int returnEndColumn =
                        returnStartColumn + columnsPerReload - 2;

                    var showTransfers = await _transferSettings.IsEnabled();
                    int transferColumn = returnEndColumn + 2;
                    if (showTransfers)
                    {
                        worksheet.Cells[2, transferColumn].Value = "Traspasos";
                        worksheet.Cells[2, transferColumn].Style.Font.Bold = true;
                        worksheet.Cells[3, transferColumn].Value = "Detalle";
                    }

                    CreateGroupHeader(
                        worksheet,
                        returnStartColumn,
                        returnEndColumn,
                        "Retorno lleno"
                    );

                    string[] detailHeaders =
                    {
                        "BOTELLON", "PAQUETE 500", "PAQUETE 250"
                    };

                    int column = 4;

                    // Cuatro grupos, cada uno con cuatro columnas
                    for (int group = 0; group < (2 + maxReload); group++)
                    {
                        foreach (string header in detailHeaders)
                        {
                            worksheet.Cells[3, column].Value = header;
                            column++;
                        }

                        if(group > 0 &&  group < (2 + maxReload) - 1)
                        {
                            worksheet.Cells[3, column].Value = "BOTES";
                            column++;
                            worksheet.Cells[3, column].Value = "CESTAS";
                            column++;
                        }

                        if(group == (1 + maxReload))
                        {
                            worksheet.Cells[3, column].Value = "Botes Faltantes";
                        }

                    }

                    int row = 4;
                    int column1 = 4;
                    foreach(var dispatch in baseQuery.OrderBy(f => f.DispatchControlDate).OrderBy(g => g.RouteName))
                    {
                        column1 = 4;

                        worksheet.Cells[row, 1].Value = dispatch.RouteName;
                        worksheet.Cells[row, 2].Value = dispatch.Vendor;
                        worksheet.Cells[row, 3].Value = dispatch.DispatchControlDate;
                        worksheet.Cells[row, 3].Style.Numberformat.Format = "dd-mm-yyyy";

                        var exit = dispatch.DispatchControlDetails.FirstOrDefault(f => f.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.Exit);

                        foreach(var product in exit.ProductMovements)
                        {
                            worksheet.Cells[row, column1].Value = product.Quantity;
                            column1++;
                        }

                        var reload = dispatch.DispatchControlDetails.Where(f => f.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.Reload).ToList();

                        foreach (var detail in reload) 
                        {
                            foreach(var product in detail.ProductMovements)
                            {
                                worksheet.Cells[row, column1].Value = product.Quantity;
                                column1++;
                            }
                            worksheet.Cells[row, column1].Value = detail.BottleQuantity;
                            column1++;
                            worksheet.Cells[row, column1].Value = detail.CrateQuantity;
                            column1++;
                        }

                        var returnDetail = dispatch.DispatchControlDetails.FirstOrDefault(f => f.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.Return);
                        
                        if(returnDetail != null)
                        {
                            column1 = returnStartColumn;
                            foreach (var product in returnDetail.ProductMovements)
                            {
                                worksheet.Cells[row, column1].Value = product.Quantity;
                                column1++;
                            }

                            worksheet.Cells[row, column1].Value = returnDetail.MissingBottleQuantity;
                            column1++;
                        }

                        if (showTransfers)
                        {
                            var transferNotes = new List<string>();
                            foreach (var transfer in dispatch.DispatchControlDetails.Where(d =>
                                d.IsActive
                                && (d.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.TransferOut
                                    || d.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.TransferIn)))
                            {
                                foreach (var product in transfer.ProductMovements)
                                {
                                    var routeName = transfer.CounterpartRoute ?? string.Empty;
                                    transferNotes.Add(transfer.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.TransferOut
                                        ? "Entregó " + product.Quantity + " " + product.ProductName + " a " + routeName
                                        : "Recibió " + product.Quantity + " " + product.ProductName + " de " + routeName);
                                }
                            }

                            worksheet.Cells[row, transferColumn].Value = string.Join("; ", transferNotes);
                        }
                        row++;
                    }


                    // Autoajustar ancho de columnas
                    worksheet.Cells.AutoFitColumns();

                    // Guardar el archivo
                    package.Save();
                }

                byte[] excelBytes = memoryStream.ToArray();

                return excelBytes;
            }
        }

        private static void CreateGroupHeader(ExcelWorksheet worksheet,int startColumn,int endColumn, string title)
        {
            ExcelRange range = worksheet.Cells[
                2,
                startColumn,
                2,
                endColumn
            ];

            range.Merge = true;
            range.Value = title;
            range.Style.Font.Bold = true;
            range.Style.HorizontalAlignment =
                ExcelHorizontalAlignment.Center;
            range.Style.VerticalAlignment =
                ExcelVerticalAlignment.Center;
        }

        public static Expression<Func<domain.Operation.DispatchControl, bool>> GetDispatchControlByDate(DispatchControlFilterModel filterModel, string timeZone)
        {
            if (filterModel.DateFrom.HasValue && filterModel.DateTo.HasValue)
            {
                filterModel.DateFrom = DateService.GetCLientLocalTime(filterModel.DateFrom.Value, timeZone);
                filterModel.DateTo = DateService.GetCLientLocalTime(filterModel.DateTo.Value, timeZone);
                return e => e.DispatchControlDate.Date >= filterModel.DateFrom.Value.Date && e.DispatchControlDate.Date <= filterModel.DateTo.Value.Date;
            }

            return e => true;
        }
    }
}
