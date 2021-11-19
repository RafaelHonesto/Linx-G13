using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BackEnd_GestaoFinanceira.Domains;

#nullable disable

namespace BackEnd_GestaoFinanceira.Contexts
{
    public partial class GestaoFinancasContext : DbContext
    {
        public GestaoFinancasContext()
        {
        }

        public GestaoFinancasContext(DbContextOptions<GestaoFinancasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Despesa> Despesas { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Funcionario> Funcionarios { get; set; }
        public virtual DbSet<Setor> Setors { get; set; }
        public virtual DbSet<TipoDespesa> TipoDespesas { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Valore> Valores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Data Source=DESKTOP-UIVSK00\\SQLEXPRESS; initial catalog=GESTAOFINANCAS; integrated security = true");

                optionsBuilder.UseSqlServer("Data Source=db-linx.cmwveh4yh3n9.us-east-1.RDS.amazonaws.com; initial catalog=GESTAOFINANCAS; user Id=admin_linx; pwd=Knives132");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Despesa>(entity =>
            {
                entity.HasKey(e => e.IdDespesa)
                    .HasName("PK__Despesa__D60EA6D9681CD4A2");

                entity.ToTable("Despesa");

                entity.Property(e => e.DataDespesa).HasColumnType("date");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("numeric(10, 2)");

                entity.HasOne(d => d.IdSetorNavigation)
                    .WithMany(p => p.Despesas)
                    .HasForeignKey(d => d.IdSetor)
                    .HasConstraintName("FK__Despesa__IdSetor__45F365D3");

                entity.HasOne(d => d.IdTipoDespesaNavigation)
                    .WithMany(p => p.Despesas)
                    .HasForeignKey(d => d.IdTipoDespesa)
                    .HasConstraintName("FK__Despesa__IdTipoD__44FF419A");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.IdEmpresa)
                    .HasName("PK__Empresa__5EF4033EDFAAAA1A");

                entity.ToTable("Empresa");

                entity.Property(e => e.Cnpj)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("CNPJ");

                entity.Property(e => e.NomeEmpresa)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdSetorNavigation)
                    .WithMany(p => p.Empresas)
                    .HasForeignKey(d => d.IdSetor)
                    .HasConstraintName("FK__Empresa__IdSetor__48CFD27E");
            });

            modelBuilder.Entity<Funcionario>(entity =>
            {
                entity.HasKey(e => e.IdFuncionario)
                    .HasName("PK__Funciona__35CB052A0657DE01");

                entity.ToTable("Funcionario");

                entity.HasIndex(e => e.IdUsuario, "UQ__Funciona__5B65BF960605E12E")
                    .IsUnique();

                entity.Property(e => e.Cpf)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("CPF");

                entity.Property(e => e.Foto)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('user.png')");

                entity.Property(e => e.Funcao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdSetorNavigation)
                    .WithMany(p => p.Funcionarios)
                    .HasForeignKey(d => d.IdSetor)
                    .HasConstraintName("FK__Funcionar__IdSet__3E52440B");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithOne(p => p.Funcionario)
                    .HasForeignKey<Funcionario>(d => d.IdUsuario)
                    .HasConstraintName("FK__Funcionar__IdUsu__3F466844");
            });

            modelBuilder.Entity<Setor>(entity =>
            {
                entity.HasKey(e => e.IdSetor)
                    .HasName("PK__Setor__113E4B9E339706A2");

                entity.ToTable("Setor");

                entity.Property(e => e.Nome)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoDespesa>(entity =>
            {
                entity.HasKey(e => e.IdTipoDespesa)
                    .HasName("PK__TipoDesp__080827EE6423B0CE");

                entity.ToTable("TipoDespesa");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdSetorNavigation)
                    .WithMany(p => p.TipoDespesas)
                    .HasForeignKey(d => d.IdSetor)
                    .HasConstraintName("FK__TipoDespe__IdSet__5AEE82B9");
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasKey(e => e.IdTipoUsuario)
                    .HasName("PK__TipoUsua__CA04062BDF97AB80");

                entity.ToTable("TipoUsuario");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuario__5B65BF978E15E6B2");

                entity.ToTable("Usuario");

                entity.Property(e => e.Acesso)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SenhaDeAcesso)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTipoUsuarioNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdTipoUsuario)
                    .HasConstraintName("FK__Usuario__IdTipoU__38996AB5");
            });

            modelBuilder.Entity<Valore>(entity =>
            {
                entity.HasKey(e => e.IdValor)
                    .HasName("PK__Valores__D74976D3EA1B5B98");

                entity.Property(e => e.DataValor).HasColumnType("date");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Foto)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('default.png')");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.Valores)
                    .HasForeignKey(d => d.IdEmpresa)
                    .HasConstraintName("FK__Valores__IdEmpre__4CA06362");

                entity.HasOne(d => d.IdSetorNavigation)
                    .WithMany(p => p.Valores)
                    .HasForeignKey(d => d.IdSetor)
                    .HasConstraintName("FK__Valores__IdSetor__5BE2A6F2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
