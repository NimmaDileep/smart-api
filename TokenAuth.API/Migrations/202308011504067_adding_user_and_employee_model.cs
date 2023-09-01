namespace TokenAuth.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adding_user_and_employee_model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeesNew",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Age = c.Int(nullable: false),
                        Country = c.String(),
                        Position = c.String(),
                        Wage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        Roles = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.EmployeesNew");
        }
    }
}
