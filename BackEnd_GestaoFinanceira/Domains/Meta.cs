using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd_GestaoFinanceira.Domains
{
    public partial class Meta
    {
        public int IdMeta { get; set; }
        public string Meta1 { get; set; }
        public DateTime? Mes { get; set; }
        public int? IdSetor { get; set; }

        public virtual Setor IdSetorNavigation { get; set; }
    }
}
