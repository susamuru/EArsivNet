using EArsivNet;
using EDMWs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EArsivNet.Middleware
{
    public class EdmService : IInvoiceClient
    {
        private readonly EDMWs.EFaturaEDMPortClient client;
        private readonly UyumsoftWS.UserInformation userInformation;
        private readonly string username;
        private readonly string password;

        private REQUEST_HEADERType header { get; set; }

        private static Regex timeZoneRegex = new Regex(@"(\d{2}:\d{2}:\d{2})(\.\d{7}[\+|-]\d{2}:\d{2})");

        public EdmService(string username, string password, Models.PlatformType platform)
        {
            string uri = platform == Models.PlatformType.Test
                ? "https://test.edmbilisim.com.tr/EFaturaEDM21ea/EFaturaEDM.svc"
                : "https://live.edmbilisim.com.tr/EFaturaEDM21ea/EFaturaEDM.svc";
            var bind = new BasicHttpBinding();
            var end = new EndpointAddress(uri);
            client = new EDMWs.EFaturaEDMPortClient(bind, end);
            this.username = username;
            this.password = password;
        }

        private void Login()
        {
            var res = client.Login(new EDMWs.LoginRequest { USER_NAME = username, PASSWORD = password });
            header.SESSION_ID = res.SESSION_ID;
        }

        private string GetInvoiceType(EArsivNet.Models.InvoiceType invoiceType)
        {
            string re = string.Empty;
            switch (invoiceType)
            {
                case Models.InvoiceType.Sales:
                    re = "SATIS";
                    break;
                case Models.InvoiceType.Return:
                    re = "IADE";
                    break;
                default:
                    break;
            }
            return re;
        }

        private INVOICE CreateInvoice(Models.Invoice inv)
        {
            INVOICE row = new INVOICE();
            row.HEADER = new INVOICEHEADER();
            row.HEADER.SENDER = inv.Sender.TaxNo;
            row.HEADER.FROM = inv.Sender.Email;
            row.HEADER.RECEIVER = inv.Receiver.TaxNo;
            row.HEADER.TO = inv.Receiver.Email;
            //row.HEADER.MOBILE = inv.Receiver.Phone;

            hm.common.Ubltr.Invoice21.InvoiceType ublinvoice = new hm.common.Ubltr.Invoice21.InvoiceType();

            //fatura bilgileri basılacak

            string invoiceUblXmlStr = ublinvoice.SerializeF();
            invoiceUblXmlStr = timeZoneRegex.Replace(invoiceUblXmlStr, "$1");
            var ublfilebytes = System.Text.Encoding.UTF8.GetBytes(invoiceUblXmlStr);

            row.CONTENT = new base64Binary()
            {
                Value = ublfilebytes
            };

            return row;
        }



        async Task<Models.Response<Models.SendInvoiceRes>> IInvoiceClient.SendInvoice(Models.Invoice inv)
        {

            var re = new Models.Response<Models.SendInvoiceRes>();
            re.Data = new Models.SendInvoiceRes();
            try
            {
                var invoice = CreateInvoice(inv);

                SendInvoiceRequest sendInvoiceRequest = new SendInvoiceRequest();

                sendInvoiceRequest.REQUEST_HEADER = header;

                sendInvoiceRequest.RECEIVER = new SendInvoiceRequestRECEIVER()
                {
                    vkn = invoice.HEADER.RECEIVER,
                    alias = invoice.HEADER.TO,
                };
                sendInvoiceRequest.INVOICE = new INVOICE[] { invoice };

                var res = await client.SendInvoiceAsync(sendInvoiceRequest);

                re.Data.InvoiceId = res.SendInvoiceResponse.INVOICE[0].ID;
                re.Data.InvoiceNo = res.SendInvoiceResponse.INVOICE[0].ID;
            }
            catch (Exception ex)
            {
                re.InitError(ex);
            }
            return re;

        }

        async Task<Models.Response<Models.CheckInvoiceRes>> IInvoiceClient.CheckInvoice(string uuid)
        {
            var re = new Models.Response<Models.CheckInvoiceRes>();
            try
            {

                //var res = await client.QueryOutboxInvoiceStatusAsync(userInformation, new string[] { uuid });
                //if (res.IsSucceded)
                //{
                //    var val = res.Value.Where(x => x.InvoiceId == uuid).FirstOrDefault();
                //    if (!string.IsNullOrEmpty(val.Message))
                //    {
                //        re.Message = $"{val.Status} : {val.Message}";
                //    }
                //    else
                //    {
                //        re.Message = val.Status.ToString();
                //    }
                //}
                //else
                //{
                //    throw new Exception(res.Message);
                //}

            }
            catch (Exception ex)
            {
                re.InitError(ex);
            }

            return re;
        }
    }
}
