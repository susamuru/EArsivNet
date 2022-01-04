using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArsivNet.Models
{
    public class Invoice
    {
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public InvoiceType InvoiceType { get; set; }
        public string InvoiceNote { get; set; }
        public string OrderNo { get; set; }
        public InvoiceInfo Sender { get; set; } = new InvoiceInfo();
        public InvoiceInfo Receiver { get; set; } = new InvoiceInfo();
        public List<InvoiceLine> Lines { get; } = new List<InvoiceLine>();

        public decimal Price { get { return Lines.Sum(x => x.Price * x.Qty); } }
        public decimal DiscountedPrice { get { return Lines.Sum(x => x.DiscountedPrice * x.Qty); } }
        public decimal TaxPrice { get { return Lines.Sum(x => x.TaxPrice); } }
        public decimal TotalPrice { get { return DiscountedPrice + TaxPrice; } }

        public TaxExemption TaxExemption { get; set; } = new TaxExemption();
    }

    public class InvoiceInfo
    {
        public string Title { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string Room { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string BuildingNumber { get;  set; }
    }

    public class InvoiceLine
    {
        public string Name { get; set; }
        public UnitType UnitType { get; set; } = UnitType.Unit;
        public int Qty { get; set; } = 1;
        public decimal Price { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountedPrice { get { return Price * (100 - DiscountRate) / 100m; } }
        public decimal TaxRate { get; set; }
        public decimal TaxPrice { get { return DiscountedPrice * Qty * TaxRate / 100m; } }
        public decimal TotalPrice { get { return DiscountedPrice * Qty; } }
        public string Description { get; set; }
        
    }

    public class TaxExemption
    {
        public string Code { get; set; }
        public string Reason { get; set; }
    }


}
