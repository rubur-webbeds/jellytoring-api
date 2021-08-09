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
    }
}
