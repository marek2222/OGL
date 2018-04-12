namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Zdjecie2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Zdjecie", "ImageName", c => c.String());
            AddColumn("dbo.Zdjecie", "SizeName", c => c.String());
            AddColumn("dbo.Zdjecie", "ImgBytes", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Zdjecie", "ImgBytes");
            DropColumn("dbo.Zdjecie", "SizeName");
            DropColumn("dbo.Zdjecie", "ImageName");
        }
    }
}
