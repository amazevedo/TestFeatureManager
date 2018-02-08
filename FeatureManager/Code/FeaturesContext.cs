namespace FeatureManager.DAL
{
    using System.Data.Entity;

    public class FeaturesContext : DbContext
    {
        // Your context has been configured to use a 'FeaturesContext' connection string from your application's
        // configuration file (App.config or Web.config). By default, this connection string targets the
        // 'FeatureManager.DAL.FeaturesContext' database on your LocalDb instance.
        //
        // If you wish to target a different database and/or database provider, modify the 'FeaturesContext'
        // connection string in the application configuration file.

        public FeaturesContext()
            : base("name=Features")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Models.Feature> Features { get; set; }

        public virtual DbSet<Models.FeatureVersion> FeatureVersions { get; set; }

        public virtual DbSet<Models.System> Systems { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}