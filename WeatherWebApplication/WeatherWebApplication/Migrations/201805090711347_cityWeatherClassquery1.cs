namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cityWeatherClassquery1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CityWeathers", new[] { "query_id" });
            RenameColumn(table: "dbo.queries", name: "query_id", newName: "CityWeather_city");
            CreateIndex("dbo.queries", "CityWeather_city");
            DropColumn("dbo.CityWeathers", "query_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CityWeathers", "query_id", c => c.Int());
            DropIndex("dbo.queries", new[] { "CityWeather_city" });
            RenameColumn(table: "dbo.queries", name: "CityWeather_city", newName: "query_id");
            CreateIndex("dbo.CityWeathers", "query_id");
        }
    }
}
