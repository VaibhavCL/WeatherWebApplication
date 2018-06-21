namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class precipitationValueAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CityWeathers", "precipitation", c => c.String());
            AlterColumn("dbo.CityWeathers", "visibility", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CityWeathers", "visibility", c => c.String());
            DropColumn("dbo.CityWeathers", "precipitation");
        }
    }
}
