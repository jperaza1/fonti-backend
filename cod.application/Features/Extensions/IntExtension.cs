namespace cod.application.Features.Extensions
{
    public static class IntExtension
    {
        public static bool IsValid(this int value)
        {
            return value > 0;
        }
        public static bool IsValid(this int? value)
        {
            return value != null && value > 0;
        }
    }
}