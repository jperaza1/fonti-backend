using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.DispatchControl.Commom
{
    public static class DispatchQuantity
    {
        public static bool IsTransfer(DispatchControlType type)
        {
            return type == DispatchControlType.TransferOut || type == DispatchControlType.TransferIn;
        }

        public static int Sign(DispatchControlType type)
        {
            switch (type)
            {
                case DispatchControlType.Exit:
                case DispatchControlType.Reload:
                case DispatchControlType.TransferIn:
                    return 1;
                case DispatchControlType.Return:
                case DispatchControlType.TransferOut:
                    return -1;
                default:
                    return 0;
            }
        }
    }
}
