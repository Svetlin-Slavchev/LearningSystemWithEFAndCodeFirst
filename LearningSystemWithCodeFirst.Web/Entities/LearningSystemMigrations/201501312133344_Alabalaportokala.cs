namespace LearningSystemWithCodeFirst.Web.Entities.LearningSystemMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alabalaportokala : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Description = c.String(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Description = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lessons", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Lessons", new[] { "CategoryId" });
            DropTable("dbo.Lessons");
            DropTable("dbo.Categories");
        }
    }
}
