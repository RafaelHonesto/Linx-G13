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
            Meta = new HashSet<Meta>();
            TipoDespesas = new HashSet<TipoDespesa>();
            Valores = new HashSet<Valore>();
        }

        public int IdSetor { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Despesa> Despesas { get; set; }
        public virtual ICollection<Empresa> Empresas { get; set; }
        public virtual ICollection<Funcionario> Funcionarios { get; set; }
        public virtual ICollection<Meta> Meta { get; set; }
        public virtual ICollection<TipoDespesa> TipoDespesas { get; set; }
        public virtual ICollection<Valore> Valores { get; set; }
    }
}
