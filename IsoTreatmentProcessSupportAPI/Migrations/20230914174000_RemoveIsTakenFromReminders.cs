using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IsoTreatmentProcessSupportAPI.Migrations
{
    public partial class RemoveIsTakenFromReminders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTaken",
                table: "Reminders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTaken",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
