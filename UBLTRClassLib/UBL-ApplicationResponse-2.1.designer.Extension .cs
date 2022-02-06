using System.Xml.Serialization;

namespace hm.common.Ubltr.ApplicationResponse21

{
    public partial class ApplicationResponseType
    {
        [XmlAttributeAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string xsiSchemaLocation = @"urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2 UBL-ApplicationResponse-2.1.xsd";
    
        public string SerializeF()
        {
            string s = hm.common.Ubltr.Tools.RemoveTimeZone(
                            hm.common.Ubltr.Tools.XmlSerialize<ApplicationResponseType>(this, hm.common.Ubltr.Tools.ApplicationResponseNamespaces, System.Text.Encoding.UTF8, false)
                        );
            return s;
        }

        public static ApplicationResponseType DeserializeF(string xml)
        {
            ApplicationResponseType t = hm.common.Ubltr.Tools.XmlDeSerialize<ApplicationResponseType>(System.Text.Encoding.UTF8.GetBytes(xml), hm.common.Ubltr.Tools.ApplicationResponseNamespaces, System.Text.Encoding.UTF8);
            return t;
        }
    }
}


