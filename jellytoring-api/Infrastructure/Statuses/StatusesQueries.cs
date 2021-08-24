namespace jellytoring_api.Infrastructure.Statuses
{
    public static class StatusesQueries
    {
        public const string GetAll = "select id Id, code Code, name Name from statuses";
        public const string Get = GetAll + " where code = @statusCode";
    }
}
