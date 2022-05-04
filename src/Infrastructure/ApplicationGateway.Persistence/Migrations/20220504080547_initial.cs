using System;
using System.Collections.Generic;
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
                name: "Apis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    TargetUrl = table.Column<string>(type: "text", nullable: true),
                    AuthType = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    KeyName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Policies = table.Column<List<string>>(type: "text[]", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Apis = table.Column<List<string>>(type: "text[]", nullable: true),
                    AuthType = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Snapshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Gateway = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    ObjectName = table.Column<string>(type: "character varying(40)", unicode: false, maxLength: 40, nullable: true),
                    ObjectKey = table.Column<string>(type: "text", nullable: true),
                    JsonData = table.Column<string>(type: "text", nullable: true),
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
                    TemplateName = table.Column<string>(type: "text", nullable: true),
                    TransformerTemplate = table.Column<string>(type: "text", nullable: true),
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
                    { new Guid("176d16be-6a5e-4914-8939-58cac1f7e0f0"), null, new DateTime(2022, 5, 4, 8, 5, 47, 121, DateTimeKind.Utc).AddTicks(6577), "Tyk", null, null, "GETKEY_TEMPLATE", "{\r\n  \"KeyId\": \"#valueof(key)\",\r\n  \"KeyName\": \"#valueof(alias)\",\r\n  \"Rate\": \"#valueof(rate)\",\r\n  \"Per\": \"#valueof(per)\",\r\n  \"ThrottleInterval\": \"#valueof(throttle_interval)\",\r\n  \"ThrottleRetries\": \"#valueof(throttle_retry_limit)\",\r\n  \"Expires\": \"#valueof(expires)\",\r\n  \"Quota\": \"#valueof(quota_max)\",\r\n  \"QuotaRenewalRate\": \"#valueof(quota_renewal_rate)\",\r\n  \"IsInActive\": \"#valueof(is_inactive)\",\r\n  \"AccessRights\": null,\r\n  \"Policies\": null,\r\n  \"Tags\": \"#valueof(tags)\"\r\n}" },
                    { new Guid("31ea7c6d-d731-47c4-af4a-155baf2e2ed4"), null, new DateTime(2022, 5, 4, 8, 5, 47, 121, DateTimeKind.Utc).AddTicks(3266), "Tyk", null, null, "CREATEAPI_TEMPLATE", "{\r\n  \"name\": \"#valueof(Name)\",\r\n  \"use_keyless\": true,\r\n  \"active\": \"#valueof(IsActive)\",\r\n  \"proxy\": {\r\n    \"listen_path\": \"#valueof(ListenPath)\",\r\n    \"target_url\": \"#valueof(TargetUrl)\",\r\n    \"strip_listen_path\": \"#valueof(StripListenPath)\"\r\n  },\r\n  \"version_data\": {\r\n    \"not_versioned\": true,\r\n    \"default_version\": \"Default\",\r\n    \"versions\": {\r\n      \"Default\": {\r\n        \"name\": \"Default\",\r\n        \"use_extended_paths\": true\r\n      }\r\n    }\r\n  }\r\n}" },
                    { new Guid("3f243dd1-644e-410f-93d0-e7979be9d629"), null, new DateTime(2022, 5, 4, 8, 5, 47, 121, DateTimeKind.Utc).AddTicks(4205), "Tyk", null, null, "UPDATEAPI_TEMPLATE", "{\r\n  \"name\": \"#valueof(Name)\",\r\n  \"api_id\": \"#valueof(ApiId)\",\r\n  \"use_keyless\": \"#ifcondition(#valueof($.AuthType),keyless,#toboolean(true),#toboolean(false))\",\r\n  \"use_basic\": \"#ifcondition(#valueof($.AuthType),basic,#toboolean(true),#toboolean(false))\",\r\n  \"use_standard_auth\": \"#ifcondition(#valueof($.AuthType),standard,#toboolean(true),#toboolean(false))\",\r\n  \"use_openid\": \"#ifcondition(#valueof($.AuthType),openid,#toboolean(true),#toboolean(false))\",\r\n  \"use_mutual_tls_auth\": \"#valueof(EnableMTLS)\",\r\n  \"client_certificates\": null,\r\n  \"openid_options\": {\r\n    \"providers\": null,\r\n    \"segregate_by_client\": false\r\n  },\r\n  \"auth\": {\r\n    \"use_param\": false,\r\n    \"param_name\": \"\",\r\n    \"use_cookie\": false,\r\n    \"cookie_name\": \"\",\r\n    \"auth_header_name\": \"gateway-authorization\",\r\n    \"use_certificate\": false,\r\n    \"validate_signature\": false,\r\n    \"signature\": {\r\n      \"algorithm\": \"\",\r\n      \"header\": \"\",\r\n      \"secret\": \"\",\r\n      \"allowed_clock_skew\": 0,\r\n      \"error_code\": 0,\r\n      \"error_message\": \"\"\r\n    }\r\n  },\r\n  \"active\": \"#valueof(IsActive)\",\r\n  \"internal\": \"#valueof(IsInternal)\",\r\n  \"protocol\": \"#valueof(Protocol)\",\r\n  \"proxy\": {\r\n    \"listen_path\": \"#valueof(ListenPath)\",\r\n    \"target_url\": \"#valueof(TargetUrl)\",\r\n    \"strip_listen_path\": \"#valueof(StripListenPath)\",\r\n    \"enable_load_balancing\": \"#ifcondition(#length(#valueof($.LoadBalancingTargets)),0,#toboolean(false),#toboolean(true))\",\r\n    \"target_list\": \"#valueof(LoadBalancingTargets)\"\r\n  },\r\n  \"global_rate_limit\": {\r\n    \"rate\": \"#valueof(RateLimit.Rate)\",\r\n    \"per\": \"#valueof(RateLimit.Per)\"\r\n  },\r\n  \"disable_rate_limit\": \"#valueof(RateLimit.IsDisabled)\",\r\n  \"disable_quota\": \"#valueof(IsQuotaDisabled)\",\r\n  \"enable_ip_whitelisting\": \"#ifcondition(#valueof($.Whitelist),null,#toboolean(false),#toboolean(true))\",\r\n  \"allowed_ips\": \"#valueof(Whitelist)\",\r\n  \"enable_ip_blacklisting\": \"#ifcondition(#valueof($.Blacklist),null,#toboolean(false),#toboolean(true))\",\r\n  \"blacklisted_ips\": \"#valueof(Blacklist)\",\r\n  \"definition\": {\r\n    \"location\": \"#valueof(VersioningInfo.Location)\",\r\n    \"key\": \"#valueof(VersioningInfo.Key)\",\r\n    \"strip_path\": \"#ifcondition(#valueof($.Key),null,#toboolean(false),#toboolean(true))\"\r\n  },\r\n  \"version_data\": {\r\n    \"not_versioned\": \"#valueof(IsVersioningDisabled)\",\r\n    \"default_version\": \"#valueof(DefaultVersion)\",\r\n    \"versions\": {\r\n      \"Default\": {\r\n        \"name\": \"Default\",\r\n        \"use_extended_paths\": true,\r\n        \"override_target\": \"\"\r\n      }\r\n    }\r\n  },\r\n  \"CORS\": {\r\n    \"enable\": \"#valueof(CORS.IsEnabled)\",\r\n    \"allowed_origins\": \"#valueof(CORS.AllowedOrigins)\",\r\n    \"allowed_methods\": \"#valueof(CORS.AllowedMethods)\",\r\n    \"allowed_headers\": \"#valueof(CORS.AllowedHeaders)\",\r\n    \"exposed_headers\": \"#valueof(CORS.ExposedHeaders)\",\r\n    \"allow_credentials\": \"#valueof(CORS.AllowCredentials)\",\r\n    \"max_age\": \"#valueof(CORS.MaxAge)\",\r\n    \"options_passthrough\": \"#valueof(CORS.OptionsPassthrough)\",\r\n    \"debug\": \"#valueof(CORS.Debug)\"\r\n  }\r\n}" },
                    { new Guid("63efdd05-a2b8-44f8-9589-86380a7052a1"), null, new DateTime(2022, 5, 4, 8, 5, 47, 121, DateTimeKind.Utc).AddTicks(7319), "Tyk", null, null, "CREATEKEY_TEMPLATE", "{\r\n  \"alias\": \"#valueof(KeyName)\",\r\n  \"rate\": \"#valueof(Rate)\",\r\n  \"per\": \"#valueof(Per)\",\r\n  \"expires\": \"#valueof(Expires)\",\r\n  \"quota_max\": \"#valueof(Quota)\",\r\n  \"quota_remaining\": \"#valueof(Quota)\",\r\n  \"quota_renewal_rate\": \"#valueof(QuotaRenewalRate)\",\r\n  \"throttle_interval\": \"#valueof(ThrottleInterval)\",\r\n  \"throttle_retry_limit\": \"#valueof(ThrottleRetries)\",\r\n  \"tags\": \"#valueof(Tags)\",\r\n  \"access_rights\": {},\r\n  \"apply_policies\": null\r\n}" },
                    { new Guid("79ab4897-947c-4638-8d38-526ac28c5bfd"), null, new DateTime(2022, 5, 4, 8, 5, 47, 121, DateTimeKind.Utc).AddTicks(5799), "Tyk", null, null, "POLICY_TEMPLATE", "{\r\n  \"access_rights\": {},\r\n  \"active\": \"#valueof(Active)\",\r\n  \"is_inactive \": \"#valueof(KeysInactive)\",\r\n  \"name\": \"#valueof(Name)\",\r\n  \"quota_max\": \"#valueof(MaxQuota)\",\r\n  \"quota_renewal_rate\": \"#valueof(QuotaRate)\",\r\n  \"rate\": \"#valueof(Rate)\",\r\n  \"per\": \"#valueof(Per)\",\r\n  \"throttle_interval\": \"#valueof(ThrottleInterval)\",\r\n  \"throttle_retry_limit\": \"#valueof(ThrottleRetries)\",\r\n  \"state\": \"#valueof(State)\",\r\n  \"tags\": \"#valueof(Tags)\",\r\n  \"key_expires_in\": \"#valueof(KeyExpiresIn)\",\r\n  \"partitions\": \"#valueof(Partitions)\"\r\n}" },
                    { new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"), null, new DateTime(2022, 5, 4, 8, 5, 47, 121, DateTimeKind.Utc).AddTicks(2325), "Tyk", null, null, "GETAPI_TEMPLATE", "{\r\n  \"apiId\": \"#valueof(api_id)\",\r\n  \"name\": \"#valueof(name)\",\r\n  \"listenPath\": \"#valueof(proxy.listen_path)\",\r\n  \"stripListenPath\": \"#valueof(proxy.strip_listen_path)\",\r\n  \"targetUrl\": \"#valueof(proxy.target_url)\",\r\n  \"isActive\": \"#valueof(active)\",\r\n  \"isInternal\": \"#valueof(internal)\",\r\n  \"enableMTLS\": \"#valueof(use_mutual_tls_auth)\",\r\n  \"certIds\": \"#valueof(client_certificates)\",\r\n  \"protocol\": \"#valueof(protocol)\",\r\n  \"rateLimit\": {\r\n    \"rate\": \"#valueof(global_rate_limit.rate)\",\r\n    \"per\": \"#valueof(global_rate_limit.per)\",\r\n    \"isDisabled\": \"#valueof(disable_rate_limit)\"\r\n  },\r\n  \"isQuotaDisabled\": \"#valueof(disable_quota)\",\r\n  \"blacklist\": \"#valueof(blacklisted_ips)\",\r\n  \"whitelist\": \"#valueof(allowed_ips)\",\r\n  \"versioningInfo\": {\r\n    \"location\": \"#valueof(definition.location)\",\r\n    \"key\": \"#valueof(definition.key)\"\r\n  },\r\n  \"isVersioningDisabled\": \"#valueof(version_data.not_versioned)\",\r\n  \"defaultVersion\": \"#valueof(version_data.default_version)\",\r\n  \"versions\": [\r\n    {\r\n      \"name\": \"string\",\r\n      \"overrideTarget\": \"string\",\r\n      \"expires\": \"string\"\r\n    }\r\n  ],\r\n  \"authType\": \"string\",\r\n  \"openidOptions\": {\r\n    \"providers\": null\r\n  },\r\n  \"loadBalancingTargets\": \"#valueof(proxy.target_list)\",\r\n  \"CORS\": {\r\n    \"IsEnabled\": \"#valueof(CORS.enable)\",\r\n    \"AllowedOrigins\": \"#valueof(CORS.allowed_origins)\",\r\n    \"AllowedMethods\": \"#valueof(CORS.allowed_methods)\",\r\n    \"AllowedHeaders\": \"#valueof(CORS.allowed_headers)\",\r\n    \"ExposedHeaders\": \"#valueof(CORS.exposed_headers)\",\r\n    \"AllowCredentials\": \"#valueof(CORS.allow_credentials)\",\r\n    \"MaxAge\": \"#valueof(CORS.max_age)\",\r\n    \"OptionsPassthrough\": \"#valueof(CORS.options_passthrough)\",\r\n    \"Debug\": \"#valueof(CORS.debug)\"\r\n  }\r\n}" },
                    { new Guid("c8a540f9-0601-4dfb-b4e6-4adac1d52123"), null, new DateTime(2022, 5, 4, 8, 5, 47, 121, DateTimeKind.Utc).AddTicks(5041), "Tyk", null, null, "GETPOLICY_TEMPLATE", "{\r\n  \"policyId\": \"\",\r\n  \"APIs\": [],\r\n  \"active\": \"#valueof(active)\",\r\n  \"keysInactive \": \"#valueof(is_inactive)\",\r\n  \"name\": \"#valueof(name)\",\r\n  \"maxQuota\": \"#valueof(quota_max)\",\r\n  \"quotaRate\": \"#valueof(quota_renewal_rate)\",\r\n  \"rate\": \"#valueof(rate)\",\r\n  \"per\": \"#valueof(per)\",\r\n  \"throttleInterval\": \"#valueof(throttle_interval)\",\r\n  \"throttleRetries\": \"#valueof(throttle_retry_limit)\",\r\n  \"state\": \"#valueof(state)\",\r\n  \"tags\": \"#valueof(tags)\",\r\n  \"partitions\": \"#valueof(partitions)\"\r\n}" },
                    { new Guid("d37832b5-8400-4a80-90b0-51b07dfaaf4a"), null, new DateTime(2022, 5, 4, 8, 5, 47, 121, DateTimeKind.Utc).AddTicks(8178), "Tyk", null, null, "UPDATEKEY_TEMPLATE", "{\r\n  \"alias\": \"#valueof(KeyName)\",\r\n  \"rate\": \"#valueof(Rate)\",\r\n  \"per\": \"#valueof(Per)\",\r\n  \"expires\": \"#valueof(Expires)\",\r\n  \"quota_max\": \"#valueof(Quota)\",\r\n  \"quota_remaining\": \"#valueof(Quota)\",\r\n  \"quota_renewal_rate\": \"#valueof(QuotaRenewalRate)\",\r\n  \"throttle_interval\": \"#valueof(ThrottleInterval)\",\r\n  \"throttle_retry_limit\": \"#valueof(ThrottleRetries)\",\r\n  \"tags\": \"#valueof(Tags)\",\r\n  \"is_inactive\": \"#valueof(IsInActive)\",\r\n  \"access_rights\": {},\r\n  \"apply_policies\": null\r\n}" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apis");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.DropTable(
                name: "Snapshot");

            migrationBuilder.DropTable(
                name: "Transformers");
        }
    }
}
