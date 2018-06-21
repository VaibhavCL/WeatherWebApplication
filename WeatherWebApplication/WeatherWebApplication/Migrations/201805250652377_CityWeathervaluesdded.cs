namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityWeathervaluesdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CityWeathers", "sunrise", c => c.String());
            AddColumn("dbo.CityWeathers", "sunset", c => c.String());
            AddColumn("dbo.CityWeathers", "chill", c => c.Int(nullable: false));
            AddColumn("dbo.CityWeathers", "date", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CityWeathers", "date");
            DropColumn("dbo.CityWeathers", "chill");
            DropColumn("dbo.CityWeathers", "sunset");
            DropColumn("dbo.CityWeathers", "sunrise");
        }
    }
}
