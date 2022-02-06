using System.Xml.Serialization;

namespace hm.common.Ubltr.ReceiptAdvice21

{
    public partial class ReceiptAdviceType
    {
        [XmlAttributeAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string xsiSchemaLocation = @"urn:oasis:names:specification:ubl:schema:xsd:ReceiptAdvice-2 UBL-ReceiptAdvice-2.1.xsd";
    
        public string SerializeF()
        {
            string s = hm.common.Ubltr.Tools.RemoveTimeZone(
                            hm.common.Ubltr.Tools.XmlSerialize<ReceiptAdviceType>(this, hm.common.Ubltr.Tools.ReceiptAdviceNamespaces, System.Text.Encoding.UTF8, false)
                        );
            return s;
        }

        public static ReceiptAdviceType DeserializeF(string xml)
        {
            ReceiptAdviceType t = hm.common.Ubltr.Tools.XmlDeSerialize<ReceiptAdviceType>(System.Text.Encoding.UTF8.GetBytes(xml), hm.common.Ubltr.Tools.ReceiptAdviceNamespaces, System.Text.Encoding.UTF8);
            return t;
        }
    }
}


