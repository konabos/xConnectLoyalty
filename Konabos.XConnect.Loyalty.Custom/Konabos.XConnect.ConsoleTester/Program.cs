using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Konabos.XConnect.Loyalty.Model.Facets;
using Konabos.XConnect.Loyalty.Model.Models;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;

namespace Konabos.XConnect.ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () => { await ProcessCustomers(); }).Wait();

            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            System.Console.WriteLine("");
            System.Console.WriteLine("END OF PROGRAM.");
            System.Console.ReadKey();
        }

        private static async Task ProcessCustomers()
        {
            CertificateWebRequestHandlerModifierOptions options =
            CertificateWebRequestHandlerModifierOptions.Parse("StoreName=My;StoreLocation=LocalMachine;FindType=FindByThumbprint;FindValue=BC9B7186102910E8F34EE8D9F38138203F7555BA");

            var certificateModifier = new CertificateWebRequestHandlerModifier(options);

            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            var collectionClient = new CollectionWebApiClient(new Uri("https://cateringdemo.xconnect.dev.local/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri("https://cateringdemo.xconnect.dev.local/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri("https://cateringdemo.xconnect.dev.local/configuration"), clientModifiers, new[] { certificateModifier });

            var cfg = new XConnectClientConfiguration(
                new XdbRuntimeModel(LoyaltyModel.Model), collectionClient, searchClient, configurationClient);

            try
            {
                await cfg.InitializeAsync(); 
            }
            catch (XdbModelConflictException ce)
            {
                System.Console.WriteLine("ERROR:" + ce.Message);
                return;
            }

            using (var client = new XConnectClient(cfg))
            {
                try
                {
                    //search for an existing contact based on an identifier
                    IdentifiedContactReference reference = new IdentifiedContactReference("CommerceUser", "Storefront\\sakshay13@gmail.com");
                    Contact existingContact = client.Get<Contact>(reference, new ContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey, LoyaltyPointsFacet.DefaultFacetKey, LoyaltyCommerceFacet.DefaultFacetKey }));

                    if (existingContact != null)
                    {
                        LoyaltyPointsFacet loyaltyPointFacet = existingContact.GetFacet<LoyaltyPointsFacet>(LoyaltyPointsFacet.DefaultFacetKey);

                        if (loyaltyPointFacet == null)
                        {
                            loyaltyPointFacet = new LoyaltyPointsFacet()
                            {
                                PointsEarned = 33,
                                PointsSpent = 44
                            };
                            client.SetFacet<LoyaltyPointsFacet>(existingContact, LoyaltyPointsFacet.DefaultFacetKey, loyaltyPointFacet);

                            client.Submit();
                        }

                        LoyaltyCommerceFacet loyaltyCommerceFacet = existingContact.GetFacet<LoyaltyCommerceFacet>(LoyaltyCommerceFacet.DefaultFacetKey);
                        if (loyaltyCommerceFacet == null)
                        {
                            loyaltyCommerceFacet = new LoyaltyCommerceFacet()
                            {
                                CommerceCustomerId = "001"
                            };
                            client.SetFacet<LoyaltyCommerceFacet>(existingContact, LoyaltyCommerceFacet.DefaultFacetKey, loyaltyCommerceFacet);

                            client.Submit();
                        }
                    }

                    //Get all contacts and process them
                    //var contacts = client.Contacts.WithExpandOptions(new ContactExpandOptions(PersonalInformation.DefaultFacetKey)).ToEnumerable();
                    ////var contacts = client.Contacts.ToEnumerable();
                    //foreach (var contact in contacts)
                    //{
                    //    Console.WriteLine("Contact ID: " + contact.Id.ToString());

                    //    var rsonalInformationFacet = contact.GetFacet<PersonalInformation>();
                    //    PersonalInformation personalInformationFacet = contact.GetFacet<PersonalInformation>(PersonalInformation.DefaultFacetKey);
                    //    if (personalInformationFacet != null)
                    //        Console.WriteLine("Contact Name: " + personalInformationFacet.FirstName + " " + personalInformationFacet.LastName);
                    //    else
                    //        Console.WriteLine("Contact Personal Information not found.");

                    //    LoyaltyPointsFacet loyaltyPointFacet = contact.GetFacet<LoyaltyPointsFacet>(LoyaltyPointsFacet.DefaultFacetKey);

                    //    if (loyaltyPointFacet == null)
                    //    {
                    //        Console.WriteLine("Contact Loyalty Information not found.");
                    //        LoyaltyPointsFacet visitorInfo = new LoyaltyPointsFacet()
                    //        {
                    //            PointsEarned = 0,
                    //            PointsSpent = 0
                    //        };
                    //        client.SetFacet<LoyaltyPointsFacet>(contact, LoyaltyPointsFacet.DefaultFacetKey, visitorInfo);
                    //    }
                    //    else
                    //        Console.WriteLine("Contact Loyalty Found: " + loyaltyPointFacet.PointsEarned);


                    //}

                    Console.ReadLine();
                }
                catch (XdbExecutionException ex)
                {
                    System.Console.WriteLine("ERROR:" + ex.Message);
                    return;
                }
            }
        }
    }
}
