namespace Sln_Granada_Reservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsBlockedToTbTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbTable", "IsBlocked", c => c.Boolean(nullable: false));
          //  DropTable("dbo.TbBlockedTables");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TbBlockedTables",
                c => new
                    {
                        TableId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.TableId);
            
            DropColumn("dbo.TbTable", "IsBlocked");
        }
    }
}
