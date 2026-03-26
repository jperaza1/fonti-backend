using System.Threading.Tasks;

namespace cod.application.Correlative.Commands.DisableCorrelative
{
    public interface IDisableCorrelativeCommand
    {
        Task Execute(int id);
    }
}
