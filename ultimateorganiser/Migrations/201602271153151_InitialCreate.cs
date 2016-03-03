namespace ultimateorganiser.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClubEvents",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        EventTitle = c.String(nullable: false),
                        EventDate = c.DateTime(nullable: false),
                        EventDesc = c.String(),
                        EventLocation = c.String(nullable: false),
                        eventType = c.Int(nullable: false),
                        eventPriority = c.Int(nullable: false),
                        ClubID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.Clubs", t => t.ClubID, cascadeDelete: true)
                .Index(t => t.ClubID);
            
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        ClubID = c.Int(nullable: false, identity: true),
                        ClubName = c.String(nullable: false, maxLength: 50),
                        ClubDescription = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ClubID);
            
            CreateTable(
                "dbo.ClubMembers",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserFName = c.String(nullable: false, maxLength: 50),
                        UserLName = c.String(nullable: false, maxLength: 50),
                        UserName = c.String(nullable: false),
                        UserEmail = c.String(nullable: false, maxLength: 50),
                        UserDescription = c.String(maxLength: 200),
                        UserDoB = c.DateTime(nullable: false),
                        UserImage = c.String(),
                        UserPassword = c.String(maxLength: 20),
                        ClubID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Clubs", t => t.ClubID, cascadeDelete: true)
                .Index(t => t.ClubID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClubMembers", "ClubID", "dbo.Clubs");
            DropForeignKey("dbo.ClubEvents", "ClubID", "dbo.Clubs");
            DropIndex("dbo.ClubMembers", new[] { "ClubID" });
            DropIndex("dbo.ClubEvents", new[] { "ClubID" });
            DropTable("dbo.ClubMembers");
            DropTable("dbo.Clubs");
            DropTable("dbo.ClubEvents");
        }
    }
}
