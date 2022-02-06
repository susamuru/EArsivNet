using EArsivNet;
using EArsivNet.Middleware;
using EArsivNet.Models;
using System;

namespace EArsivNetTest
{
    class Program
    {
        static void Main(string[] args)
        {

            CheckInvoice();
            Console.ReadKey();
        }

        static async void Test()
        {
            var inv = new Invoice();
            inv.InvoiceNo = "AA1";
            //inv.OrderNo = "sipariş no";
            inv.InvoiceDate = DateTime.Now;

            inv.Sender.Title = "StoreNate";
            inv.Sender.TaxOffice = "KURUMLAR";
            inv.Sender.TaxNo = "9000068418";
            inv.Sender.Country = "Türkiye";
            inv.Sender.City = "Antalya";
            inv.Sender.District = "Muratpaşa";
            inv.Sender.Street = "Etiler mah, evliya çelebi caddesi no 15";
            inv.Sender.BuildingNumber = "23";
            inv.Sender.Room = "109";

            inv.Receiver.Title = "HDN BILISIM AS";
            inv.Receiver.TaxOffice = "KURUMLAR";
            inv.Receiver.TaxNo = "1231231";
            inv.Receiver.Country = "Türkiye";
            inv.Receiver.City = "Antalya";
            inv.Receiver.District = "Kepez";
            inv.Receiver.Street = "sokak";
            inv.Receiver.BuildingNumber = "36";
            inv.Receiver.Room = "1";

            inv.Lines.Add(new InvoiceLine { Name = "Ürün Adı 1", Price = 100, Qty = 10, TaxRate = 0, DiscountRate = 10, UnitType = UnitType.Unit, Description = "Açıklama" });
            inv.Lines.Add(new InvoiceLine { Name = "Ürün Adı 2", Price = 100, Qty = 1, TaxRate = 18, DiscountRate = 0, UnitType = UnitType.Unit, Description = "Açıklama" });

            IInvoiceClient uyumsoft = new UyumsoftService("Uyumsoft", "Uyumsoft", PlatformType.Test);

            var re = await uyumsoft.SendInvoice(inv);
            if (re.State == ResponseState.Error)
            {
                Console.WriteLine("Hata : " + re.Message);
            }
            else
            {
                Console.WriteLine("Invoice Id : " + re.Data.InvoiceId);
                Console.WriteLine("Invoice No : " + re.Data.InvoiceNo);
            }
        }

        static async void CheckInvoice()
        {
            var id = "47887b3e-e5db-4076-84b6-cc3edab02211";
            IInvoiceClient uyumsoft = new UyumsoftService("Uyumsoft", "Uyumsoft", PlatformType.Test);
            await uyumsoft.CheckInvoice(id);
        }
    }
}
