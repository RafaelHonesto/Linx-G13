using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd_GestaoFinanceira.Domains
{
    public partial class Empresa
    {
        public Empresa()
        {
            Valores = new HashSet<Valore>();
        }

        public int IdEmpresa { get; set; }
        public int? IdSetor { get; set; }
        public string Cnpj { get; set; }
        public string NomeEmpresa { get; set; }

        public virtual Setor IdSetorNavigation { get; set; }
        public virtual ICollection<Valore> Valores { get; set; }
    }
}
