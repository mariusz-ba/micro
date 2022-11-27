using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro.Examples.Simple.Products.Persistence.Migrations
{
    public partial class AddBackgroundMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Micro");

            migrationBuilder.CreateTable(
                name: "BackgroundJobs",
                schema: "Micro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Queue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RetryAttempt = table.Column<int>(type: "int", nullable: false),
                    RetryMaxCount = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvisibleUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessingDuration = table.Column<long>(type: "bigint", nullable: true),
                    ServerId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundJobs_State_ProcessedAt",
                schema: "Micro",
                table: "BackgroundJobs",
                columns: new[] { "State", "ProcessedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundJobs_State_Queue_InvisibleUntil_CreatedAt",
                schema: "Micro",
                table: "BackgroundJobs",
                columns: new[] { "State", "Queue", "InvisibleUntil", "CreatedAt" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackgroundJobs",
                schema: "Micro");
        }
    }
}
