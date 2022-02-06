using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace hm.common.Ubltr
{
    public class Tools
    {

        private static Dictionary<string, XmlSerializer> XmlSerializers = new Dictionary<string, XmlSerializer>();
        private static Regex timeZoneRegex = new Regex(@"(\d{2}:\d{2}:\d{2})(\.\d{7}[\+|-]\d{2}:\d{2})");
        private static readonly Object locker = new Object();

        public static System.Xml.Serialization.XmlSerializerNamespaces InvoiceNamespaces
        {
            get
            {
                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();

                ns.Add("", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
                ns.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                ns.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
                //ns.Add("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
                //ns.Add("ubltr", "urn:oasis:names:specification:ubl:schema:xsd:TurkishCustomizationExtensionComponents");
                //ns.Add("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
                ns.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                ns.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                //ns.Add("ccts", "urn:un:unece:uncefact:documentation:2");
                ns.Add("ds", "http://www.w3.org/2000/09/xmldsig#");

                return ns;
            }
        }

        public static System.Xml.Serialization.XmlSerializerNamespaces ApplicationResponseNamespaces
        {
            get
            {
                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();

                ns.Add("", "urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2");
                ns.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                ns.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
                //ns.Add("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
                //ns.Add("ubltr", "urn:oasis:names:specification:ubl:schema:xsd:TurkishCustomizationExtensionComponents");
                //ns.Add("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
                ns.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                ns.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                //ns.Add("ccts", "urn:un:unece:uncefact:documentation:2");
                ns.Add("ds", "http://www.w3.org/2000/09/xmldsig#");

                return ns;
            }
        }

        public static System.Xml.Serialization.XmlSerializerNamespaces DespatchAdviceNamespaces
        {
            get
            {
                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();

                ns.Add("", "urn:oasis:names:specification:ubl:schema:xsd:DespatchAdvice-2");
                ns.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                ns.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
                //ns.Add("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
                //ns.Add("ubltr", "urn:oasis:names:specification:ubl:schema:xsd:TurkishCustomizationExtensionComponents");
                //ns.Add("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
                ns.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                ns.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                //ns.Add("ccts", "urn:un:unece:uncefact:documentation:2");
                ns.Add("ds", "http://www.w3.org/2000/09/xmldsig#");

                return ns;
            }
        }

        public static System.Xml.Serialization.XmlSerializerNamespaces ReceiptAdviceNamespaces
        {
            get
            {
                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();

                ns.Add("", "urn:oasis:names:specification:ubl:schema:xsd:ReceiptAdvice-2");
                ns.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                ns.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
                //ns.Add("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
                //ns.Add("ubltr", "urn:oasis:names:specification:ubl:schema:xsd:TurkishCustomizationExtensionComponents");
                //ns.Add("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
                ns.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                ns.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                //ns.Add("ccts", "urn:un:unece:uncefact:documentation:2");
                ns.Add("ds", "http://www.w3.org/2000/09/xmldsig#");

                return ns;
            }
        }

        public static System.Xml.Serialization.XmlSerializerNamespaces CreditNoteNamespaces
        {
            get
            {
                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();

                ns.Add("", "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2");
                ns.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                ns.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
                //ns.Add("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
                //ns.Add("ubltr", "urn:oasis:names:specification:ubl:schema:xsd:TurkishCustomizationExtensionComponents");
                //ns.Add("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
                ns.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                ns.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                //ns.Add("ccts", "urn:un:unece:uncefact:documentation:2");
                ns.Add("ds", "http://www.w3.org/2000/09/xmldsig#");

                return ns;
            }
        }

        public static string RemoveTimeZone(string inputXml)
        {
            inputXml = timeZoneRegex.Replace(inputXml, "$1");
            return inputXml;
        }

        public static T XmlDeSerialize<T>(byte[] item, XmlSerializerNamespaces ns, Encoding encoding)
        {
            #region get XmlSerializer

            XmlSerializer xmlSerializer = null;

            if (!XmlSerializers.Keys.Contains(typeof(T).FullName))
            {
                lock (locker)
                {
                    if (!XmlSerializers.Keys.Contains(typeof(T).FullName))
                    {
                        XmlSerializers.Add(typeof(T).FullName, new XmlSerializer(typeof(T)));
                    }
                }
            }

            xmlSerializer = XmlSerializers[typeof(T).FullName];

            #endregion

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            MemoryStream memoryStream = new MemoryStream(item);
            StreamReader reader = new StreamReader(memoryStream, encoding);
            return (T)xmlSerializer.Deserialize(reader);
        }

        public static string XmlSerialize<T>(T item, XmlSerializerNamespaces ns, Encoding encoding, bool heading)
        {
            #region get XmlSerializer

            XmlSerializer xmlSerializer = null;

            if (!XmlSerializers.Keys.Contains(typeof(T).FullName))
            {
                lock (locker)
                {
                    if (!XmlSerializers.Keys.Contains(typeof(T).FullName))
                    {
                        XmlSerializers.Add(typeof(T).FullName, new XmlSerializer(typeof(T)));
                    }
                }
            }

            xmlSerializer = XmlSerializers[typeof(T).FullName];

            #endregion

            var xmlString = "";

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }


            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                    {
                        Encoding = encoding,
                        OmitXmlDeclaration = !heading,
                    };

                    using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter, xmlWriterSettings))
                    {
                        xmlSerializer.Serialize(xmlWriter, item, ns);

                        xmlWriter.Flush();
                        xmlString = xmlWriter.ToString();
                        xmlWriter.Close();
                    }

                    using (StreamReader streamReader = new StreamReader(memoryStream))
                    {
                        memoryStream.Position = 0;
                        xmlString = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                }
            }

            //using (StringWriter stringWriter = new StringWriter())
            //{

            //    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            //    {
            //        Encoding = Encoding.UTF8
            //    };

            //    using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
            //    {
            //        xmlSerializer.Serialize(xmlWriter, item, ns);
            //    }
            //    xmlString = stringWriter.ToString();
            //}

            return xmlString;
        }



    }
}
