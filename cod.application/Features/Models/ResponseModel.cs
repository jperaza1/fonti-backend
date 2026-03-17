using System;
using System.Collections.Generic;

namespace cod.application.Features.Models
{
    [Serializable]
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string MessageResponse { get; set; }
        public string Title { get; set; }
        public int Identity { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }
    [Serializable]
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string MessageResponse { get; set; }
        public string Title { get; set; }
        public int Identity { get; set; }
        public T Data { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
