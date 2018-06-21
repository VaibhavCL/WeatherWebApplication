namespace WeatherWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityWeatherClassIdAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CityWeathers", "query_id", "dbo.queries");
            DropIndex("dbo.CityWeathers", new[] { "query_id" });
            RenameColumn(table: "dbo.CityWeathers", name: "query_id", newName: "Id");
            AlterColumn("dbo.CityWeathers", "Id", c => c.Int(nullable: false));
            CreateIndex("dbo.CityWeathers", "Id");
            AddForeignKey("dbo.CityWeathers", "Id", "dbo.queries", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CityWeathers", "Id", "dbo.queries");
            DropIndex("dbo.CityWeathers", new[] { "Id" });
            AlterColumn("dbo.CityWeathers", "Id", c => c.Int());
            RenameColumn(table: "dbo.CityWeathers", name: "Id", newName: "query_id");
            CreateIndex("dbo.CityWeathers", "query_id");
            AddForeignKey("dbo.CityWeathers", "query_id", "dbo.queries", "id");
        }
    }
}
