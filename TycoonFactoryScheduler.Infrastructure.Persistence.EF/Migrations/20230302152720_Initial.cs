using Microsoft.EntityFrameworkCore.Migrations;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivityType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityWorker",
                columns: table => new
                {
                    ActivitiesId = table.Column<int>(type: "int", nullable: false),
                    WorkersId = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityWorker", x => new { x.ActivitiesId, x.WorkersId });
                    table.ForeignKey(
                        name: "FK_ActivityWorker_Activities_ActivitiesId",
                        column: x => x.ActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityWorker_Workers_WorkersId",
                        column: x => x.WorkersId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "ActivityType", "Description", "End", "Start" },
                values: new object[,]
                {
                    { 1, 1, "Flux capacitor", new DateTime(2023, 3, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, "Fusion engine", new DateTime(2023, 3, 3, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, "Bionic arm", new DateTime(2023, 3, 3, 4, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, "Thought sensor", new DateTime(2023, 3, 3, 5, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, "Interestellar jump engine", new DateTime(2023, 3, 3, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, "Synthetic skin", new DateTime(2023, 3, 3, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1, "Bionic leg", new DateTime(2023, 3, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 2, "Autonomous car", new DateTime(2023, 3, 3, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 2, "Time machine", new DateTime(2023, 3, 3, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 4, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 2, "Battlestar", new DateTime(2023, 3, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 6, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 2, "Flying car", new DateTime(2023, 3, 3, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 2, "Laser gun", new DateTime(2023, 3, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 5, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 2, "Augmented reality glasses", new DateTime(2023, 3, 3, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 2, "Teletransporter", new DateTime(2023, 3, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 4, 3, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 2, "Laser sword", new DateTime(2023, 3, 4, 4, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 2, "Hoverboard", new DateTime(2023, 3, 4, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 3, 17, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "Id", "CreationDate", "Manufacturer", "Model" },
                values: new object[,]
                {
                    { "A", new DateTime(2008, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "WALL-E", "WALL-E" },
                    { "B", new DateTime(2004, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "I, Robot", "NS-5" },
                    { "C", new DateTime(1984, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Terminator", "T-800" },
                    { "D", new DateTime(1991, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Terminator", "T-1000" },
                    { "E", new DateTime(2003, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Battlestar Galactica", "Cavil" },
                    { "F", new DateTime(2003, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Battlestar Galactica", "Leoben" },
                    { "G", new DateTime(2003, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Battlestar Galactica", "D'Anna" },
                    { "H", new DateTime(2003, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Battlestar Galactica", "Simon" },
                    { "I", new DateTime(2003, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Battlestar Galactica", "Aaron" },
                    { "J", new DateTime(2003, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Battlestar Galactica", "Caprica" },
                    { "K", new DateTime(2003, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Battlestar Galactica", "Daniel" },
                    { "L", new DateTime(2003, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Battlestar Galactica", "Boomer" },
                    { "M", new DateTime(2016, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Westworld", "Dolores" },
                    { "N", new DateTime(2016, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Westworld", "Maeve" },
                    { "O", new DateTime(2016, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Westworld", "Bernard" },
                    { "P", new DateTime(2016, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Westworld", "Hector" },
                    { "Q", new DateTime(2016, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Westworld", "Teddy" },
                    { "R", new DateTime(1982, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Blade Runner", "Nexus" },
                    { "S", new DateTime(2014, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Big Hero 6", "Baymax" }
                });

            migrationBuilder.InsertData(
                table: "ActivityWorker",
                columns: new[] { "ActivitiesId", "WorkersId" },
                values: new object[,]
                {
                    { 1, "A" },
                    { 2, "B" },
                    { 3, "C" },
                    { 4, "D" },
                    { 5, "E" },
                    { 6, "F" },
                    { 7, "G" },
                    { 8, "A" },
                    { 8, "H" },
                    { 8, "I" },
                    { 9, "B" },
                    { 9, "J" },
                    { 9, "K" },
                    { 9, "L" },
                    { 10, "C" },
                    { 11, "D" },
                    { 11, "M" },
                    { 11, "N" },
                    { 11, "O" },
                    { 11, "P" },
                    { 12, "E" },
                    { 12, "Q" },
                    { 12, "R" },
                    { 12, "S" },
                    { 13, "F" },
                    { 13, "G" },
                    { 14, "A" },
                    { 14, "C" },
                    { 14, "D" },
                    { 14, "L" },
                    { 15, "B" },
                    { 15, "F" },
                    { 15, "G" },
                    { 15, "R" },
                    { 16, "E" },
                    { 16, "N" },
                    { 16, "S" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityWorker_WorkersId",
                table: "ActivityWorker",
                column: "WorkersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityWorker");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
