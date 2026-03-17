using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Main
{
    public interface IExcelService
    {
        List<T> TransformFile<T>(byte[] file, int workSheetNumber, int initialRow);
    }
}
