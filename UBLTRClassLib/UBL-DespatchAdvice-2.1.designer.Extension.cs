using System.Xml.Serialization;

namespace hm.common.Ubltr.DespatchAdvice21

{
    public partial class DespatchAdviceType
    {
        [XmlAttributeAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string xsiSchemaLocation = @"urn:oasis:names:specification:ubl:schema:xsd:DespatchAdvice-2 UBL-DespatchAdvice-2.1.xsd";
    
        public string SerializeF()
        {
            string s = hm.common.Ubltr.Tools.RemoveTimeZone(
                            hm.common.Ubltr.Tools.XmlSerialize<DespatchAdviceType>(this, hm.common.Ubltr.Tools.DespatchAdviceNamespaces, System.Text.Encoding.UTF8, false)
                        );
            return s;
        }

        public static DespatchAdviceType DeserializeF(string xml)
        {
            DespatchAdviceType t = hm.common.Ubltr.Tools.XmlDeSerialize<DespatchAdviceType>(System.Text.Encoding.UTF8.GetBytes(xml), hm.common.Ubltr.Tools.DespatchAdviceNamespaces, System.Text.Encoding.UTF8);
            return t;
        }
    }
}


