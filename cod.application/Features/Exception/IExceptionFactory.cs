using System.Collections.Generic;

namespace cod.application.Features.Exception
{
    public interface IExceptionFactory
    {
        Dictionary<string, List<string>> Exceptions { get; }
        void Add(string key, string value);
        string GetMessage(string key, ExceptionEnums.ExceptionCode code);
        bool IsValid();
    }
}
