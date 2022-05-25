using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.PasswordRecoveries
{
    public static class PasswordRecoveryQueries
    {
        public const string Create = @"insert into password_recoveries(user_id, confirmation_code, issued_at)
                                                                           values (@userId, @confirmationCode, @issuedAt);
                                                   select Last_Insert_Id();";

        public const string Get = @"select user_id UserId, issued_at IssuedAt
                                    from password_recoveries
                                    where confirmation_code = @confirmationCode";

        public const string UpdatePassword = @"update users u
                                                join password_recoveries pr on pr.user_id = u.id
                                                set password = @password
                                                where pr.confirmation_code = @confirmationCode;
                                                delete from password_recoveries where confirmation_code = @confirmationCode;";
    }
}
