namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityWeathervaluesdded3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CityWeathers");
            AddColumn("dbo.CityWeathers", "id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.CityWeathers", new[] { "id", "city" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CityWeathers");
            DropColumn("dbo.CityWeathers", "id");
            AddPrimaryKey("dbo.CityWeathers", "city");
        }
    }
}
