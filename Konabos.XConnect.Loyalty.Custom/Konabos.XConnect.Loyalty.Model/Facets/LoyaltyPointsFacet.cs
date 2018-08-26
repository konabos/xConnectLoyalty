using Sitecore.XConnect;

namespace Konabos.XConnect.Loyalty.Model.Facets
{
    [FacetKey(DefaultFacetKey)]
    public class LoyaltyPointsFacet : Facet //Contact facet to store the points earned and spent based on the minion processing - read only
    {
        public const string DefaultFacetKey = "LoyaltyPointsFacet";

        public int PointsEarned { get; set; }
        public int PointsSpent { get; set; }
    }
}
