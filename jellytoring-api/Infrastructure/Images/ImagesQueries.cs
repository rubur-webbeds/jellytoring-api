namespace jellytoring_api.Infrastructure.Images
{
    public static class ImagesQueries
    {
        public const string Create = @"insert into images(user_id, location, date, filename, confirmed)
                                        values (@userId, @Location, @Date, @Filename, @Confirmed);
                                        select Last_Insert_Id();";

        public const string Get = @"select id Id, user_id UserId, location Location, date Date, filename Filename, confirmed Confirmed
                                    from images where id = @imageId;";
    }
}
