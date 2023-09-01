namespace TokenAuth.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubmissionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Submissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        Role = c.String(),
                        Client = c.String(),
                        VendorCompany = c.String(),
                        VendorName = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Submissions", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Submissions", new[] { "EmployeeId" });
            DropTable("dbo.Submissions");
        }
    }
}
