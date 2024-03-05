using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class LearningHubContext : DbContext
    {
        public LearningHubContext()
        {
        }

        public LearningHubContext(DbContextOptions<LearningHubContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BaiGiang> BaiGiangs { get; set; }
        public virtual DbSet<BaoCao> BaoCaos { get; set; }
        public virtual DbSet<CauHoi> CauHois { get; set; }
        public virtual DbSet<Chuong> Chuongs { get; set; }
        public virtual DbSet<DangKyHoc> DangKyHocs { get; set; }
        public virtual DbSet<DanhMucCon> DanhMucCons { get; set; }
        public virtual DbSet<DanhMucKhoaHoc> DanhMucKhoaHocs { get; set; }
        public virtual DbSet<HocVien> HocViens { get; set; }
        public virtual DbSet<HoiDap> HoiDaps { get; set; }
        public virtual DbSet<KhoaHoc> KhoaHocs { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }
        public virtual DbSet<KhuyenMaiCuaKhoaHoc> KhuyenMaiCuaKhoaHocs { get; set; }
        public virtual DbSet<LinhVucGiangDay> LinhVucGiangDays { get; set; }
        public virtual DbSet<NguoiDay> NguoiDays { get; set; }
        public virtual DbSet<NhanXet> NhanXets { get; set; }
        public virtual DbSet<ThanhToan> ThanhToans { get; set; }
        public virtual DbSet<TienTrinhHoc> TienTrinhHocs { get; set; }
        public virtual DbSet<TraLoiHoiDap> TraLoiHoiDaps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=LearningHub;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BaiGiang>(entity =>
            {
                entity.HasKey(e => e.MaBaiGiang)
                    .HasName("PK_DataLessons");

                entity.ToTable("BaiGiang");

                entity.Property(e => e.TieuDeBaiGiang).HasMaxLength(200);

                entity.Property(e => e.TinhTrang).HasMaxLength(100);

                entity.HasOne(d => d.MaChuongNavigation)
                    .WithMany(p => p.BaiGiangs)
                    .HasForeignKey(d => d.MaChuong)
                    .HasConstraintName("FK_BaiGiang_Chuong");
            });

            modelBuilder.Entity<BaoCao>(entity =>
            {
                entity.HasKey(e => e.MaBaoCao);

                entity.ToTable("BaoCao");

                entity.Property(e => e.ChiTietBaoCao).HasMaxLength(200);

                entity.Property(e => e.HinhAnh).HasMaxLength(500);

                entity.Property(e => e.MaKhoaHoc).HasMaxLength(12);

                entity.Property(e => e.MoTa).HasMaxLength(500);

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.PhanHoi).HasMaxLength(500);

                entity.Property(e => e.TieuDe).HasMaxLength(200);

                entity.Property(e => e.TinhTrang).HasMaxLength(50);

                entity.HasOne(d => d.MaHocVienNavigation)
                    .WithMany(p => p.BaoCaos)
                    .HasForeignKey(d => d.MaHocVien)
                    .HasConstraintName("FK_BaoCao_HocVien");

                entity.HasOne(d => d.MaKhoaHocNavigation)
                    .WithMany(p => p.BaoCaos)
                    .HasForeignKey(d => d.MaKhoaHoc)
                    .HasConstraintName("FK_BaoCao_KhoaHoc");
            });

            modelBuilder.Entity<CauHoi>(entity =>
            {
                entity.HasKey(e => e.MaCauHoi)
                    .HasName("PK_question");

                entity.ToTable("CauHoi");

                entity.Property(e => e.CauHoi1)
                    .HasMaxLength(200)
                    .HasColumnName("CauHoi");

                entity.Property(e => e.DapAn1).HasMaxLength(200);

                entity.Property(e => e.DapAn2).HasMaxLength(200);

                entity.Property(e => e.DapAn3).HasMaxLength(200);

                entity.Property(e => e.DapAn4).HasMaxLength(200);

                entity.Property(e => e.DapAnDung).HasMaxLength(200);

                entity.Property(e => e.MaKhoaHoc)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.HasOne(d => d.MaKhoaHocNavigation)
                    .WithMany(p => p.CauHois)
                    .HasForeignKey(d => d.MaKhoaHoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CauHoi_KhoaHoc1");
            });

            modelBuilder.Entity<Chuong>(entity =>
            {
                entity.HasKey(e => e.MaChuong);

                entity.ToTable("Chuong");

                entity.Property(e => e.MaKhoaHoc).HasMaxLength(12);

                entity.Property(e => e.TieuDeChuong).HasMaxLength(200);

                entity.Property(e => e.TinhTrang).HasMaxLength(100);

                entity.HasOne(d => d.MaKhoaHocNavigation)
                    .WithMany(p => p.Chuongs)
                    .HasForeignKey(d => d.MaKhoaHoc)
                    .HasConstraintName("FK_Chuong_KhoaHoc1");
            });

            modelBuilder.Entity<DangKyHoc>(entity =>
            {
                entity.HasKey(e => e.MaDangKy)
                    .HasName("PK_RegisterTheCourse");

                entity.ToTable("DangKyHoc");

                entity.Property(e => e.MaDangKy).ValueGeneratedNever();

                entity.Property(e => e.HocPhi).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.MaKhoaHoc)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.Property(e => e.NgayHuy).HasColumnType("datetime");

                entity.Property(e => e.NgayThanhToan).HasColumnType("datetime");

                entity.Property(e => e.TieuDeKhoaHoc).HasMaxLength(500);

                entity.Property(e => e.TinhTrang).HasMaxLength(50);

                entity.HasOne(d => d.MaHocVienNavigation)
                    .WithMany(p => p.DangKyHocs)
                    .HasForeignKey(d => d.MaHocVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DangKyHoc_HocVien");

                entity.HasOne(d => d.MaKhoaHocNavigation)
                    .WithMany(p => p.DangKyHocs)
                    .HasForeignKey(d => d.MaKhoaHoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DangKyHoc_KhoaHoc");
            });

            modelBuilder.Entity<DanhMucCon>(entity =>
            {
                entity.HasKey(e => e.MaDanhMucCon);

                entity.ToTable("DanhMucCon");

                entity.Property(e => e.TieuDeDanhMuc).HasMaxLength(250);

                entity.HasOne(d => d.MaDanhMucNavigation)
                    .WithMany(p => p.DanhMucCons)
                    .HasForeignKey(d => d.MaDanhMuc)
                    .HasConstraintName("FK_DanhMucCon_DanhMucKhoaHoc");
            });

            modelBuilder.Entity<DanhMucKhoaHoc>(entity =>
            {
                entity.HasKey(e => e.MaDanhMuc);

                entity.ToTable("DanhMucKhoaHoc");

                entity.Property(e => e.TieuDeDanhMuc).HasMaxLength(250);
            });

            modelBuilder.Entity<HocVien>(entity =>
            {
                entity.HasKey(e => e.MaHocVien);

                entity.ToTable("HocVien");

                entity.Property(e => e.MaHocVien).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.MatKhau).HasMaxLength(250);

                entity.Property(e => e.NgaySinh).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Role).HasMaxLength(10);

                entity.Property(e => e.TenDangNhap).HasMaxLength(50);

                entity.Property(e => e.TenHocVien)
                    .HasMaxLength(20)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<HoiDap>(entity =>
            {
                entity.HasKey(e => e.MaHoiDap);

                entity.ToTable("HoiDap");

                entity.Property(e => e.MaKhoaHoc).HasMaxLength(12);

                entity.Property(e => e.MoTa).HasMaxLength(500);

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.TenNguoiGui).HasMaxLength(200);

                entity.Property(e => e.TieuDe).HasMaxLength(200);

                entity.HasOne(d => d.MaHocVienNavigation)
                    .WithMany(p => p.HoiDaps)
                    .HasForeignKey(d => d.MaHocVien)
                    .HasConstraintName("FK_HoiDap_HocVien");

                entity.HasOne(d => d.MaKhoaHocNavigation)
                    .WithMany(p => p.HoiDaps)
                    .HasForeignKey(d => d.MaKhoaHoc)
                    .HasConstraintName("FK_HoiDap_KhoaHoc");
            });

            modelBuilder.Entity<KhoaHoc>(entity =>
            {
                entity.HasKey(e => e.MaKhoaHoc)
                    .HasName("PK_Course");

                entity.ToTable("KhoaHoc");

                entity.Property(e => e.MaKhoaHoc).HasMaxLength(12);

                entity.Property(e => e.NgayDuyet).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.TinhTrang).HasMaxLength(50);

                entity.HasOne(d => d.MaNguoiDayNavigation)
                    .WithMany(p => p.KhoaHocs)
                    .HasForeignKey(d => d.MaNguoiDay)
                    .HasConstraintName("FK_KhoaHoc_NguoiDay");
            });

            modelBuilder.Entity<KhuyenMai>(entity =>
            {
                entity.HasKey(e => e.MaGiamGia);

                entity.ToTable("KhuyenMai");

                entity.Property(e => e.GhiChu)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.HasOne(d => d.MaNguoiDayNavigation)
                    .WithMany(p => p.KhuyenMais)
                    .HasForeignKey(d => d.MaNguoiDay)
                    .HasConstraintName("FK_KhuyenMai_NguoiDay");
            });

            modelBuilder.Entity<KhuyenMaiCuaKhoaHoc>(entity =>
            {
                entity.HasKey(e => e.MaApDung);

                entity.ToTable("KhuyenMaiCuaKhoaHoc");

                entity.Property(e => e.MaKhoaHoc).HasMaxLength(12);

                entity.Property(e => e.NgayBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");

                entity.HasOne(d => d.MaKhoaHocNavigation)
                    .WithMany(p => p.KhuyenMaiCuaKhoaHocs)
                    .HasForeignKey(d => d.MaKhoaHoc)
                    .HasConstraintName("FK_KhuyenMaiCuaKhoaHoc_KhoaHoc");
            });

            modelBuilder.Entity<LinhVucGiangDay>(entity =>
            {
                entity.HasKey(e => e.MaLinhVuc);

                entity.ToTable("LinhVucGiangDay");

                entity.HasOne(d => d.MaDanhmucNavigation)
                    .WithMany(p => p.LinhVucGiangDays)
                    .HasForeignKey(d => d.MaDanhmuc)
                    .HasConstraintName("FK_LinhVucGiangDay_DanhMucKhoaHoc");

                entity.HasOne(d => d.MaNguoiDayNavigation)
                    .WithMany(p => p.LinhVucGiangDays)
                    .HasForeignKey(d => d.MaNguoiDay)
                    .HasConstraintName("FK_LinhVucGiangDay_NguoiDay");
            });

            modelBuilder.Entity<NguoiDay>(entity =>
            {
                entity.HasKey(e => e.MaNguoiDay)
                    .HasName("PK_DataTeachers");

                entity.ToTable("NguoiDay");

                entity.Property(e => e.MaNguoiDay).ValueGeneratedNever();

                entity.Property(e => e.Cccd)
                    .HasMaxLength(12)
                    .HasColumnName("CCCD");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.MatKhau).HasMaxLength(250);

                entity.Property(e => e.NgaySinh).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Role).HasMaxLength(10);

                entity.Property(e => e.TenDangNhap).HasMaxLength(50);

                entity.Property(e => e.TenNguoiDay).HasMaxLength(50);

                entity.Property(e => e.TinhTrang).HasMaxLength(50);
            });

            modelBuilder.Entity<NhanXet>(entity =>
            {
                entity.HasKey(e => e.MaNhanXet);

                entity.ToTable("NhanXet");

                entity.Property(e => e.MaKhoaHoc).HasMaxLength(12);

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.TenNguoiGui).HasMaxLength(200);

                entity.HasOne(d => d.MaKhoaHocNavigation)
                    .WithMany(p => p.NhanXets)
                    .HasForeignKey(d => d.MaKhoaHoc)
                    .HasConstraintName("FK_NhanXet_KhoaHoc");
            });

            modelBuilder.Entity<ThanhToan>(entity =>
            {
                entity.HasKey(e => e.MaThanhToan);

                entity.ToTable("ThanhToan");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.NgayThanhToan).HasColumnType("datetime");

                entity.Property(e => e.SoTienNhanDuoc).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SotTien).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TinhTrang).HasMaxLength(50);

                entity.HasOne(d => d.MaDangKyNavigation)
                    .WithMany(p => p.ThanhToans)
                    .HasForeignKey(d => d.MaDangKy)
                    .HasConstraintName("FK_ThanhToan_DangKyHoc");

                entity.HasOne(d => d.MaNguoiDayNavigation)
                    .WithMany(p => p.ThanhToans)
                    .HasForeignKey(d => d.MaNguoiDay)
                    .HasConstraintName("FK_ThanhToan_NguoiDay");
            });

            modelBuilder.Entity<TienTrinhHoc>(entity =>
            {
                entity.HasKey(e => e.MaTienTrinh);

                entity.ToTable("TienTrinhHoc");

                entity.Property(e => e.MaKhoaHoc).HasMaxLength(12);

                entity.HasOne(d => d.MaHocVienNavigation)
                    .WithMany(p => p.TienTrinhHocs)
                    .HasForeignKey(d => d.MaHocVien)
                    .HasConstraintName("FK_TienTrinhHoc_HocVien");

                entity.HasOne(d => d.MaKhoaHocNavigation)
                    .WithMany(p => p.TienTrinhHocs)
                    .HasForeignKey(d => d.MaKhoaHoc)
                    .HasConstraintName("FK_TienTrinhHoc_KhoaHoc");
            });

            modelBuilder.Entity<TraLoiHoiDap>(entity =>
            {
                entity.HasKey(e => e.MaTraLoi);

                entity.ToTable("TraLoiHoiDap");

                entity.Property(e => e.MoTa).HasMaxLength(500);

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.TenNguoiTraLoi).HasMaxLength(200);

                entity.HasOne(d => d.MaHoiDapNavigation)
                    .WithMany(p => p.TraLoiHoiDaps)
                    .HasForeignKey(d => d.MaHoiDap)
                    .HasConstraintName("FK_TraLoiHoiDap_HoiDap");

                entity.HasOne(d => d.MaNguoiDayNavigation)
                    .WithMany(p => p.TraLoiHoiDaps)
                    .HasForeignKey(d => d.MaNguoiDay)
                    .HasConstraintName("FK_TraLoiHoiDap_NguoiDay");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
