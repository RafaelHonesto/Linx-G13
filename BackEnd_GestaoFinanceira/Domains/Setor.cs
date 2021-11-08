using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd_GestaoFinanceira.Domains
{
    public partial class Setor
    {
        public Setor()
        {
            Despesas = new HashSet<Despesa>();
            Empresas = new HashSet<Empresa>();
            Funcionarios = new HashSet<Funcionario>();
        }

        public int IdSetor { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Despesa> Despesas { get; set; }
        public virtual ICollection<Empresa> Empresas { get; set; }
        public virtual ICollection<Funcionario> Funcionarios { get; set; }
    }
}
