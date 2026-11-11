using System.Threading.Tasks;

namespace cod.application.Sale.Commands.ConsolidateSale
{
    public interface IConsolidateSaleCommand
    {
        Task Execute(ConsolidateSaleModel model);
    }
}
