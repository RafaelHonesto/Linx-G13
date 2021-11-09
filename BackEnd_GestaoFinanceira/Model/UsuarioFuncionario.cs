using BackEnd_GestaoFinanceira.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Model
{
    public class UsuarioFuncionario
    {
        public Usuario usuario { get; set; }
        public Funcionario funcionario { get; set; }
    }
}
