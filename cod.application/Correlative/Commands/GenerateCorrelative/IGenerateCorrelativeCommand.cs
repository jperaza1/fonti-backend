using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Correlative.Commands.GenerateCorrelative
{
    public interface IGenerateCorrelativeCommand
    {
        Task<GenerateCorrelativeModel> Execute();
    }
}
