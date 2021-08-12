using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Email
{
    public static class EmailConfirmationQueries
    {
        public const string Create = @"insert into email_confirmations(user_id, confirmation_code, issued_at)
                                                                           values (@userId, @confirmationCode, @issuedAt);
                                                   select Last_Insert_Id();";

        public const string ConfirmEmail = @"update users set email_confirmed = 1 where id = (
                                                select user_id from email_confirmations where confirmation_code = @confirmationCode);
                                             delete from email_confirmations where confirmation_code = @confirmationCode;";

        public const string Get = @"select user_id UserId, issued_at IssuedAt
                                    from email_confirmations
                                    where confirmation_code = @confirmationCode";
    }
}
