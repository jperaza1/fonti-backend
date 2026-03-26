using System.Threading.Tasks;

namespace cod.application.Correlative.Commands.EnableCorrelative
{
    public interface IEnableCorrelativeCommand
    {
        Task Execute(int id);
    }
}
