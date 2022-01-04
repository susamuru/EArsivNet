# EArsivNet
 eArsiv sistemlerinin tek çatı altında toplanması amaçlanmıştır.

# Örnek Kullanım
```csharp
var inv = new Invoice();
inv.OrderNo = "sipariş no";
inv.InvoiceDate = DateTime.Now;

inv.Sender.Title = "Uyumsoft";
inv.Sender.TaxOffice = "KURUMLAR";
inv.Sender.TaxNo = "9000068418";
inv.Sender.Country = "Türkiye";
inv.Sender.City = "Antalya";
inv.Sender.District = "Muratpaşa";
inv.Sender.Street = "sokak";
inv.Sender.BuildingNumber = "23";
inv.Sender.Room = "109";


inv.Receiver.Title = "Fatura başlığı";
inv.Receiver.TaxOffice = "KURUMLAR";
inv.Receiver.TaxNo = "1111111111";
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
```
