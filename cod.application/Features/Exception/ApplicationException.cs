using System;
using System.Collections.Generic;
using System.Net;

namespace cod.application.Features.Exception
{
    [Serializable]
    public class ApplicationException : System.Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string StatusCode { get; set; }
        public bool Success { get; set; }
        public string MessageResponse { get; set; }
        public string Title { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
        public ApplicationException()
        { }

        public ApplicationException(string message) : base(message)
        {
            this.Success = false;
            this.MessageResponse = message;
        }
        public ApplicationException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            this.Success = false;
            this.HttpStatusCode = httpStatusCode;
            this.MessageResponse = message;
        }
        public ApplicationException(string message, string title, string statusCode) : base(message)
        {
            this.Success = false;
            this.StatusCode = statusCode;
            this.Title = title;
            this.MessageResponse = message;
        }
        public ApplicationException(string message, string title) : base(message)
        {
            this.Success = false;
            this.Title = title;
            this.MessageResponse = message;
        }
        public ApplicationException(string message, string title, Dictionary<string, List<string>> errors) : base(message)
        {
            this.Success = false;
            this.Title = title;
            this.Errors = errors;
            this.MessageResponse = message;
        }
    }
}
