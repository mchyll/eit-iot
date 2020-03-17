using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EitIotService.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "SensorDevices",
				columns: table => new
				{
					DeviceId = table.Column<string>(nullable: false),
					CollectionId = table.Column<string>(nullable: true),
					Imei = table.Column<string>(nullable: true),
					Imsi = table.Column<string>(nullable: true),
					Tags = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SensorDevices", x => x.DeviceId);
				});

			migrationBuilder.CreateTable(
				name: "SensorDatas",
				columns: table => new
				{
					Id = table.Column<long>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					DeviceId = table.Column<string>(nullable: true),
					Timestamp = table.Column<DateTimeOffset>(nullable: false),
					FillContentMeters = table.Column<float>(nullable: false),
					Temperature = table.Column<float>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SensorDatas", x => x.Id);
					table.ForeignKey(
						name: "FK_SensorDatas_SensorDevices_DeviceId",
						column: x => x.DeviceId,
						principalTable: "SensorDevices",
						principalColumn: "DeviceId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_SensorDatas_DeviceId",
				table: "SensorDatas",
				column: "DeviceId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "SensorDatas");

			migrationBuilder.DropTable(
				name: "SensorDevices");
		}
	}
}
