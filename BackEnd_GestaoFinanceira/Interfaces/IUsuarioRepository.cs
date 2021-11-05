using BackEnd_GestaoFinanceira.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Interfaces
{
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Cria usuario
        /// </summary>
        /// <param name="usuario">Usuario a ser criado</param>
        public void Create(Usuario usuario);

        /// <summary>
        /// Lista usuarios
        /// </summary>
        /// <returns>lista de usuarios</returns>
        public List<Usuario> Read();

        /// <summary>
        /// Atualiza usuario
        /// </summary>
        /// <param name="usuario">usuario a ser atualizado</param>
        public void Update(Usuario usuario);

        /// <summary>
        /// Deleta usuario
        /// </summary>
        /// <param name="id">usuario a ser deletado</param>
        public void Delete(int id);

        /// <summary>
        /// Verifica email e senha
        /// </summary>
        /// <param name="usuario">Usuario com email e senha</param>
        /// <returns>Usuario se encontrado</returns>
        public Usuario VerificarEmailESenha(Usuario usuario);
    }
}
