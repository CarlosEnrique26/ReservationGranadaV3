namespace Sln_Granada_Reservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTbBlockedTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TbBlockedTables",
                c => new
                    {
                        TableId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.TableId);
           
            CreateTable(
                "dbo.TbChair",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TbTableId = c.Int(nullable: false),
                        DescriptionChair = c.String(),
                        Position = c.String(),
                        Active = c.Boolean(nullable: false),
                        IsOcupate = c.Boolean(nullable: false),
                        DateReservation = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TbConfig",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enterprise = c.String(),
                        BackgroundTable = c.String(),
                        URL = c.String(),
                        txtCredential = c.String(),
                        txtConfirm = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TbReservation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TbTableId = c.Int(nullable: false),
                        TbUserId = c.Int(nullable: false),
                        TbChair = c.Int(nullable: false),
                        Observation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TbTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DescriptionTable = c.String(),
                        TotalPerson = c.Int(nullable: false),
                        Position = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TbUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameUser = c.String(),
                        DNI = c.String(),
                        UserName = c.String(),
                        TypeUser = c.Int(nullable: false),
                        NameFriend = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        PasswordUser = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TbUser");
            DropTable("dbo.TbTable");
            DropTable("dbo.TbReservation");
            DropTable("dbo.TbConfig");
            DropTable("dbo.TbChair");
            DropTable("dbo.TbBlockedTables");
        }
    }
}
