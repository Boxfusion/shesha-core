using FluentMigrator;
using Shesha.FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shesha.Migrations
{
    [Migration(20210724114400)]
    public class M20210724114400 : Migration
    {
        public override void Up()
        {
            Alter.Table("Core_Addresses")
                 .AddColumn("BuildingNameUnitNumber").AsString().Nullable();
        }
        public override void Down()
        {
            throw new NotImplementedException();
        }

    }
}