namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityWeatherValuesAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CityWeathers", "high7", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "high8", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "high9", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "high10", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "low7", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "low8", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "low9", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "low10", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CityWeathers", "low10");
            DropColumn("dbo.CityWeathers", "low9");
            DropColumn("dbo.CityWeathers", "low8");
            DropColumn("dbo.CityWeathers", "low7");
            DropColumn("dbo.CityWeathers", "high10");
            DropColumn("dbo.CityWeathers", "high9");
            DropColumn("dbo.CityWeathers", "high8");
            DropColumn("dbo.CityWeathers", "high7");
        }
    }
}
