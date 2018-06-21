namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityWeathervaluesdded1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CityWeathers", "date1", c => c.String());
            AddColumn("dbo.CityWeathers", "date2", c => c.String());
            AddColumn("dbo.CityWeathers", "date3", c => c.String());
            AddColumn("dbo.CityWeathers", "date4", c => c.String());
            AddColumn("dbo.CityWeathers", "date5", c => c.String());
            AddColumn("dbo.CityWeathers", "date6", c => c.String());
            AddColumn("dbo.CityWeathers", "date7", c => c.String());
            AddColumn("dbo.CityWeathers", "date8", c => c.String());
            AddColumn("dbo.CityWeathers", "date9", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CityWeathers", "date9");
            DropColumn("dbo.CityWeathers", "date8");
            DropColumn("dbo.CityWeathers", "date7");
            DropColumn("dbo.CityWeathers", "date6");
            DropColumn("dbo.CityWeathers", "date5");
            DropColumn("dbo.CityWeathers", "date4");
            DropColumn("dbo.CityWeathers", "date3");
            DropColumn("dbo.CityWeathers", "date2");
            DropColumn("dbo.CityWeathers", "date1");
        }
    }
}
