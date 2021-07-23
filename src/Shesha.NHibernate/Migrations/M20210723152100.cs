using FluentMigrator;
using Shesha.FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shesha.Migrations
{
    [Migration(20210723152100)]
    public class M20210723152100 : Migration
    {
        public override void Up()
        {
            Alter.Table("Core_Persons")
                .AddColumn("PreferredLanguageLkp").AsInt32().Nullable();
        }
        public override void Down()
        {
            throw new NotImplementedException();
        }

    }
}