namespace jellytoring_api.Infrastructure.Users
{
    public static class UsersQueries
    {
        public const string Create = @"insert into users(full_name, email, password, interest_id, institution, country_code,
                                                         grant_contact_info_permission, grant_uib_permission)
                                        values(@FullName, @Email, @Password, @InterestId, @Institution, @CountryCode,
                                                @GrantContactInfoPermission, @GrantUibPermission);
                                        select Last_Insert_Id();";

        public const string GetAll = @"select id, full_name, email, password, interest_id, institution, country_code,
                                              grant_contact_info_permission, grant_uib_permission, email_confirmed, active
                                       from users;";

        public const string Get = @"select id Id, full_name FullName, email Email, interest_id InterestId,
                                           institution Institution, country_code CountryCode, grant_contact_info_permission GrantContactInfoPermission,
                                           grant_uib_permission GrantUibPermission, email_confirmed EmailConfirmed, active Active
                                    from users
                                    where id = @Id;";
    }
}
