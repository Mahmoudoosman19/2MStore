﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class add2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryMethods_City_cityId",
                table: "DeliveryMethods");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryMethods_cityId",
                table: "DeliveryMethods");

            migrationBuilder.DropColumn(
                name: "cityId",
                table: "DeliveryMethods");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cityId",
                table: "DeliveryMethods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryMethods_cityId",
                table: "DeliveryMethods",
                column: "cityId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryMethods_City_cityId",
                table: "DeliveryMethods",
                column: "cityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
