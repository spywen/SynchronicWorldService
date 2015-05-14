using System.Runtime.Serialization;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class ContributionType
    {
        public ContributionType() { }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Value { get; set; }

    }

    /// <summary>
    /// Contribution Type code
    /// </summary>
    public enum ContributionTypeCode
    {
        Money,
        Food,
        Beverage
    }
}
