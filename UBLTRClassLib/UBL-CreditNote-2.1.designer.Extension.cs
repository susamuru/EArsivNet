using System.Xml.Serialization;

namespace hm.common.Ubltr.CreditNote21
{
    public partial class CreditNoteType
    {
        [XmlAttributeAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string xsiSchemaLocation = @"urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2 UBL-CreditNote-2.1.xsd";
    
        public string SerializeF()
        {
            string s = hm.common.Ubltr.Tools.RemoveTimeZone(
                            hm.common.Ubltr.Tools.XmlSerialize<CreditNoteType>(this, hm.common.Ubltr.Tools.CreditNoteNamespaces, System.Text.Encoding.UTF8, false)
                        );
            return s;
        }

        public static CreditNoteType DeserializeF(string xml)
        {
            CreditNoteType t = hm.common.Ubltr.Tools.XmlDeSerialize<CreditNoteType>(System.Text.Encoding.UTF8.GetBytes(xml), hm.common.Ubltr.Tools.CreditNoteNamespaces, System.Text.Encoding.UTF8);
            return t;
        }
    }
}


