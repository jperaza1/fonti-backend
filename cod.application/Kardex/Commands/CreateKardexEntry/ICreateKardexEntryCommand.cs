using cod.application.Kardex.Common;
using System.Threading.Tasks;

namespace cod.application.Kardex.Commands.CreateKardexEntry
{
    public interface ICreateKardexEntryCommand
    {
        Task Execute(KardexEntryModel model);
    }
}
