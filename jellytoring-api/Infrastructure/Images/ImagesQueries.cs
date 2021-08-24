﻿namespace jellytoring_api.Infrastructure.Images
{
    public static class ImagesQueries
    {
        private const string Select = @"select images.id Id, user_id UserId, location Location, date Date, filename Filename, images.status_id Status_Id, statuses.code Status_Code, statuses.name Status_Name
                                    from images 
                                    join statuses on images.status_id = statuses.id";

        public const string Create = @"insert into images(user_id, location, date, filename, status_id)
                                        values (@userId, @Location, @Date, @Filename, @StatusId);
                                        select Last_Insert_Id();";

        public const string Get = Select + " where images.id = @imageId;";

        public const string GetUserImages = Select + " where user_id = @userId";
    }
}
