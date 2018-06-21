namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cityWeatherClassquery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CityWeathers", "query_id", c => c.Int());
            CreateIndex("dbo.CityWeathers", "query_id");
            AddForeignKey("dbo.CityWeathers", "query_id", "dbo.queries", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CityWeathers", "query_id", "dbo.queries");
            DropIndex("dbo.CityWeathers", new[] { "query_id" });
            DropColumn("dbo.CityWeathers", "query_id");
        }
    }
}
