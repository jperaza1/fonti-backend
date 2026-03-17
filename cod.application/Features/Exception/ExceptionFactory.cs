using System.Collections.Generic;
using System.Linq;

namespace cod.application.Features.Exception
{
    public class ExceptionFactory : IExceptionFactory
    {
        public Dictionary<string, List<string>> Exceptions { get; }

        public ExceptionFactory()
        {
            Exceptions = new Dictionary<string, List<string>>();
        }
        public void Add(string key, string value)
        {
            if (Exceptions.Any(c => c.Key == key))
            {
                var oldValue = this.Exceptions[key];
                oldValue.Add(value);
                Exceptions[key] = oldValue;
            }
            else
            {
                this.Exceptions.Add(key, new List<string> { value });
            }
        }

        public bool IsValid()
        {
            return !Exceptions.Any();
        }

        public string GetMessage(string key, ExceptionEnums.ExceptionCode code)
        {
            switch (code)
            {
                case ExceptionEnums.ExceptionCode.Required:
                    return $"{key.ToUpper()} IS REQUIRED";
                case ExceptionEnums.ExceptionCode.Duplicate:
                    return $"THERE IS ALREADY A REGISTRATION WITH THIS SPECIFICATION IN THIS FIELD ({key.ToUpper()}).";
                case ExceptionEnums.ExceptionCode.EmailDuplicate:
                    return $"EMAIL IN USE.";
                case ExceptionEnums.ExceptionCode.Mask:
                    return $"INVALID FORMAT. PLEASE ENTER A VALID VALUE IN THIS FIELD {key.ToUpper()}";
                default:
                    return key;
            }
        }

    }
}
