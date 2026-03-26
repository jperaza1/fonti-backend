using System;

namespace cod.application.Correlative.Common
{
    public class CorrelativeModel
    {
        public int Id { get; set; }
        public string Cai { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ulong Current { get; set; }
        public string EmissionPoint { get; set; }
        public string Establishment { get; set; }
        public string DocumentType { get; set; }
        public bool IsActive { get; set; }
    }
}
