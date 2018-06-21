namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityWeatherValuesadded : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CityWeathers");
            AddColumn("dbo.CityWeathers", "id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.CityWeathers", "high7", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "high8", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "high9", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "high10", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "low7", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "low8", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "low9", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "low10", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "precipitation", c => c.String());
            AddColumn("dbo.CityWeathers", "sunrise", c => c.String());
            AddColumn("dbo.CityWeathers", "sunset", c => c.String());
            AddColumn("dbo.CityWeathers", "chill", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "date", c => c.String());
            AddColumn("dbo.CityWeathers", "date1", c => c.String());
            AddColumn("dbo.CityWeathers", "date2", c => c.String());
            AddColumn("dbo.CityWeathers", "date3", c => c.String());
            AddColumn("dbo.CityWeathers", "date4", c => c.String());
            AddColumn("dbo.CityWeathers", "date5", c => c.String());
            AddColumn("dbo.CityWeathers", "date6", c => c.String());
            AddColumn("dbo.CityWeathers", "date7", c => c.String());
            AddColumn("dbo.CityWeathers", "date8", c => c.String());
            AddColumn("dbo.CityWeathers", "date9", c => c.String());
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
            AddPrimaryKey("dbo.CityWeathers", new[] { "id", "city" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CityWeathers");
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
            DropColumn("dbo.CityWeathers", "date9");
            DropColumn("dbo.CityWeathers", "date8");
            DropColumn("dbo.CityWeathers", "date7");
            DropColumn("dbo.CityWeathers", "date6");
            DropColumn("dbo.CityWeathers", "date5");
            DropColumn("dbo.CityWeathers", "date4");
            DropColumn("dbo.CityWeathers", "date3");
            DropColumn("dbo.CityWeathers", "date2");
            DropColumn("dbo.CityWeathers", "date1");
            DropColumn("dbo.CityWeathers", "date");
            DropColumn("dbo.CityWeathers", "chill");
            DropColumn("dbo.CityWeathers", "sunset");
            DropColumn("dbo.CityWeathers", "sunrise");
            DropColumn("dbo.CityWeathers", "precipitation");
            DropColumn("dbo.CityWeathers", "low10");
            DropColumn("dbo.CityWeathers", "low9");
            DropColumn("dbo.CityWeathers", "low8");
            DropColumn("dbo.CityWeathers", "low7");
            DropColumn("dbo.CityWeathers", "high10");
            DropColumn("dbo.CityWeathers", "high9");
            DropColumn("dbo.CityWeathers", "high8");
            DropColumn("dbo.CityWeathers", "high7");
            DropColumn("dbo.CityWeathers", "id");
            AddPrimaryKey("dbo.CityWeathers", "city");
        }
    }
}
