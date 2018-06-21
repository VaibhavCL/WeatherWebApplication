namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class precipitationValueAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CityWeathers", "precipitation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CityWeathers", "precipitation");
        }
    }
}
