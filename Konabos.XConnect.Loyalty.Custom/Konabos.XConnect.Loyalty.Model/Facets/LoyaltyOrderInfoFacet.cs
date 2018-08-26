using Sitecore.XConnect;

namespace Konabos.XConnect.Loyalty.Model.Facets
{
    [FacetKey(DefaultFacetKey)]
    public class LoyaltyOrderInfoFacet : Facet //Interaction Facet: Information about the order place during the interaction
    {
        public const string DefaultFacetKey = "LoyaltyOrderInfoFacet";
        public string OrderId { get; set; }
        public double OrderTotal { get; set; }
        public bool PointsUsers { get; set; }
    }
}
