using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationGateway.Persistence.Migrations
{
    public partial class added_expire_in_keyDto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransformerTemplate",
                table: "Transformers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "TemplateName",
                table: "Transformers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ObjectName",
                table: "Snapshot",
                type: "character varying(40)",
                unicode: false,
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(40)",
                oldUnicode: false,
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "ObjectKey",
                table: "Snapshot",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "JsonData",
                table: "Snapshot",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Gateway",
                table: "Snapshot",
                type: "character varying(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "PolicyDtos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PolicyDtos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AuthType",
                table: "PolicyDtos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<List<string>>(
                name: "Apis",
                table: "PolicyDtos",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<List<string>>(
                name: "Policies",
                table: "KeyDtos",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "KeyName",
                table: "KeyDtos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "KeyDtos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "ApiDtos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "TargetUrl",
                table: "ApiDtos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApiDtos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("176d16be-6a5e-4914-8939-58cac1f7e0f0"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 15, 16, 40, 210, DateTimeKind.Utc).AddTicks(3381));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("31ea7c6d-d731-47c4-af4a-155baf2e2ed4"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 15, 16, 40, 209, DateTimeKind.Utc).AddTicks(7810));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("3f243dd1-644e-410f-93d0-e7979be9d629"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 15, 16, 40, 209, DateTimeKind.Utc).AddTicks(9504));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("63efdd05-a2b8-44f8-9589-86380a7052a1"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 15, 16, 40, 210, DateTimeKind.Utc).AddTicks(4599));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("79ab4897-947c-4638-8d38-526ac28c5bfd"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 15, 16, 40, 210, DateTimeKind.Utc).AddTicks(2065));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 15, 16, 40, 209, DateTimeKind.Utc).AddTicks(6225));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("c8a540f9-0601-4dfb-b4e6-4adac1d52123"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 15, 16, 40, 210, DateTimeKind.Utc).AddTicks(781));

            migrationBuilder.UpdateData(
                table: "Transformers",
                keyColumn: "TransformerId",
                keyValue: new Guid("d37832b5-8400-4a80-90b0-51b07dfaaf4a"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 8, 15, 16, 40, 210, DateTimeKind.Utc).AddTicks(6094));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expires",
                table: "KeyDtos");

            migrationBuilder.AlterColumn<string>(
                name: "TransformerTemplate",
                table: "Transformers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TemplateName",
                table: "Transformers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ObjectName",
                table: "Snapshot",
                type: "character varying(40)",
                unicode: false,
                maxLength: 40,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(40)",
                oldUnicode: false,
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ObjectKey",
                table: "Snapshot",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JsonData",
                table: "Snapshot",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gateway",
                table: "Snapshot",
                type: "character varying(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "PolicyDtos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PolicyDtos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthType",
                table: "PolicyDtos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<string>>(
                name: "Apis",
                table: "PolicyDtos",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<string>>(
                name: "Policies",
                table: "KeyDtos",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KeyName",
                table: "KeyDtos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "ApiDtos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TargetUrl",
                table: "ApiDtos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApiDtos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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
    }
}
