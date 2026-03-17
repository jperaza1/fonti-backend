using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using cod.application.Features.Extensions;
using cod.application.Main;
using cod.application.Main.Attributes;

namespace cod.infraestructure.Services
{
    public class ExcelService : IExcelService
    {
        public List<T> TransformFile<T>(byte[] file, int workSheetNumber, int initialRow)
        {
            if (file == null || file.Length == 0)
                return new List<T>();

            using var fileStream = new MemoryStream(file);

            var listOfDataTransform = new List<T>();

            using ExcelPackage excelPackage = new ExcelPackage(fileStream);
            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[workSheetNumber];

            for (int rowIndex = initialRow; rowIndex <= workSheet.Dimension.Rows; rowIndex++)
            {

                var newRow = Activator.CreateInstance(typeof(T));

                var properties = newRow.GetType().GetProperties();

                foreach (var prop in properties)
                {
                    var attribute = prop.GetCustomAttribute<IndexColumnAttribute>();

                    try
                    {
                        if (!attribute.IsNull())
                            if (!workSheet.Cells[rowIndex, attribute.Index].Value.IsNull())
                                prop.SetValue(newRow,
                                    Convert.ChangeType(workSheet.Cells[rowIndex, attribute.Index].Value, prop.PropertyType));
                    }
                    catch(Exception ex)
                     {

                    }
                }

                listOfDataTransform.Add((T)newRow);

            }

            return listOfDataTransform;

        }
    }
}
