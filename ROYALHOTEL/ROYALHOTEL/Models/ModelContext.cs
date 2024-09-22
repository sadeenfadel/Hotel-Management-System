using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ROYALHOTEL.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutu> Aboutus { get; set; }

    public virtual DbSet<Blogging> Bloggings { get; set; }

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<Footer> Footers { get; set; }

    public virtual DbSet<Gallery> Galleries { get; set; }

    public virtual DbSet<Homepage> Homepages { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Reporting> Reportings { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Testimonials1> Testimonials1s { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##Hotel;PASSWORD=Test123;DATA SOURCE=localhost:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##HOTEL")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutu>(entity =>
        {
            entity.HasKey(e => e.Aboutusid).HasName("SYS_C008580");

            entity.ToTable("ABOUTUS");

            entity.Property(e => e.Aboutusid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ABOUTUSID");
            entity.Property(e => e.Content)
                .HasColumnType("CLOB")
                .HasColumnName("CONTENT");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
        });

        modelBuilder.Entity<Blogging>(entity =>
        {
            entity.HasKey(e => e.Blogid).HasName("SYS_C008590");

            entity.ToTable("BLOGGING");

            entity.Property(e => e.Blogid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("BLOGID");
            entity.Property(e => e.Content)
                .HasColumnType("CLOB")
                .HasColumnName("CONTENT");
            entity.Property(e => e.Datecreated)
                .HasDefaultValueSql("SYSDATE")
                .HasColumnType("DATE")
                .HasColumnName("DATECREATED");
            entity.Property(e => e.Heading)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HEADING");
            entity.Property(e => e.Imageurl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEURL");
        });

        modelBuilder.Entity<Contactu>(entity =>
        {
            entity.HasKey(e => e.Contactusid).HasName("SYS_C008582");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.Contactusid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACTUSID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Mapurl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("MAPURL");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Featureid).HasName("SYS_C008584");

            entity.ToTable("FEATURES");

            entity.Property(e => e.Featureid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("FEATUREID");
            entity.Property(e => e.Featuretext)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("FEATURETEXT");
        });

        modelBuilder.Entity<Footer>(entity =>
        {
            entity.HasKey(e => e.Footerid).HasName("SYS_C008608");

            entity.ToTable("FOOTER");

            entity.Property(e => e.Footerid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("FOOTERID");
            entity.Property(e => e.About)
                .HasColumnType("CLOB")
                .HasColumnName("ABOUT");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Links)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LINKS");
            entity.Property(e => e.Newsletter)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NEWSLETTER");
        });

        modelBuilder.Entity<Gallery>(entity =>
        {
            entity.HasKey(e => e.Galleryid).HasName("SYS_C008604");

            entity.ToTable("GALLERY");

            entity.Property(e => e.Galleryid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("GALLERYID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
        });

        modelBuilder.Entity<Homepage>(entity =>
        {
            entity.HasKey(e => e.Homepageid).HasName("SYS_C008578");

            entity.ToTable("HOMEPAGE");

            entity.Property(e => e.Homepageid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("HOMEPAGEID");
            entity.Property(e => e.Greeting)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("GREETING");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Paragraph)
                .HasColumnType("CLOB")
                .HasColumnName("PARAGRAPH");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Hotelid).HasName("SYS_C008554");

            entity.ToTable("HOTELS");

            entity.Property(e => e.Hotelid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Description)
                .HasColumnType("CLOB")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Hotelname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HOTELNAME");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("LOCATION");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Invoiceid).HasName("SYS_C008574");

            entity.ToTable("INVOICE");

            entity.Property(e => e.Invoiceid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("INVOICEID");
            entity.Property(e => e.Invoicecontent)
                .HasColumnType("CLOB")
                .HasColumnName("INVOICECONTENT");
            entity.Property(e => e.Invoicedate)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("INVOICEDATE");
            entity.Property(e => e.Reservationid)
                .HasColumnType("NUMBER")
                .HasColumnName("RESERVATIONID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Reservationid)
                .HasConstraintName("SYS_C008575");

            entity.HasOne(d => d.User).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008576");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Loginid).HasName("SYS_C008547");

            entity.ToTable("LOGIN");

            entity.Property(e => e.Loginid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("LOGINID");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Logins)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("SYS_C008549");

            entity.HasOne(d => d.User).WithMany(p => p.Logins)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008548");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("SYS_C008571");

            entity.ToTable("PAYMENT");

            entity.Property(e => e.Paymentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Cardholdername)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CARDHOLDERNAME");
            entity.Property(e => e.Creditcardnumber)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CREDITCARDNUMBER");
            entity.Property(e => e.Expirydate)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("EXPIRYDATE");
            entity.Property(e => e.Paymentdate)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("PAYMENTDATE");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAYMENTMETHOD");
            entity.Property(e => e.Reservationid)
                .HasColumnType("NUMBER")
                .HasColumnName("RESERVATIONID");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Reservationid)
                .HasConstraintName("SYS_C008572");
        });

        modelBuilder.Entity<Reporting>(entity =>
        {
            entity.HasKey(e => e.Reportid).HasName("SYS_C008602");

            entity.ToTable("REPORTING");

            entity.Property(e => e.Reportid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("REPORTID");
            entity.Property(e => e.Netprofit)
                .HasComputedColumnSql("\"TOTALROOMSBOOKED\"*\"PRICEPERNIGHT\"-\"TOTALEXPENSES\"", false)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("NETPROFIT");
            entity.Property(e => e.Pricepernight)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("PRICEPERNIGHT");
            entity.Property(e => e.Profitorloss)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComputedColumnSql("CASE  WHEN \"TOTALROOMSBOOKED\"*\"PRICEPERNIGHT\"-\"TOTALEXPENSES\">=0 THEN 'Profit' ELSE 'Loss' END ", false)
                .HasColumnName("PROFITORLOSS");
            entity.Property(e => e.Revenue)
                .HasComputedColumnSql("\"TOTALROOMSBOOKED\"*\"PRICEPERNIGHT\"", false)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("REVENUE");
            entity.Property(e => e.Totalexpenses)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("TOTALEXPENSES");
            entity.Property(e => e.Totalroomsbooked)
                .HasColumnType("NUMBER")
                .HasColumnName("TOTALROOMSBOOKED");
            entity.Property(e => e.Year)
                .HasPrecision(4)
                .HasColumnName("YEAR");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Reservationid).HasName("SYS_C008562");

            entity.ToTable("RESERVATIONS");

            entity.Property(e => e.Reservationid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("RESERVATIONID");
            entity.Property(e => e.Checkindate)
                .HasColumnType("DATE")
                .HasColumnName("CHECKINDATE");
            entity.Property(e => e.Checkoutdate)
                .HasColumnType("DATE")
                .HasColumnName("CHECKOUTDATE");
            entity.Property(e => e.Reservationdate)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("RESERVATIONDATE");
            entity.Property(e => e.Roomid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROOMID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Room).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Roomid)
                .HasConstraintName("SYS_C008564");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008563");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008530");

            entity.ToTable("ROLES");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Roomid).HasName("SYS_C008559");

            entity.ToTable("ROOMS");

            entity.Property(e => e.Roomid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROOMID");
            entity.Property(e => e.Hotelid)
                .HasColumnType("NUMBER")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Isavailable)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'Y'")
                .IsFixedLength()
                .HasColumnName("ISAVAILABLE");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.Roomtype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROOMTYPE");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.Hotelid)
                .HasConstraintName("SYS_C008560");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C008588");

            entity.ToTable("TESTIMONIALS");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Imageurl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEURL");
            entity.Property(e => e.Testimonialtext)
                .HasColumnType("CLOB")
                .HasColumnName("TESTIMONIALTEXT");
        });

        modelBuilder.Entity<Testimonials1>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C008595");

            entity.ToTable("TESTIMONIALS_1");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Datecreated)
                .HasDefaultValueSql("SYSDATE")
                .HasColumnType("DATE")
                .HasColumnName("DATECREATED");
            entity.Property(e => e.Hotelid)
                .HasColumnType("NUMBER")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Rating)
                .HasPrecision(1)
                .HasColumnName("RATING");
            entity.Property(e => e.Testimonialtext)
                .HasColumnType("CLOB")
                .HasColumnName("TESTIMONIALTEXT");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Testimonials1s)
                .HasForeignKey(d => d.Hotelid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TESTIMONIAL_HOTEL");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials1s)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TESTIMONIAL_USER");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008543");

            entity.ToTable("USERS");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Userfname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERFNAME");
            entity.Property(e => e.Userlname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERLNAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
