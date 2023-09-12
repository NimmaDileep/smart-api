namespace TokenAuth.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResumeToSubmissions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Submissions", "Resume", c => c.Binary());
            AddColumn("dbo.Submissions", "ResumeContentType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Submissions", "ResumeContentType");
            DropColumn("dbo.Submissions", "Resume");
        }
    }
}
