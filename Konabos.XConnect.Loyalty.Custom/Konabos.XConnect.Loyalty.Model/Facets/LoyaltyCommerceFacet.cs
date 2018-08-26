using Sitecore.XConnect;

namespace Konabos.XConnect.Loyalty.Model.Facets
{
    public class LoyaltyCommerceFacet : Facet //Contact facet to store the Commerce CustomerId  - read only
    {
        public const string DefaultFacetKey = "LoyaltyCommerceFacet";

        public string CommerceCustomerId { get; set; }
    }
}
