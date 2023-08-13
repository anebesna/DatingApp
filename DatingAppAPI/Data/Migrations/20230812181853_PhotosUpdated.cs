﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingAppAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class PhotosUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "Photos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "Photos");
        }
    }
}
