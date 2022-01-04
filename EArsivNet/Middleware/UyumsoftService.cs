using EArsivNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UyumsoftWS;

namespace EArsivNet.Middleware
{
    public class UyumsoftService : IInvoiceClient
    {
        private readonly UyumsoftWS.BasicIntegrationClient client;
        private readonly UyumsoftWS.UserInformation userInformation;

        public UyumsoftService(string username, string password, Models.PlatformType platform)
        {
            string uri = platform == Models.PlatformType.Test
                ? "http://efatura-test.uyumsoft.com.tr/Services/BasicIntegration"
                : "http://efatura.uyumsoft.com.tr/Services/BasicIntegration";
            var bind = new BasicHttpBinding();
            var end = new EndpointAddress(uri);
            client = new UyumsoftWS.BasicIntegrationClient(bind, end);
            client.ClientCredentials.UserName.UserName = username;
            client.ClientCredentials.UserName.Password = password;

            userInformation = new UserInformation() { Username = username, Password = password };
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

        private InvoiceInfo CreateInvoice(Models.Invoice inv)
        {
            var row = new InvoiceType();
            {
                #region Genel Fatura Bilgileri

                row.ProfileID = new ProfileIDType { Value = "TICARIFATURA" };

                row.CopyIndicator = new CopyIndicatorType { Value = false };
                row.UUID = new UUIDType { Value = Guid.NewGuid().ToString() }; //Set edilmediğinde sistem tarafından otomatik verilir.
                row.IssueDate = new IssueDateType { Value = inv.InvoiceDate };
                row.IssueTime = new IssueTimeType { Value = inv.InvoiceDate };
                row.InvoiceTypeCode = new InvoiceTypeCodeType { Value = inv.Lines.Any(x => x.TaxRate > 0) ? GetInvoiceType(inv.InvoiceType) : "ISTISNA" };
                row.Note = new NoteType[] { new NoteType { Value = inv.InvoiceNote } };
                row.DocumentCurrencyCode = new DocumentCurrencyCodeType { Value = "TRY" };
                row.PricingCurrencyCode = new PricingCurrencyCodeType { Value = "TRY" };
                row.LineCountNumeric = new LineCountNumericType { Value = 2 };
                //PaymentTerms = new PaymentTermsType { Note = new NoteType { Value = "30 gün vadeli" }, Amount = new AmountType1 { Value = 100, currencyID = "TRY" } },
                //PaymentMeans = new PaymentMeansType[] { new PaymentMeansType { PaymentDueDate = new PaymentDueDateType { Value = DateTime.Now.AddDays(15) }, PaymentMeansCode = new PaymentMeansCodeType1 { Value = "42" } } },
                //Delivery = new DeliveryType { DeliveryParty = new PartyType { };
                // PricingExchangeRate = new ExchangeRateType{ SourceCurrencyCode= "TRY",}
                #endregion

                #region SGK fatura alanları
                //AccountingCost = cmbInvoicetypeCode.Text == "SGK" ? new AccountingCostType { Value = cmbSgkInvoicetype.Text } : null,
                //InvoicePeriod = new PeriodType { StartDate = new StartDateType { Value = dpInvoicePeriodStart.Value }, EndDate = new EndDateType { Value = dpInvoicePeriodEnd.Value } },
                #endregion



                //AllowanceCharge = new AllowanceChargeType[]
                //{
                //    new AllowanceChargeType { ChargeIndicator= new ChargeIndicatorType { Value=true }, Amount = new AmountType2 { currencyID="TRY",Value=100 }, AllowanceChargeReason = new AllowanceChargeReasonType { Value= "Bayi İskontosu" },   }
                //},

                //BillingReference = new BillingReferenceType {   BillingReferenceLine = new BillingReferenceLineType[] { new BillingReferenceLineType {  } } }

                //AllowanceCharge = new AllowanceChargeType[] { new AllowanceChargeType { AllowanceChargeReason="Sigorta", ChargeIndicator = true },  }

                #region İrsaliye Bilgileri
                //Irsaliye dosyasi               
                //DespatchDocumentReference = new DocumentReferenceType[]{ new DocumentReferenceType{IssueDate= new IssueDateType{ Value=DateTime.Now},  DocumentType= new DocumentTypeType{  Value = "Irsaliye" }, ID= new IDType{Value="IRS000000001"}},
                //                                                         new DocumentReferenceType{IssueDate= new IssueDateType{ Value=DateTime.Now},  DocumentType= new DocumentTypeType{  Value = "Irsaliye" }, ID= new IDType{Value="IRS000000002"}}},

                #endregion

                #region Xslt ve Ek belgeler
                //Fatura içerisinde görünüm dosyasını set etme. Değer geçilmediğinde varsayılan xslt kullanılır. 
                //AdditionalDocumentReference = GetXsltAndDocuments(),

                ////               AdditionalDocumentReference = new DocumentReferenceType { DocumentType= new DocumentTypeType{ Value="SATINALAMA BELGESİ"}, IssueDate=new IssueDateType{ Value= DateTime.Now},ID= new IDType{ Value="12345"}};
                //#endregion

                //#region Additional Document Reference
                //new DocumentReferenceType[]{
                //    new  DocumentReferenceType {
                //    ID = new IDType{ Value = new Guid().ToString()},
                //    IssueDate = new IssueDateType{ Value = DateTime.Now},
                //    Attachment= new AttachmentType{ 
                //                                    EmbeddedDocumentBinaryObject= new EmbeddedDocumentBinaryObjectType{ 
                //                                                                                                       filename="customxslt.xslt", 
                //                                                                                                        encodingCode= "Base64",
                //                                                                                                         mimeCode= BinaryObjectMimeCodeContentType.applicationxml,
                //                                                                                                        format="", 
                //                                                                                                        characterSetCode="UTF-8",
                //                                                                                                        Value = Encoding.UTF8.GetBytes(Properties.Resources.xslt) }}},


                // },
                #endregion

                #region Order Document Reference
                if (!string.IsNullOrEmpty(inv.OrderNo))
                {
                    row.OrderReference = new OrderReferenceType { ID = new IDType { Value = inv.OrderNo }, IssueDate = new IssueDateType { Value = DateTime.Now } };
                }
                #endregion

                #region Fatura Seri ve numarası
                if (!string.IsNullOrEmpty(inv.InvoiceNo))
                {
                    row.ID = new IDType { Value = inv.InvoiceNo }; //Set edilmediğinde sistem tarafından otomatik verilir. 
                }
                #endregion


                #region Gönderici Bilgileri - AccountingSupplierParty

                row.AccountingSupplierParty = new SupplierPartyType
                {

                    Party = new PartyType
                    {
                        PartyName = new PartyNameType { Name = new NameType1 { Value = inv.Sender.Title } },
                        PartyIdentification = new PartyIdentificationType[] { new PartyIdentificationType() { ID = new IDType { Value = inv.Sender.TaxNo, schemeID = "VKN" } }, },

                        PostalAddress = new AddressType
                        {
                            CityName = new CityNameType { Value = inv.Sender.City },
                            StreetName = new StreetNameType { Value = inv.Sender.Street },
                            Country = new CountryType { Name = new NameType1 { Value = inv.Sender.Country } },
                            Room = new RoomType { Value = inv.Sender.Room },
                            BuildingNumber = new BuildingNumberType { Value = inv.Sender.BuildingNumber },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = inv.Sender.District },
                        },
                        //PartyIdentification = new PartyIdentificationType[] { new PartyIdentificationType() { ID = new IDType { Value = "77777777701", schemeID = "TCKN" } } },
                        //Person = new PersonType{ FirstName= new FirstNameType{ Value="Ahmet"}, FamilyName= new FamilyNameType{ Value="Altınordu"} },
                        //PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = "Esenler" } } },
                        PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = inv.Sender.TaxOffice } } },
                    }
                };
                #endregion

                #region Alıcı Bilgileri

                row.AccountingCustomerParty = new CustomerPartyType()
                {
                    Party = new PartyType
                    {
                        PartyName = new PartyNameType { Name = new NameType1 { Value = inv.Receiver.Title } },
                        PartyIdentification = new PartyIdentificationType[] { new PartyIdentificationType { ID = new IDType { Value = inv.Receiver.TaxNo, schemeID = inv.Receiver.TaxNo.Length == 10 ? "VKN" : "TCKN" } } },
                        PostalAddress = new AddressType
                        {
                            CityName = new CityNameType { Value = inv.Receiver.City },
                            StreetName = new StreetNameType { Value = inv.Receiver.Street },
                            Country = new CountryType { Name = new NameType1 { Value = inv.Receiver.Country } },
                            Room = new RoomType { Value = inv.Receiver.Room },
                            BuildingNumber = new BuildingNumberType { Value = inv.Receiver.BuildingNumber },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = inv.Receiver.District },
                        },
                        Contact = new ContactType { ElectronicMail = new ElectronicMailType { Value = inv.Receiver.Email }, Telephone = new TelephoneType { Value = inv.Receiver.Phone } },
                        WebsiteURI = new WebsiteURIType { Value = inv.Receiver.Website },
                        PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = inv.Receiver.TaxOffice } } },
                        Person = inv.Receiver.TaxNo.Length == 11
                        ? new PersonType { FirstName = new FirstNameType { Value = inv.Receiver.Title } }
                        : null
                    }
                };


                #endregion

                //row.BuyerCustomerParty = GetBuyerCustomerParty(),
                //row.TaxRepresentativeParty = GetTaxRepresantiveParty(),

                #region Fatura Satırları - InvoiceLines



                //Fatura Satırları
                row.InvoiceLine = inv.Lines.Select(x => new InvoiceLineType
                {
                    ID = new IDType { Value = $"{inv.Lines.IndexOf(x) + 1}" },
                    Item = new ItemType
                    {
                        Name = new NameType1 { Value = x.Name },
                        Description = new DescriptionType { Value = x.Description },
                    },
                    AllowanceCharge = x.DiscountRate > 0
                    ? new AllowanceChargeType[]
                    {
                        new AllowanceChargeType
                        {
                            ChargeIndicator = new ChargeIndicatorType { Value = true } ,
                            MultiplierFactorNumeric = new MultiplierFactorNumericType {  Value =  x.DiscountRate / 100 },
                            Amount = new AmountType2 { Value = (x.Price - x.DiscountedPrice) * x.Qty , currencyID = "TRY"},
                        }
                    }
                    : null,
                    Price = new PriceType { PriceAmount = new PriceAmountType { Value = x.Price, currencyID = "TRY" } },
                    InvoicedQuantity = new InvoicedQuantityType { unitCode = "NIU", Value = x.Qty },
                    TaxTotal = new TaxTotalType
                    {
                        TaxSubtotal = new TaxSubtotalType[]
                            {
                                new TaxSubtotalType
                                {
                                    Percent = new PercentType1 { Value =  x.TaxRate},
                                    TaxAmount = new TaxAmountType { Value = x.TaxPrice , currencyID = "TRY"},
                                    TaxCategory = new TaxCategoryType
                                    {
                                        TaxScheme = new TaxSchemeType
                                            {
                                                TaxTypeCode = new TaxTypeCodeType {  Value = "0015"},
                                                Name =new NameType1{ Value="KDV"}
                                            },
                                        //TaxExemptionReason= x.TaxRate == 0 ? new TaxExemptionReasonType{ Value = "11/1-a Mal ihracatı"} : null,
                                        //TaxExemptionReasonCode = x.TaxRate == 0 ? new TaxExemptionReasonCodeType { Value = "301"} : null
                                    },
                                }
                            },
                        TaxAmount = new TaxAmountType { Value = x.TaxPrice, currencyID = "TRY" }
                    },
                    LineExtensionAmount = new LineExtensionAmountType { Value = x.TotalPrice, currencyID = "TRY" },

                }).ToArray();

                #endregion

                #region Vergi Alt Toplamları - TaxTotal

                if (inv.Lines.Any(x => x.TaxRate > 0))
                {
                    row.TaxTotal = inv.Lines.Where(x => x.TaxRate > 0).GroupBy(x => x.TaxRate).Select(x =>
                    new TaxTotalType
                    {
                        TaxSubtotal = new TaxSubtotalType[]
                        {
                                new TaxSubtotalType
                                {
                                    Percent = new PercentType1 { Value = x.Key},
                                    TaxAmount = new TaxAmountType { Value = x.Sum(_=>_.TaxPrice) , currencyID = "TRY" },
                                    TaxCategory = new TaxCategoryType
                                            {
                                                TaxScheme = new TaxSchemeType
                                                    {
                                                        TaxTypeCode = new TaxTypeCodeType {  Value = "0015"},
                                                        Name =new NameType1{ Value = "KDV"}
                                                    },
                                            },
                                },
                        },
                        TaxAmount = new TaxAmountType { Value = x.Sum(_ => _.TaxPrice), currencyID = "TRY" }
                    }).ToArray();
                }
                else
                {
                    row.TaxTotal = new TaxTotalType[]
                    {
                        new TaxTotalType
                        {
                            TaxSubtotal = new TaxSubtotalType[]
                            {
                                    new TaxSubtotalType
                                    {
                                        Percent = new PercentType1 { Value = 0},
                                        TaxAmount = new TaxAmountType { Value = inv.Lines.Sum(_=>_.TaxPrice) , currencyID = "TRY" },
                                        TaxCategory = new TaxCategoryType
                                                {
                                                    TaxScheme = new TaxSchemeType
                                                        {
                                                            TaxTypeCode = new TaxTypeCodeType {  Value = "0015"},
                                                            Name =new NameType1{ Value = "KDV"}
                                                        },
                                                    TaxExemptionReason= new TaxExemptionReasonType{ Value = inv.TaxExemption.Reason} ,
                                                    TaxExemptionReasonCode =  new TaxExemptionReasonCodeType { Value = inv.TaxExemption.Code}
                                                },
                                    },
                            },
                            TaxAmount = new TaxAmountType { Value = inv.Lines.Sum(_ => _.TaxPrice), currencyID = "TRY" }
                        }
                    };

                }






                #endregion

                #region Yasal Alt Toplamlar - Legal Monetary Total

                row.LegalMonetaryTotal = new MonetaryTotalType
                {
                    LineExtensionAmount = new LineExtensionAmountType { Value = inv.Price, currencyID = "TRY" },
                    TaxExclusiveAmount = new TaxExclusiveAmountType { Value = inv.DiscountedPrice, currencyID = "TRY" },
                    TaxInclusiveAmount = new TaxInclusiveAmountType { Value = inv.TotalPrice, currencyID = "TRY" },
                    AllowanceTotalAmount = new AllowanceTotalAmountType { Value = inv.Price - inv.DiscountedPrice, currencyID = "TRY" },
                    //-+    ChargeTotalAmount = new ChargeTotalAmountType { Value = Convert.ToDecimal(txtIskontoTutar1.Text) + Convert.ToDecimal(txtIskontoTutar2.Text), currencyID = "TRY" },
                    PayableAmount = new PayableAmountType { Value = inv.TotalPrice, currencyID = "TRY" },
                    // PayableRoundingAmount = new PayableRoundingAmountType { Value = Convert.ToDecimal(txtToplamTutar1.Text) + Convert.ToDecimal(txtToplamTutar2.Text), currencyID = "TRY" }

                };
                #endregion

            };

            #region e-Arşiv Fatura Bilgileri
            //Bu alanda eğer fatura bir e-arşiv faturası ise doldurulması gerkene alanlar doldurulmalıdır.
            EArchiveInvoiceInformation earchiveinfo = new EArchiveInvoiceInformation
            {
                DeliveryType = InvoiceDeliveryType.Electronic, //kağıt ortamda olduğunda Paper değeri set edilmelidir.
            };


            #endregion



            return new InvoiceInfo
            {
                EArchiveInvoiceInfo = earchiveinfo,
                LocalDocumentId = string.Empty,
                Invoice = row,
                TargetCustomer = new CustomerInfo { Alias = string.Empty },
                Scenario = InvoiceScenarioChoosen.Automated,
                ExtraInformation = string.Empty,
            };

        }



        async Task<Models.Response<Models.SendInvoiceRes>> IInvoiceClient.SendInvoice(Models.Invoice inv)
        {

            var re = new Models.Response<Models.SendInvoiceRes>();
            re.Data = new Models.SendInvoiceRes();
            try
            {
                var invoice = CreateInvoice(inv);
                var res = await client.SendInvoiceAsync(userInformation, new InvoiceInfo[] { invoice });
                if (!res.IsSucceded)
                {
                    throw new Exception(res.Message);
                }

                re.Data.InvoiceId = res.Value[0].Id;
                re.Data.InvoiceNo = res.Value[0].Number;
            }
            catch (Exception ex)
            {
                re.InitError(ex);
            }
            return re;

        }


    }
}
