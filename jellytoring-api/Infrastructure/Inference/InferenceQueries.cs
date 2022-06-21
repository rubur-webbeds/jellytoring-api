namespace jellytoring_api.Infrastructure.Inference
{
    public static class InferenceQueries
    {
        public const string Create = @"insert into running_inferences(user_id, image_name, started_at, status)
                                        values (@userId, @imageName, @startedAt, @status);
                                        select Last_Insert_Id();";

        public const string MarkAsCompleted = @"update running_inferences set status = 'COMPLETED', output_path = @outputPath 
                                                where image_name = @originalImageName and status = 'RUNNING';";

        public const string Get = "select status, output_path from running_inferences where id = @inferenceId";
    }
}
