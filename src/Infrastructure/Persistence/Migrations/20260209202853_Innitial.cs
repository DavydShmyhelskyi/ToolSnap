using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Innitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "action_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_action_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "location_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_location_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "models",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "photo_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_photo_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tool_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tool_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tool_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tool_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "photo_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    action_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_photo_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_photo_sessions_action_types_action_type_id",
                        column: x => x.action_type_id,
                        principalTable: "action_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    location_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address = table.Column<string>(type: "varchar(500)", nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_locations", x => x.id);
                    table.ForeignKey(
                        name: "fk_locations_location_types_location_type_id",
                        column: x => x.location_type_id,
                        principalTable: "location_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "varchar(255)", nullable: false),
                    email = table.Column<string>(type: "varchar(255)", nullable: false),
                    confirmed_email = table.Column<bool>(type: "boolean", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tools",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tool_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    brand_id = table.Column<Guid>(type: "uuid", nullable: true),
                    model_id = table.Column<Guid>(type: "uuid", nullable: true),
                    serial_number = table.Column<string>(type: "varchar(255)", nullable: true),
                    tool_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tools", x => x.id);
                    table.ForeignKey(
                        name: "fk_tools_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tools_models_model_id",
                        column: x => x.model_id,
                        principalTable: "models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tools_tool_statuses_tool_status_id",
                        column: x => x.tool_status_id,
                        principalTable: "tool_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tools_tool_types_tool_type_id",
                        column: x => x.tool_type_id,
                        principalTable: "tool_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "photos_for_detection",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    photo_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_name = table.Column<string>(type: "varchar(500)", nullable: false),
                    upload_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_photos_for_detection", x => x.id);
                    table.ForeignKey(
                        name: "fk_photos_for_detection_photo_sessions_photo_session_id",
                        column: x => x.photo_session_id,
                        principalTable: "photo_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tool_photos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_name = table.Column<string>(type: "varchar(500)", nullable: false),
                    tool_id = table.Column<Guid>(type: "uuid", nullable: false),
                    upload_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    photo_type_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tool_photos", x => x.id);
                    table.ForeignKey(
                        name: "fk_tool_photos_photo_types_photo_type_id",
                        column: x => x.photo_type_id,
                        principalTable: "photo_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tool_photos_tools_tool_id",
                        column: x => x.tool_id,
                        principalTable: "tools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "detected_tools",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    photo_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tool_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    brand_id = table.Column<Guid>(type: "uuid", nullable: true),
                    model_id = table.Column<Guid>(type: "uuid", nullable: true),
                    serial_number = table.Column<string>(type: "varchar(255)", nullable: true),
                    confidence = table.Column<float>(type: "real", nullable: false),
                    red_flagged = table.Column<bool>(type: "boolean", nullable: false),
                    tool_assignment_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_detected_tools", x => x.id);
                    table.ForeignKey(
                        name: "fk_detected_tools_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_detected_tools_models_model_id",
                        column: x => x.model_id,
                        principalTable: "models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_detected_tools_photo_sessions_photo_session_id",
                        column: x => x.photo_session_id,
                        principalTable: "photo_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_detected_tools_tool_types_tool_type_id",
                        column: x => x.tool_type_id,
                        principalTable: "tool_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tool_assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    taken_detected_tool_id = table.Column<Guid>(type: "uuid", nullable: false),
                    returned_detected_tool_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tool_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    taken_location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    returned_location_id = table.Column<Guid>(type: "uuid", nullable: true),
                    taken_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    returned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tool_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_tool_assignments_detected_tools_returned_detected_tool_id",
                        column: x => x.returned_detected_tool_id,
                        principalTable: "detected_tools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tool_assignments_detected_tools_taken_detected_tool_id",
                        column: x => x.taken_detected_tool_id,
                        principalTable: "detected_tools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tool_assignments_locations_returned_location_id",
                        column: x => x.returned_location_id,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tool_assignments_locations_taken_location_id",
                        column: x => x.taken_location_id,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tool_assignments_tools_tool_id",
                        column: x => x.tool_id,
                        principalTable: "tools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tool_assignments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_detected_tools_brand_id",
                table: "detected_tools",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_detected_tools_model_id",
                table: "detected_tools",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "ix_detected_tools_photo_session_id",
                table: "detected_tools",
                column: "photo_session_id");

            migrationBuilder.CreateIndex(
                name: "ix_detected_tools_tool_assignment_id",
                table: "detected_tools",
                column: "tool_assignment_id");

            migrationBuilder.CreateIndex(
                name: "ix_detected_tools_tool_type_id",
                table: "detected_tools",
                column: "tool_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_locations_location_type_id",
                table: "locations",
                column: "location_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_photo_sessions_action_type_id",
                table: "photo_sessions",
                column: "action_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_photos_for_detection_photo_session_id",
                table: "photos_for_detection",
                column: "photo_session_id");

            migrationBuilder.CreateIndex(
                name: "ix_tool_assignments_returned_detected_tool_id",
                table: "tool_assignments",
                column: "returned_detected_tool_id");

            migrationBuilder.CreateIndex(
                name: "ix_tool_assignments_returned_location_id",
                table: "tool_assignments",
                column: "returned_location_id");

            migrationBuilder.CreateIndex(
                name: "ix_tool_assignments_taken_detected_tool_id",
                table: "tool_assignments",
                column: "taken_detected_tool_id");

            migrationBuilder.CreateIndex(
                name: "ix_tool_assignments_taken_location_id",
                table: "tool_assignments",
                column: "taken_location_id");

            migrationBuilder.CreateIndex(
                name: "ix_tool_assignments_tool_id",
                table: "tool_assignments",
                column: "tool_id");

            migrationBuilder.CreateIndex(
                name: "ix_tool_assignments_user_id",
                table: "tool_assignments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tool_photos_photo_type_id",
                table: "tool_photos",
                column: "photo_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_tool_photos_tool_id",
                table: "tool_photos",
                column: "tool_id");

            migrationBuilder.CreateIndex(
                name: "ix_tools_brand_id",
                table: "tools",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_tools_model_id",
                table: "tools",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "ix_tools_tool_status_id",
                table: "tools",
                column: "tool_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_tools_tool_type_id",
                table: "tools",
                column: "tool_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_detected_tools_tool_assignments_tool_assignment_id",
                table: "detected_tools",
                column: "tool_assignment_id",
                principalTable: "tool_assignments",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_detected_tools_brands_brand_id",
                table: "detected_tools");

            migrationBuilder.DropForeignKey(
                name: "fk_tools_brands_brand_id",
                table: "tools");

            migrationBuilder.DropForeignKey(
                name: "fk_detected_tools_models_model_id",
                table: "detected_tools");

            migrationBuilder.DropForeignKey(
                name: "fk_tools_models_model_id",
                table: "tools");

            migrationBuilder.DropForeignKey(
                name: "fk_detected_tools_photo_sessions_photo_session_id",
                table: "detected_tools");

            migrationBuilder.DropForeignKey(
                name: "fk_detected_tools_tool_assignments_tool_assignment_id",
                table: "detected_tools");

            migrationBuilder.DropTable(
                name: "photos_for_detection");

            migrationBuilder.DropTable(
                name: "tool_photos");

            migrationBuilder.DropTable(
                name: "photo_types");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "models");

            migrationBuilder.DropTable(
                name: "photo_sessions");

            migrationBuilder.DropTable(
                name: "action_types");

            migrationBuilder.DropTable(
                name: "tool_assignments");

            migrationBuilder.DropTable(
                name: "detected_tools");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "tools");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "location_types");

            migrationBuilder.DropTable(
                name: "tool_statuses");

            migrationBuilder.DropTable(
                name: "tool_types");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
