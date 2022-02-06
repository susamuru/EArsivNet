using System.Xml.Serialization;

namespace hm.common.Ubltr.Invoice21

{
    public partial class InvoiceType
    {
        [XmlAttributeAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string xsiSchemaLocation = @"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2 UBL-Invoice-2.1.xsd";
    
        public string SerializeF()
        {
            string s = hm.common.Ubltr.Tools.RemoveTimeZone(
                            hm.common.Ubltr.Tools.XmlSerialize<InvoiceType>(this, hm.common.Ubltr.Tools.InvoiceNamespaces, System.Text.Encoding.UTF8, false)
                        );
            return s;
        }

        public static InvoiceType DeserializeF(string xml)
        {
            InvoiceType t = hm.common.Ubltr.Tools.XmlDeSerialize<InvoiceType>(System.Text.Encoding.UTF8.GetBytes(xml), hm.common.Ubltr.Tools.InvoiceNamespaces, System.Text.Encoding.UTF8);
            return t;
        }
    }
}


