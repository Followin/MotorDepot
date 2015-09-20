namespace MotorDepot.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class onemore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DriverLicenses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IssueDate = c.DateTime(nullable: false),
                        IssuedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DriverLicenseId = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                        BithDate = c.DateTime(nullable: false),
                        EntryState = c.Int(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.DriverVoyageRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DriverId = c.Int(nullable: false),
                        VoyageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .ForeignKey("dbo.Voyages", t => t.VoyageId, cascadeDelete: true)
                .Index(t => t.DriverId)
                .Index(t => t.VoyageId);
            
            CreateTable(
                "dbo.Voyages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        StartPoint_Name = c.String(),
                        StartPoint_Latitude = c.Double(nullable: false),
                        StartPoint_Longitude = c.Double(nullable: false),
                        EndPoint_Name = c.String(),
                        EndPoint_Latitude = c.Double(nullable: false),
                        EndPoint_Longitude = c.Double(nullable: false),
                        RequestedStartTime = c.DateTime(nullable: false),
                        RequestedEndTime = c.DateTime(nullable: false),
                        DriverId = c.Int(),
                        VehicleId = c.Int(nullable: false),
                        LifeCycle_Opened = c.DateTime(),
                        LifeCycle_Acceped = c.DateTime(),
                        LifeCycle_ProcessingStart = c.DateTime(),
                        LifeCycle_Succeded = c.DateTime(),
                        LifeCycle_Canceled = c.DateTime(),
                        EntryState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.DriverId)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SeatsNumber = c.Int(nullable: false),
                        Dimensions_Length = c.Double(nullable: false),
                        Dimensions_Width = c.Double(nullable: false),
                        Dimensions_Height = c.Double(nullable: false),
                        VehicleClassId = c.Int(nullable: false),
                        DriveId = c.Int(nullable: false),
                        Photo_Type = c.String(),
                        Photo_Bytes = c.Binary(),
                        EntryState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VehicleClasses", t => t.VehicleClassId, cascadeDelete: true)
                .ForeignKey("dbo.Drives", t => t.DriveId, cascadeDelete: true)
                .Index(t => t.VehicleClassId)
                .Index(t => t.DriveId);
            
            CreateTable(
                "dbo.VehicleClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        EntryState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Drives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Volume = c.Double(nullable: false),
                        DriveType = c.Int(nullable: false),
                        FuelTypeId = c.Int(nullable: false),
                        CylindersNumber = c.Int(nullable: false),
                        MaxSpeed = c.Int(nullable: false),
                        AccelerationTime = c.Double(nullable: false),
                        EntryState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FuelTypes", t => t.FuelTypeId, cascadeDelete: true)
                .Index(t => t.FuelTypeId);
            
            CreateTable(
                "dbo.FuelTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EntryState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nickname = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        ConfirmationToken = c.String(),
                        IsConfirmed = c.Boolean(nullable: false),
                        RoleId = c.Int(nullable: false),
                        EntryState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleClassDriverLicenses",
                c => new
                    {
                        VehicleClass_Id = c.Int(nullable: false),
                        DriverLicense_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VehicleClass_Id, t.DriverLicense_Id })
                .ForeignKey("dbo.VehicleClasses", t => t.VehicleClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.DriverLicenses", t => t.DriverLicense_Id, cascadeDelete: true)
                .Index(t => t.VehicleClass_Id)
                .Index(t => t.DriverLicense_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Drivers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Voyages", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "DriveId", "dbo.Drives");
            DropForeignKey("dbo.Drives", "FuelTypeId", "dbo.FuelTypes");
            DropForeignKey("dbo.Vehicles", "VehicleClassId", "dbo.VehicleClasses");
            DropForeignKey("dbo.VehicleClassDriverLicenses", "DriverLicense_Id", "dbo.DriverLicenses");
            DropForeignKey("dbo.VehicleClassDriverLicenses", "VehicleClass_Id", "dbo.VehicleClasses");
            DropForeignKey("dbo.DriverVoyageRequests", "VoyageId", "dbo.Voyages");
            DropForeignKey("dbo.Voyages", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DriverVoyageRequests", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DriverLicenses", "Id", "dbo.Drivers");
            DropIndex("dbo.VehicleClassDriverLicenses", new[] { "DriverLicense_Id" });
            DropIndex("dbo.VehicleClassDriverLicenses", new[] { "VehicleClass_Id" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Drives", new[] { "FuelTypeId" });
            DropIndex("dbo.Vehicles", new[] { "DriveId" });
            DropIndex("dbo.Vehicles", new[] { "VehicleClassId" });
            DropIndex("dbo.Voyages", new[] { "VehicleId" });
            DropIndex("dbo.Voyages", new[] { "DriverId" });
            DropIndex("dbo.DriverVoyageRequests", new[] { "VoyageId" });
            DropIndex("dbo.DriverVoyageRequests", new[] { "DriverId" });
            DropIndex("dbo.Drivers", new[] { "User_Id" });
            DropIndex("dbo.DriverLicenses", new[] { "Id" });
            DropTable("dbo.VehicleClassDriverLicenses");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.FuelTypes");
            DropTable("dbo.Drives");
            DropTable("dbo.VehicleClasses");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Voyages");
            DropTable("dbo.DriverVoyageRequests");
            DropTable("dbo.Drivers");
            DropTable("dbo.DriverLicenses");
        }
    }
}
