using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Interfaces
{
    public interface IHttpClientService
    {
        Task<T> PostResult<T>(string urlClient, object modelo);
        Task<T> GetResult<T>(string urlClient);
        void NewInstance();
        void AddHeader(string key, string value);
        void AddHeaderAutorization(string key, string value);
    }
}
