using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd_GestaoFinanceira.Domains
{
    public partial class Funcionario
    {
        public int IdFuncionario { get; set; }
        public int? IdSetor { get; set; }
        public int? IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Foto { get; set; }
        public string Funcao { get; set; }

        public virtual Setor IdSetorNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
