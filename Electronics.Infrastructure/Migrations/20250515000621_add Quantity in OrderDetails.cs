﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electronics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addQuantityinOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderDetails");
        }
    }
}
