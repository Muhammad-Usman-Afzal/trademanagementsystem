using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Party",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "varchar(30)", nullable: true),
                    CompanyContact = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    CompanyEmail = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(125)", nullable: true),
                    IsRegisterd = table.Column<bool>(type: "bit", nullable: false),
                    NTN = table.Column<string>(type: "nvarchar(9)", nullable: true),
                    STRNo = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    FocalPersonName = table.Column<string>(type: "varchar(30)", nullable: true),
                    FocalPersonContact = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    FocalPersonEmail = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Party", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PONo = table.Column<string>(type: "varchar(15)", nullable: true),
                    PODate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VendorName = table.Column<string>(type: "varchar(30)", nullable: true),
                    DeliveryFrom = table.Column<string>(type: "varchar(30)", nullable: true),
                    PaymentMode = table.Column<string>(type: "varchar(20)", nullable: true),
                    ShipmentType = table.Column<string>(type: "varchar(20)", nullable: true),
                    GrossAmount = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    TaxRate = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    TaxAmount = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    NetAmount = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "varchar(30)", nullable: true),
                    Qty = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    Rate = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetail_PurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrder",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetail_PurchaseOrderId",
                table: "PurchaseOrderDetail",
                column: "PurchaseOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Party");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetail");

            migrationBuilder.DropTable(
                name: "PurchaseOrder");
        }
    }
}
