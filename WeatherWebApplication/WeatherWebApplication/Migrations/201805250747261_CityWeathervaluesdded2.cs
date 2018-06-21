namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityWeathervaluesdded2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CityWeathers", "region", c => c.String());
            AddColumn("dbo.CityWeathers", "day", c => c.String());
            AddColumn("dbo.CityWeathers", "day1", c => c.String());
            AddColumn("dbo.CityWeathers", "day2", c => c.String());
            AddColumn("dbo.CityWeathers", "day3", c => c.String());
            AddColumn("dbo.CityWeathers", "day4", c => c.String());
            AddColumn("dbo.CityWeathers", "day5", c => c.String());
            AddColumn("dbo.CityWeathers", "day6", c => c.String());
            AddColumn("dbo.CityWeathers", "day7", c => c.String());
            AddColumn("dbo.CityWeathers", "day8", c => c.String());
            AddColumn("dbo.CityWeathers", "day9", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CityWeathers", "day9");
            DropColumn("dbo.CityWeathers", "day8");
            DropColumn("dbo.CityWeathers", "day7");
            DropColumn("dbo.CityWeathers", "day6");
            DropColumn("dbo.CityWeathers", "day5");
            DropColumn("dbo.CityWeathers", "day4");
            DropColumn("dbo.CityWeathers", "day3");
            DropColumn("dbo.CityWeathers", "day2");
            DropColumn("dbo.CityWeathers", "day1");
            DropColumn("dbo.CityWeathers", "day");
            DropColumn("dbo.CityWeathers", "region");
        }
    }
}
