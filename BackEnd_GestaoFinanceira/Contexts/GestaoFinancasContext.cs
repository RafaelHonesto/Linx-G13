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
                // optionsBuilder.UseSqlServer("Data Source=DESKTOP-KVKV9TT\\SA; initial catalog=GESTAOFINANCAS; user Id=sa; pwd=senai@132");

                optionsBuilder.UseSqlServer("Data Source=db-linx.cmwveh4yh3n9.us-east-1.rds.amazonaws.com; initial catalog=GESTAOFINANCAS; user id=admin_linx; pwd=Knives132");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Despesa>(entity =>
            {
                entity.HasKey(e => e.IdDespesa)
                    .HasName("PK__Despesa__D60EA6D9B96D022D");

                entity.ToTable("Despesa");

                entity.Property(e => e.DataDespesa).HasColumnType("date");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdSetorNavigation)
                    .WithMany(p => p.Despesas)
                    .HasForeignKey(d => d.IdSetor)
                    .HasConstraintName("FK__Despesa__IdSetor__46E78A0C");

                entity.HasOne(d => d.IdTipoDespesaNavigation)
                    .WithMany(p => p.Despesas)
                    .HasForeignKey(d => d.IdTipoDespesa)
                    .HasConstraintName("FK__Despesa__IdTipoD__45F365D3");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.IdEmpresa)
                    .HasName("PK__Empresa__5EF4033ECFB09E42");

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
                    .HasConstraintName("FK__Empresa__IdSetor__49C3F6B7");
            });

            modelBuilder.Entity<Funcionario>(entity =>
            {
                entity.HasKey(e => e.IdFuncionario)
                    .HasName("PK__Funciona__35CB052ABFA3C9C6");

                entity.ToTable("Funcionario");

                entity.HasIndex(e => e.IdUsuario, "UQ__Funciona__5B65BF96C6ABE3B5")
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
                    .HasName("PK__Setor__113E4B9E2C9F624B");

                entity.ToTable("Setor");

                entity.Property(e => e.Nome)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoDespesa>(entity =>
            {
                entity.HasKey(e => e.IdTipoDespesa)
                    .HasName("PK__TipoDesp__080827EEB81B7F93");

                entity.ToTable("TipoDespesa");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdSetorNavigation)
                    .WithMany(p => p.TipoDespesas)
                    .HasForeignKey(d => d.IdSetor)
                    .HasConstraintName("FK__TipoDespe__IdSet__4316F928");
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasKey(e => e.IdTipoUsuario)
                    .HasName("PK__TipoUsua__CA04062B4EE389DC");

                entity.ToTable("TipoUsuario");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuario__5B65BF970F6721A8");

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
                    .HasName("PK__Valores__D74976D32103DD51");

                entity.Property(e => e.DataValor).HasColumnType("date");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Foto)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('default.png')");

                entity.Property(e => e.Valor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.Valores)
                    .HasForeignKey(d => d.IdEmpresa)
                    .HasConstraintName("FK__Valores__IdEmpre__534D60F1");

                entity.HasOne(d => d.IdSetorNavigation)
                    .WithMany(p => p.Valores)
                    .HasForeignKey(d => d.IdSetor)
                    .HasConstraintName("FK__Valores__IdSetor__5165187F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
