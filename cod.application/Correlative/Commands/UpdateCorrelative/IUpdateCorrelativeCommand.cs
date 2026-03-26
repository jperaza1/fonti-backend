using cod.application.Correlative.Common;
using System.Threading.Tasks;

namespace cod.application.Correlative.Commands.UpdateCorrelative
{
    public interface IUpdateCorrelativeCommand
    {
        Task Execute(CorrelativeModel model, int id);
    }
}
