using System;

namespace cod.application.Features.Extensions
{
    public static class ByteExtension
    {
        public static string ConvertToPictureString(this byte[] value)
        {
            return string.Concat("data:image/png;base64,", Convert.ToBase64String(value ?? new byte[] { }));
        }

    }
}
