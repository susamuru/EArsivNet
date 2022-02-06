using EArsivNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArsivNet
{
    public interface IInvoiceClient
    {
        public Task<Response<SendInvoiceRes>> SendInvoice(Invoice inv);

        public Task<Response<CheckInvoiceRes>> CheckInvoice(string uuid);
    }
}
