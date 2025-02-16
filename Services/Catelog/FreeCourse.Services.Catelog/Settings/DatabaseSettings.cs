namespace FreeCourse.Services.Catelog.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string CourseCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ConectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
