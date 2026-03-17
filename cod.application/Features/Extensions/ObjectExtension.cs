namespace cod.application.Features.Extensions
{
    public static class ObjectExtension
    {
        public static bool IsNull<T>(this T value)
        {
            return value == null;
        }
    }
}
