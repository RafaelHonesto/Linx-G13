using BackEnd_GestaoFinanceira.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Interfaces
{
    public interface IFuncionarioRepository
    {
        /// <summary>
        /// Cria funcionario
        /// </summary>
        /// <param name="funcionario">funcionario a ser criado</param>
        public void Create(Funcionario funcionario);

        /// <summary>
        /// Lista funcionarios
        /// </summary>
        /// <returns>lista de funcionarios</returns>
        public List<Funcionario> Read();

        /// <summary>
        /// Atualiza funcionario
        /// </summary>
        /// <param name="funcionario">funcionario a ser atualizado</param>
        public void Update(Funcionario funcionario);

        /// <summary>
        /// Deleta funcionario
        /// </summary>
        /// <param name="id">id do funcionario a ser deletado</param>
        public void Delete(int id);

        /// <summary>
        /// Busca funcionario pelo id do funcionario
        /// </summary>
        /// <param name="idUsuario">id do usuario do funcionario a ser buscado</param>
        /// <returns>funcionario</returns>
        public Funcionario FindByUserId(int idUsuario);
    }
}
