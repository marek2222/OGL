namespace Repozytorium.Migrations
{
  using System;
  using System.Data.Entity.Migrations;

  public partial class Edytor : DbMigration
  {
    public override void Up()
    {
      CreateTable(
      "dbo.Edytor",
      c => new
      {
        Id = c.String(nullable: false, maxLength: 128),
        Tresc = c.String(),
      })
      .PrimaryKey(t => t.Id)
      .ForeignKey("dbo.AspNetUsers", t => t.Id)
      .Index(t => t.Id);
    }

    public override void Down()
    {
      DropForeignKey("dbo.Edytor", "Id", "dbo.AspNetUsers"); 
      DropIndex("dbo.Edytor", new[] { "Id" }); 
      DropTable("dbo.Edytor");
    }
  }
}
