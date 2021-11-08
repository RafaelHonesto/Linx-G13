using BackEnd_GestaoFinanceira.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Interfaces
{
    public interface ITipoUsuarioRepository
    {
        /// <summary>
        /// Cria tipo de usuario
        /// </summary>
        /// <param name="tipoUsuario">tipo de usuario a ser crciado</param>
        public void Create(TipoUsuario tipoUsuario);

        /// <summary>
        /// Lista tipos de usuario
        /// </summary>
        /// <returns>Lista de tipos de usuario</returns>
        public List<TipoUsuario> Read();

        /// <summary>
        /// Atualiza tipo de usuario
        /// </summary>
        /// <param name="tipoUsuario">tipo de usuario a ser atualizado</param>
        public void Update(TipoUsuario tipoUsuario);

        /// <summary>
        /// Deleta tipo de usuario
        /// </summary>
        /// <param name="id">id do usuario a ser deletado</param>
        public void Delete(int id);
    }
}
