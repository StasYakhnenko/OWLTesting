using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AddedfewfieldstoTestResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ResultGrade",
                table: "TestResults",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeEnd",
                table: "TestResults",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeEnd",
                table: "TestResults");

            migrationBuilder.AlterColumn<int>(
                name: "ResultGrade",
                table: "TestResults",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
