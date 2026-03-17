using System;
using System.Collections.Generic;
using System.Text;

namespace cod.domain.Features
{
    public static class Clock
    {
        public static bool Utc = false;
        public static DateTime Now => Utc ? DateTime.UtcNow : DateTime.Now;
    }
}
