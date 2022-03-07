﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApplicationGateway.Persistence.Migrations
{
    public partial class initialmigrations : Migration
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
                values: new object[,]
                {
                    { new Guid("176d16be-6a5e-4914-8939-58cac1f7e0f0"), null, new DateTime(2022, 3, 3, 13, 45, 56, 69, DateTimeKind.Utc).AddTicks(2605), "Tyk", null, null, "GETKEY_TEMPLATE", "{\r\n  \"KeyId\": \"#valueof(key)\",\r\n  \"Rate\": \"#valueof(rate)\",\r\n  \"Per\": \"#valueof(per)\",\r\n  \"ThrottleInterval\": \"#valueof(throttle_interval)\",\r\n  \"ThrottleRetries\": \"#valueof(throttle_retry_limit)\",\r\n  \"Expires\": \"#valueof(expires)\",\r\n  \"Quota\": \"#valueof(quota_max)\",\r\n  \"QuotaRenewalRate\": \"#valueof(quota_renewal_rate)\",\r\n  \"IsInActive\": \"#valueof(is_inactive)\",\r\n  \"AccessRights\": null,\r\n  \"Policies\": null\r\n}" },
                    { new Guid("31ea7c6d-d731-47c4-af4a-155baf2e2ed4"), null, new DateTime(2022, 3, 3, 13, 45, 56, 68, DateTimeKind.Utc).AddTicks(7197), "Tyk", null, null, "CREATEAPI_TEMPLATE", "{\r\n  \"name\": \"#valueof(Name)\",\r\n  \"use_keyless\": true,\r\n  \"active\": true,\r\n  \"proxy\": {\r\n    \"listen_path\": \"#valueof(ListenPath)\",\r\n    \"target_url\": \"#valueof(TargetUrl)\",\r\n    \"strip_listen_path\": true\r\n  },\r\n  \"version_data\": {\r\n    \"not_versioned\": true,\r\n    \"default_version\": \"Default\",\r\n    \"versions\": {\r\n      \"Default\": {\r\n        \"name\": \"Default\",\r\n        \"use_extended_paths\": true\r\n      }\r\n    }\r\n  }\r\n}" },
                    { new Guid("3f243dd1-644e-410f-93d0-e7979be9d629"), null, new DateTime(2022, 3, 3, 13, 45, 56, 68, DateTimeKind.Utc).AddTicks(8814), "Tyk", null, null, "UPDATEAPI_TEMPLATE", "{\r\n  \"name\": \"#valueof(Name)\",\r\n  \"api_id\": \"#valueof(ApiId)\",\r\n  \"use_keyless\": \"#ifcondition(#valueof($.AuthType),keyless,#toboolean(true),#toboolean(false))\",\r\n  \"use_basic\": \"#ifcondition(#valueof($.AuthType),basic,#toboolean(true),#toboolean(false))\",\r\n  \"use_standard_auth\": \"#ifcondition(#valueof($.AuthType),standard,#toboolean(true),#toboolean(false))\",\r\n  \"use_openid\": \"#ifcondition(#valueof($.AuthType),openid,#toboolean(true),#toboolean(false))\",\r\n  \"openid_options\": {\r\n    \"providers\": null,\r\n    \"segregate_by_client\": false\r\n  },\r\n  \"active\": true,\r\n  \"proxy\": {\r\n    \"listen_path\": \"#valueof(ListenPath)\",\r\n    \"target_url\": \"#valueof(TargetUrl)\",\r\n    \"strip_listen_path\": true,\r\n    \"enable_load_balancing\": \"#ifcondition(#length(#valueof($.LoadBalancingTargets)),0,#toboolean(false),#toboolean(true))\",\r\n    \"target_list\": \"#valueof(LoadBalancingTargets)\"\r\n  },\r\n  \"global_rate_limit\": {\r\n    \"rate\": \"#valueof(RateLimit.Rate)\",\r\n    \"per\": \"#valueof(RateLimit.Per)\"\r\n  },\r\n  \"disable_rate_limit\": \"#ifcondition(#valueof($.RateLimit),null,#toboolean(true),#toboolean(false))\",\r\n  \"enable_ip_whitelisting\": \"#ifcondition(#valueof($.Whitelist),null,#toboolean(false),#toboolean(true))\",\r\n  \"allowed_ips\": \"#valueof(Whitelist)\",\r\n  \"enable_ip_blacklisting\": \"#ifcondition(#valueof($.Blacklist),null,#toboolean(false),#toboolean(true))\",\r\n  \"blacklisted_ips\": \"#valueof(Blacklist)\",\r\n  \"definition\": {\r\n    \"location\": \"#valueof(VersioningInfo.Location)\",\r\n    \"key\": \"#valueof(VersioningInfo.Key)\",\r\n    \"strip_path\": \"#ifcondition(#valueof($.Key),null,#toboolean(false),#toboolean(true))\"\r\n  },\r\n  \"version_data\": {\r\n    \"not_versioned\": \"#ifcondition(#length(#valueof($.Versions)),0,#toboolean(true),#toboolean(false))\",\r\n    \"default_version\": \"#valueof(DefaultVersion)\",\r\n    \"versions\": {\r\n      \"Default\": {\r\n        \"name\": \"Default\",\r\n        \"use_extended_paths\": true,\r\n        \"override_target\": \"\"\r\n      }\r\n    }\r\n  },\r\n  \"CORS\": {\r\n    \"enable\": \"#valueof(Enable)\",\r\n    \"allowed_origins\": \"#valueof(Allowed_origins)\",\r\n    \"allowed_methods\": \"#valueof(Allowed_methods)\",\r\n    \"allowed_headers\": \"#valueof(Allowed_headers)\",\r\n    \"exposed_headers\": \"#valueof(Exposed_headers)\",\r\n    \"allow_credentials\": \"#valueof(Allow_credentials)\",\r\n    \"max_age\": \"#valueof(Max_age)\",\r\n    \"options_passthrough\": \"#valueof(Options_passthrough)\",\r\n    \"debug\": \"#valueof(Debug)\"\r\n  }\r\n}" },
                    { new Guid("63efdd05-a2b8-44f8-9589-86380a7052a1"), null, new DateTime(2022, 3, 3, 13, 45, 56, 69, DateTimeKind.Utc).AddTicks(3693), "Tyk", null, null, "CREATEKEY_TEMPLATE", "{\r\n  \"rate\": \"#valueof(Rate)\",\r\n  \"per\": \"#valueof(Per)\",\r\n  \"expires\": \"#valueof(Expires)\",\r\n  \"quota_max\": \"#valueof(Quota)\",\r\n  \"quota_remaining\": \"#valueof(Quota)\",\r\n  \"quota_renewal_rate\": \"#valueof(QuotaRenewalRate)\",\r\n  \"throttle_interval\": \"#valueof(ThrottleInterval)\",\r\n  \"throttle_retry_limit\": \"#valueof(ThrottleRetries)\",\r\n  \"access_rights\": {},\r\n  \"apply_policies\": null\r\n}" },
                    { new Guid("79ab4897-947c-4638-8d38-526ac28c5bfd"), null, new DateTime(2022, 3, 3, 13, 45, 56, 69, DateTimeKind.Utc).AddTicks(1320), "Tyk", null, null, "POLICY_TEMPLATE", "{\r\n  \"access_rights\": {},\r\n  \"active\": \"#valueof(Active)\",\r\n  \"is_inactive \": \"#valueof(KeysInactive)\",\r\n  \"name\": \"#valueof(Name)\",\r\n  \"quota_max\": \"#valueof(MaxQuota)\",\r\n  \"quota_renewal_rate\": \"#valueof(QuotaRate)\",\r\n  \"rate\": \"#valueof(Rate)\",\r\n  \"per\": \"#valueof(Per)\",\r\n  \"throttle_interval\": \"#valueof(ThrottleInterval)\",\r\n  \"throttle_retry_limit\": \"#valueof(ThrottleRetries)\",\r\n  \"state\": \"#valueof(State)\",\r\n  \"tags\": \"#valueof(Tags)\",\r\n  \"key_expires_in\": \"#valueof(KeyExpiresIn)\",\r\n  \"partitions\": \"#valueof(Partitions)\"\r\n}" },
                    { new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"), null, new DateTime(2022, 3, 3, 13, 45, 56, 68, DateTimeKind.Utc).AddTicks(5734), "Tyk", null, null, "GETAPI_TEMPLATE", "{\r\n  \"apiId\": \"#valueof(api_id)\",\r\n  \"name\": \"#valueof(name)\",\r\n  \"listenPath\": \"#valueof(proxy.listen_path)\",\r\n  \"targetUrl\": \"#valueof(proxy.target_url)\",\r\n  \"rateLimit\": {\r\n    \"rate\": \"#valueof(global_rate_limit.rate)\",\r\n    \"per\": \"#valueof(global_rate_limit.per)\"\r\n  },\r\n  \"blacklist\": \"#valueof(blacklisted_ips)\",\r\n  \"whitelist\": \"#valueof(allowed_ips)\",\r\n  \"versioningInfo\": {\r\n    \"location\": \"#valueof(definition.location)\",\r\n    \"key\": \"#valueof(definition.key)\"\r\n  },\r\n  \"defaultVersion\": \"#valueof(version_data.default_version)\",\r\n  \"versions\": [\r\n    {\r\n      \"name\": \"string\",\r\n      \"overrideTarget\": \"string\"\r\n    }\r\n  ],\r\n  \"authType\": \"string\",\r\n  \"openidOptions\": {\r\n    \"providers\": null\r\n  },\r\n  \"loadBalancingTargets\": \"#valueof(target_list)\"\r\n}" },
                    { new Guid("c8a540f9-0601-4dfb-b4e6-4adac1d52123"), null, new DateTime(2022, 3, 3, 13, 45, 56, 69, DateTimeKind.Utc).AddTicks(21), "Tyk", null, null, "GETPOLICY_TEMPLATE", "{\r\n  \"policyId\": \"\",\r\n  \"APIs\": [],\r\n  \"active\": \"#valueof(active)\",\r\n  \"keysInactive \": \"#valueof(is_inactive)\",\r\n  \"name\": \"#valueof(name)\",\r\n  \"maxQuota\": \"#valueof(quota_max)\",\r\n  \"quotaRate\": \"#valueof(quota_renewal_rate)\",\r\n  \"rate\": \"#valueof(rate)\",\r\n  \"per\": \"#valueof(per)\",\r\n  \"throttleInterval\": \"#valueof(throttle_interval)\",\r\n  \"throttleRetries\": \"#valueof(throttle_retry_limit)\",\r\n  \"state\": \"#valueof(state)\",\r\n  \"tags\": \"#valueof(tags)\",\r\n  \"partitions\": \"#valueof(partitions)\"\r\n}" },
                    { new Guid("d37832b5-8400-4a80-90b0-51b07dfaaf4a"), null, new DateTime(2022, 3, 3, 13, 45, 56, 69, DateTimeKind.Utc).AddTicks(4787), "Tyk", null, null, "UPDATEKEY_TEMPLATE", "{\r\n  \"rate\": \"#valueof(Rate)\",\r\n  \"per\": \"#valueof(Per)\",\r\n  \"expires\": \"#valueof(Expires)\",\r\n  \"quota_max\": \"#valueof(Quota)\",\r\n  \"quota_remaining\": \"#valueof(Quota)\",\r\n  \"quota_renewal_rate\": \"#valueof(QuotaRenewalRate)\",\r\n  \"throttle_interval\": \"#valueof(ThrottleInterval)\",\r\n  \"throttle_retry_limit\": \"#valueof(ThrottleRetries)\",\r\n  \"is_inactive\": \"#valueof(IsInActive)\",\r\n  \"access_rights\": {},\r\n  \"apply_policies\": null\r\n}" }
                });
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
