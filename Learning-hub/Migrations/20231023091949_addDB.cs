using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning_hub.Migrations
{
    public partial class addDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    IdKhoaHoc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDay = table.Column<int>(type: "int", nullable: false),
                    TieuDeKhoaHoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KienThucThuDuoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrinhDo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DanhMuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DanhMucCon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gia = table.Column<int>(type: "int", nullable: false),
                    SoLuongHocVien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.IdKhoaHoc);
                });

            migrationBuilder.CreateTable(
                name: "DataDocuments",
                columns: table => new
                {
                    IdTaiLieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBaiGiang = table.Column<int>(type: "int", nullable: false),
                    TenTaiLieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiTaiLieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuongDan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataDocuments", x => x.IdTaiLieu);
                });

            migrationBuilder.CreateTable(
                name: "DataEducationPrograms",
                columns: table => new
                {
                    IdChuongTrinh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNguoiDay = table.Column<int>(type: "int", nullable: false),
                    TieuDeMuc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataEducationPrograms", x => x.IdChuongTrinh);
                });

            migrationBuilder.CreateTable(
                name: "DataLessons",
                columns: table => new
                {
                    IdBaiGiang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdKhoaHoc = table.Column<int>(type: "int", nullable: false),
                    TieuDeBaiGiang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataLessons", x => x.IdBaiGiang);
                });

            migrationBuilder.CreateTable(
                name: "RequestApproval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Describe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TeachingFields = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestApproval", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PassWord = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CCCD = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    Role = table.Column<int>(type: "int", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "DataTeachers",
                columns: table => new
                {
                    IdNguoiDay = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNguoiDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GioiThieuBanThan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinhVucDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhTrang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTeachers", x => x.IdNguoiDay);
                    table.ForeignKey(
                        name: "FK_DataTeachers_User_IdNguoiDay",
                        column: x => x.IdNguoiDay,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserModelIdUser = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserModelIdUser",
                        column: x => x.UserModelIdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserModelIdUser",
                table: "RefreshToken",
                column: "UserModelIdUser");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdUser",
                table: "User",
                column: "IdUser",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "DataDocuments");

            migrationBuilder.DropTable(
                name: "DataEducationPrograms");

            migrationBuilder.DropTable(
                name: "DataLessons");

            migrationBuilder.DropTable(
                name: "DataTeachers");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "RequestApproval");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
