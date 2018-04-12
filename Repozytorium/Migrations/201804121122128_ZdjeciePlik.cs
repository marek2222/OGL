namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZdjeciePlik : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ZdjeciePlik",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UzytkownikId = c.String(maxLength: 128),
                        ImageName = c.String(),
                        SizeName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UzytkownikId)
                .Index(t => t.UzytkownikId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZdjeciePlik", "UzytkownikId", "dbo.AspNetUsers");
            DropIndex("dbo.ZdjeciePlik", new[] { "UzytkownikId" });
            DropTable("dbo.ZdjeciePlik");
        }
    }
}
