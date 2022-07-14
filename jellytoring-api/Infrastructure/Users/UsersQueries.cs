namespace jellytoring_api.Infrastructure.Users
{
    public static class UsersQueries
    {
        public const string Create = @"insert into users(full_name, email, password, interest_id, institution, country_code,
                                                         grant_contact_info_permission, grant_uib_permission)
                                        values(@FullName, @Email, @Password, @InterestId, @Institution, @CountryCode,
                                                @GrantContactInfoPermission, @GrantUibPermission);
                                        select Last_Insert_Id();";

        public const string GetAll = @"select u.id Id, full_name FullName, email Email, institution Institution, email_confirmed EmailConfirmed,
                                        active Active, i.name InterestName, c.country_name CountryName, r.code RoleCode, count(ri.user_id) InferencesCount
                                        from users u
                                        join interests i on u.interest_id = i.id
                                        join countries c on u.country_code = c.country_code
                                        join user_roles ur on u.id = ur.user_id
                                        join roles r on r.id = ur.role_id
                                        join running_inferences ri on u.id = ri.user_id
                                        group by (ri.user_id);";

        public const string Get = @"select id Id, full_name FullName, email Email, interest_id InterestId,
                                           institution Institution, country_code CountryCode, grant_contact_info_permission GrantContactInfoPermission,
                                           grant_uib_permission GrantUibPermission, email_confirmed EmailConfirmed, active Active
                                    from users
                                    where id = @Id;";

        public const string GetByEmail = @"select id Id, email Email, password Password, email_confirmed EmailConfirmed
                                        from users
                                        where email = @email;";

        public const string GetUserRoles = @"select u.email as Email, u.full_name as FullName, r.id as Roles_Id, r.name as Roles_Name, r.Code as Roles_Code
                                        from users u
                                        inner join user_roles ur on u.id = ur.user_id
                                        inner join roles r on ur.role_id = r.id
                                        where email = @email;";

        public const string GetUserRoleId = "select id from roles where code = 'USR';";
        public const string GetAdminRoleId = "select id from roles where code = 'ADM';";
        public const string AddRoleToUser = "insert into user_roles(user_id, role_id) values (@userId, @userRoleId);";
        public const string SetRoleToUser = "update user_roles set role_id = @RoleId where user_id = @UserId";

    }
}
