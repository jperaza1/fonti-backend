using cod.application.ProductRecipe.Common;
using System;

namespace cod.application.ProductRecipe
{
    public static class InventoryQuantity
    {
        public const int Scale = 6;

        public static decimal Round(decimal value)
        {
            return Math.Round(value, Scale, MidpointRounding.AwayFromZero);
        }

        public static decimal PerFinishedUnit(decimal unitsPerPackage, ProductRecipeDetailModel detail)
        {
            if (detail.PerPackage)
                return Round(detail.Quantity);

            return Round(detail.Quantity * unitsPerPackage / detail.ProducedUnits);
        }

        public static decimal ForFinishedUnits(decimal finishedUnits, decimal unitsPerPackage, decimal quantity, decimal producedUnits, bool perPackage)
        {
            if (perPackage)
                return Round(finishedUnits * quantity);

            return Round(finishedUnits * quantity * unitsPerPackage / producedUnits);
        }
    }
}
