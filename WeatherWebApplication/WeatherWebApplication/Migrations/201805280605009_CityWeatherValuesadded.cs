namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityWeatherValuesadded : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CityWeathers");
            AddPrimaryKey("dbo.CityWeathers", "city");
            DropColumn("dbo.CityWeathers", "id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CityWeathers", "id", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.CityWeathers");
            AddPrimaryKey("dbo.CityWeathers", new[] { "id", "city" });
        }
    }
}
