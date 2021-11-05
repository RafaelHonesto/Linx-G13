using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd_GestaoFinanceira.Domains
{
    public partial class Despesa
    {
        public int? IdTipoDespesa { get; set; }
        public int IdDespesa { get; set; }
        public int? IdSetor { get; set; }
        public DateTime? DataDespesa { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public virtual Setor IdSetorNavigation { get; set; }
        public virtual TipoDespesa IdTipoDespesaNavigation { get; set; }
    }
}
