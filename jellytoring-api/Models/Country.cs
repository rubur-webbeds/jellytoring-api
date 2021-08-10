namespace jellytoring_api.Models
{
    public class Country
    {
        public uint Id { get; set; }
        public int PhoneCode { get; set; }
        public string CountryCode { get; set; }
        public string CountryCodeAlpha3 { get; set; }
        public string CountryName { get; set; }
        public string ContinentCode { get; set; }
    }
}
