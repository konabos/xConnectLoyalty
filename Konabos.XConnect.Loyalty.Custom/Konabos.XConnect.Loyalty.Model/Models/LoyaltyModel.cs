using Konabos.XConnect.Loyalty.Model.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;

namespace Konabos.XConnect.Loyalty.Model.Models
{
    public class LoyaltyModel
    {
        public static XdbModel Model { get; } = BuildModel();

        private static XdbModel BuildModel()
        {
            XdbModelBuilder modelBuilder = new XdbModelBuilder("LoyaltyModel", new XdbModelVersion(1, 0));

            modelBuilder.DefineFacet<Contact, LoyaltyPointsFacet>(FacetKeys.LoyaltyPointsFacet);
            modelBuilder.DefineFacet<Interaction, LoyaltyOrderInfoFacet>(FacetKeys.LoyaltyOrderInfoFacet);
            modelBuilder.DefineFacet<Contact, LoyaltyCommerceFacet>(FacetKeys.LoyaltyCommerceFacet);

            modelBuilder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);

            return modelBuilder.BuildModel();
        }
    }
    public class FacetKeys
    {
        public const string LoyaltyOrderInfoFacet = "LoyaltyOrderInfoFacet";
        public const string LoyaltyPointsFacet = "LoyaltyPointsFacet";
        public const string LoyaltyCommerceFacet = "LoyaltyCommerceFacet";
    }
}
