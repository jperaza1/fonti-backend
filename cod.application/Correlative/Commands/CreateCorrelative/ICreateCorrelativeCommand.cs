using cod.application.Correlative.Common;
using System.Threading.Tasks;

namespace cod.application.Correlative.Commands.CreateCorrelative
{
    public interface ICreateCorrelativeCommand
    {
        Task Execute(CorrelativeModel model);
    }
}
