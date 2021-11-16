using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd_GestaoFinanceira.Domains
{
    public partial class Valore
    {
        public bool? TipoEntrada { get; set; }
        public int IdValor { get; set; }
        public string Valor { get; set; }
        public DateTime? DataValor { get; set; }
        public string Foto { get; set; }
        public int? IdEmpresa { get; set; }
        public string Descricao { get; set; }
        public bool? Perda { get; set; }
        public int? IdSetor { get; set; }
        public string Titulo { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Setor IdSetorNavigation { get; set; }
    }
}
