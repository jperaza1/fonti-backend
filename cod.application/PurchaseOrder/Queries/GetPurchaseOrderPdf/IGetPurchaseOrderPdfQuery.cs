using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPurchaseOrderPdf
{
    public interface IGetPurchaseOrderPdfQuery
    {
        Task<string> Execute(int id, bool hideCost);
    }
}
