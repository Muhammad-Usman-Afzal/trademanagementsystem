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
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(30)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    FullName = table.Column<string>(type: "varchar(100)", nullable: true),
                    IsSuperUser = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterMenu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "varchar(50)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Party",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "varchar(100)", nullable: true),
                    CompanyContact = table.Column<string>(type: "nvarchar(13)", nullable: true),
                    CompanyEmail = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    IsRegisterd = table.Column<bool>(type: "bit", nullable: false),
                    NTN = table.Column<string>(type: "nvarchar(9)", nullable: true),
                    STRNo = table.Column<string>(type: "nvarchar(17)", nullable: true),
                    FocalPersonName = table.Column<string>(type: "varchar(50)", nullable: true),
                    FocalPersonContact = table.Column<string>(type: "nvarchar(13)", nullable: true),
                    FocalPersonEmail = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PartyType = table.Column<string>(type: "nvarchar(12)", nullable: true),
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
                name: "ProductionOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveredItem = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    ReceivingItem = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    ProductionQty = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    NotSpeciffiedYet = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrderDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PONo = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    PODate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractorName = table.Column<string>(type: "varchar(50)", nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(125)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(125)", nullable: true),
                    Rate = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", nullable: true),
                    TransactionType = table.Column<string>(type: "varchar(20)", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UntilNextContract = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(125)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthorizedScreens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterMenuId = table.Column<int>(type: "int", nullable: false),
                    AuthorizedPermission = table.Column<int>(type: "int", nullable: false),
                    AppUsersId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthorizedScreens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAuthorizedScreens_AppUsers_AppUsersId",
                        column: x => x.AppUsersId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAuthorizedScreens_MasterMenu_MasterMenuId",
                        column: x => x.MasterMenuId,
                        principalTable: "MasterMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNo = table.Column<string>(type: "varchar(15)", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartyId = table.Column<int>(type: "int", nullable: false),
                    PaymentMode = table.Column<string>(type: "varchar(25)", nullable: true),
                    OType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OrderMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossAmount = table.Column<double>(type: "float", nullable: false),
                    TaxRate = table.Column<double>(type: "float", nullable: false),
                    TaxAmount = table.Column<double>(type: "float", nullable: false),
                    NetAmount = table.Column<double>(type: "float", nullable: false),
                    IspaymentClear = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyId = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Isprocessed = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PONo = table.Column<string>(type: "varchar(15)", nullable: true),
                    PODate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    PaymentMode = table.Column<string>(type: "varchar(25)", nullable: true),
                    GrossAmount = table.Column<double>(type: "float", nullable: false),
                    TaxRate = table.Column<double>(type: "float", nullable: false),
                    TaxAmount = table.Column<double>(type: "float", nullable: false),
                    NetAmount = table.Column<double>(type: "float", nullable: false),
                    IspaymentClear = table.Column<bool>(type: "bit", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_Party_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SRNo = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    SRDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(125)", nullable: true),
                    GrossAmount = table.Column<int>(type: "int", nullable: false),
                    DiscountAmount = table.Column<int>(type: "int", nullable: false),
                    TaxRate = table.Column<int>(type: "int", nullable: false),
                    TaxAmount = table.Column<int>(type: "int", nullable: false),
                    NetAmount = table.Column<int>(type: "int", nullable: false),
                    IspaymentClear = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleOrders_Party_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDetails_ProductDetails_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    StockType = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Warehouse = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Remaeks = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransactions_ProductDetails_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(15)", nullable: true),
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
                    table.PrimaryKey("PK_PurchaseOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_ProductDetails_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_PurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrder",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SaleOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    SaleOrderId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleOrderDetails_ProductDetails_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleOrderDetails_SaleOrders_SaleOrderId",
                        column: x => x.SaleOrderId,
                        principalTable: "SaleOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TType = table.Column<int>(type: "int", nullable: false),
                    TransectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    ReciverParty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecivingLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDirectDelivery = table.Column<bool>(type: "bit", nullable: false),
                    Remaeks = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    OrderDetailId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTransactions_OrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "CreateBy", "CreateOn", "FullName", "IsActive", "IsDeleted", "IsSuperUser", "Password", "UpdateBy", "UpdateOn", "UserName" },
                values: new object[] { 1, null, null, "Administrator", true, false, false, "admin", null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Order_PartyId",
                table: "Order",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ItemId",
                table: "OrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTransactions_OrderDetailId",
                table: "OrderTransactions",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_PartyId",
                table: "ProductDetails",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_VendorId",
                table: "PurchaseOrder",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_ItemId",
                table: "PurchaseOrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderId",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrderDetails_ItemId",
                table: "SaleOrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrderDetails_SaleOrderId",
                table: "SaleOrderDetails",
                column: "SaleOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrders_SupplierId",
                table: "SaleOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ItemId",
                table: "StockTransactions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizedScreens_AppUsersId",
                table: "UserAuthorizedScreens",
                column: "AppUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizedScreens_MasterMenuId",
                table: "UserAuthorizedScreens",
                column: "MasterMenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderTransactions");

            migrationBuilder.DropTable(
                name: "ProductionOrderDetails");

            migrationBuilder.DropTable(
                name: "ProductionOrders");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "SaleOrderDetails");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "UserAuthorizedScreens");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "PurchaseOrder");

            migrationBuilder.DropTable(
                name: "SaleOrders");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "MasterMenu");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "Party");
        }
    }
}
