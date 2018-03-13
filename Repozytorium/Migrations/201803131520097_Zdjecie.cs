namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Zdjecie : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Zdjecie",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UzytkownikId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UzytkownikId)
                .Index(t => t.UzytkownikId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Zdjecie", "UzytkownikId", "dbo.AspNetUsers");
            DropIndex("dbo.Zdjecie", new[] { "UzytkownikId" });
            DropTable("dbo.Zdjecie");
        }
    }
}
