using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationGateway.Persistence.Migrations
{
    public partial class dto_tables_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiDtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TargetUrl = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiDtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyDtos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    KeyName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Policies = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyDtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolicyDtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Apis = table.Column<List<string>>(type: "text[]", nullable: false),
                    AuthType = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyDtos", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("176d16be-6a5e-4914-8939-58cac1f7e0f0"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 10, 34, 58, 909, DateTimeKind.Utc).AddTicks(7003));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("31ea7c6d-d731-47c4-af4a-155baf2e2ed4"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 10, 34, 58, 909, DateTimeKind.Utc).AddTicks(3538));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("3f243dd1-644e-410f-93d0-e7979be9d629"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 10, 34, 58, 909, DateTimeKind.Utc).AddTicks(4500));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("63efdd05-a2b8-44f8-9589-86380a7052a1"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 10, 34, 58, 909, DateTimeKind.Utc).AddTicks(7785));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("79ab4897-947c-4638-8d38-526ac28c5bfd"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 10, 34, 58, 909, DateTimeKind.Utc).AddTicks(6186));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 10, 34, 58, 909, DateTimeKind.Utc).AddTicks(2536));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("c8a540f9-0601-4dfb-b4e6-4adac1d52123"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 10, 34, 58, 909, DateTimeKind.Utc).AddTicks(5275));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("d37832b5-8400-4a80-90b0-51b07dfaaf4a"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 10, 34, 58, 909, DateTimeKind.Utc).AddTicks(8547));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiDtos");

            migrationBuilder.DropTable(
                name: "KeyDtos");

            migrationBuilder.DropTable(
                name: "PolicyDtos");

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("176d16be-6a5e-4914-8939-58cac1f7e0f0"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 3, 13, 45, 56, 69, DateTimeKind.Utc).AddTicks(2605));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("31ea7c6d-d731-47c4-af4a-155baf2e2ed4"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 3, 13, 45, 56, 68, DateTimeKind.Utc).AddTicks(7197));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("3f243dd1-644e-410f-93d0-e7979be9d629"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 3, 13, 45, 56, 68, DateTimeKind.Utc).AddTicks(8814));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("63efdd05-a2b8-44f8-9589-86380a7052a1"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 3, 13, 45, 56, 69, DateTimeKind.Utc).AddTicks(3693));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("79ab4897-947c-4638-8d38-526ac28c5bfd"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 3, 13, 45, 56, 69, DateTimeKind.Utc).AddTicks(1320));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 3, 13, 45, 56, 68, DateTimeKind.Utc).AddTicks(5734));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("c8a540f9-0601-4dfb-b4e6-4adac1d52123"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 3, 13, 45, 56, 69, DateTimeKind.Utc).AddTicks(21));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("d37832b5-8400-4a80-90b0-51b07dfaaf4a"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 3, 13, 45, 56, 69, DateTimeKind.Utc).AddTicks(4787));
        }
    }
}
