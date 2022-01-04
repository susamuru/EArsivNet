using EArsivNet.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArsivNet.Models
{
    public class Response
    {
        public ResponseState State { get; set; }

        public string Message { get; set; }

        public void InitError(Exception exception)
        {
            this.State = ResponseState.Error;
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            Message = string.Join(Environment.NewLine, messages);
        }

        public void ThrowIfException()
        {
            if (this.State == ResponseState.Error)
                throw new Exception(this.Message);
        }

       
    }

    public class Response<T> : Response
    {
        public T Data { get; set; }
    }

    public enum ResponseState
    {
        Success,
        Error
    }
}
