namespace jellytoring_api.Infrastructure.Countries
{
    public static class CountriesQueries
    {
        public const string GetAll = @"select id Id, phone_code PhoneCode, country_code CountryCode, country_code_alpha_3 CountryCodeAlpha3,
                                            country_name CountryName, continent_code ContinentCode
                                      from countries;";
    }
}
