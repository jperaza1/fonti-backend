namespace cod.application.Production
{
    public static class ProductionShift
    {
        public const string GroupCode = "PRODUCTION_SHIFT";

        public static string Status(bool isActive, bool isClosed)
        {
            if (!isActive)
                return "Anulado";

            return isClosed ? "Cerrado" : "Abierto";
        }
    }
}
