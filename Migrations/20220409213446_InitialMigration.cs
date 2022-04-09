using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Event.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Event");

            migrationBuilder.CreateTable(
                name: "Organisations",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Price",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegularPrice = table.Column<double>(type: "double precision", nullable: false),
                    MemberPrice = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Salt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PriceId = table.Column<int>(type: "integer", nullable: true),
                    OrganisationId = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "Event",
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_Price_PriceId",
                        column: x => x.PriceId,
                        principalSchema: "Event",
                        principalTable: "Price",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrganisationUser",
                schema: "Event",
                columns: table => new
                {
                    MembersId = table.Column<int>(type: "integer", nullable: false),
                    OrganisationsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationUser", x => new { x.MembersId, x.OrganisationsId });
                    table.ForeignKey(
                        name: "FK_OrganisationUser_Organisations_OrganisationsId",
                        column: x => x.OrganisationsId,
                        principalSchema: "Event",
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganisationUser_Users_MembersId",
                        column: x => x.MembersId,
                        principalSchema: "Event",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParticipantCount = table.Column<int>(type: "integer", nullable: false),
                    PaidParticipants = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    TieBreaker = table.Column<double>(type: "double precision", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "Event",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganisationId",
                schema: "Event",
                table: "Events",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_PriceId",
                schema: "Event",
                table: "Events",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationUser_OrganisationsId",
                schema: "Event",
                table: "OrganisationUser",
                column: "OrganisationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_EventId",
                schema: "Event",
                table: "Teams",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganisationUser",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "Organisations",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "Price",
                schema: "Event");
        }
    }
}
