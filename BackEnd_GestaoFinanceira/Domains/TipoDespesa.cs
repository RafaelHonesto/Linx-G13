using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd_GestaoFinanceira.Domains
{
    public partial class TipoDespesa
    {
        public TipoDespesa()
        {
            Despesas = new HashSet<Despesa>();
        }

        public int IdTipoDespesa { get; set; }
        public string Titulo { get; set; }
        public int? IdSetor { get; set; }

        public virtual Setor IdSetorNavigation { get; set; }
        public virtual ICollection<Despesa> Despesas { get; set; }
    }
}
