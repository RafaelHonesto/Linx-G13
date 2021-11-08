using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd_GestaoFinanceira.Domains
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string Acesso { get; set; }
        public string SenhaDeAcesso { get; set; }
        public string Foto { get; set; }
        public int? IdTipoUsuario { get; set; }

        public virtual TipoUsuario IdTipoUsuarioNavigation { get; set; }
        public virtual Funcionario Funcionario { get; set; }
    }
}
