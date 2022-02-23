using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApplicationGateway.Persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Snapshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Gateway = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    ObjectName = table.Column<string>(type: "character varying(40)", unicode: false, maxLength: 40, nullable: false),
                    ObjectKey = table.Column<string>(type: "text", nullable: false),
                    JsonData = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Comment = table.Column<string>(type: "character varying(450)", unicode: false, maxLength: 450, nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(450)", unicode: false, maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "character varying(450)", unicode: false, maxLength: 450, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snapshot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transformers",
                columns: table => new
                {
                    TransformerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateName = table.Column<string>(type: "text", nullable: false),
                    TransformerTemplate = table.Column<string>(type: "text", nullable: false),
                    Gateway = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transformers", x => x.TransformerId);
                });

            migrationBuilder.InsertData(
                table: "Transformers",
                columns: new[] { "TransformerId", "CreatedBy", "CreatedDate", "Gateway", "LastModifiedBy", "LastModifiedDate", "TemplateName", "TransformerTemplate" },
                values: new object[] { new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tyk", null, null, "CREATEAPI_TEMPLATE", "{\r\n  \"name\": \"#valueof(Name)\",\r\n  \"use_keyless\": true,\r\n  \"active\": true,\r\n  \"proxy\": {\r\n    \"listen_path\": \"#valueof(ListenPath)\",\r\n    \"target_url\": \"#valueof(TargetUrl)\",\r\n    \"strip_listen_path\": true\r\n  },\r\n  \"version_data\": {\r\n    \"not_versioned\": true,\r\n    \"default_version\": \"Default\",\r\n    \"versions\": {\r\n      \"Default\": {\r\n        \"name\": \"Default\",\r\n        \"use_extended_paths\": true\r\n      }\r\n    }\r\n  }\r\n}" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Snapshot");

            migrationBuilder.DropTable(
                name: "Transformers");
        }
    }
}
