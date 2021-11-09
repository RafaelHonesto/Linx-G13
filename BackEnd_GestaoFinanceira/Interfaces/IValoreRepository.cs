using BackEnd_GestaoFinanceira.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Interfaces
{
    public interface IValoreRepository
    {
        /// <summary>
        /// Cria valor
        /// </summary>
        /// <param name="valor">valor a ser criado</param>
        public void Create(Valore valor);

        /// <summary>
        /// Lista valores
        /// </summary>
        /// <returns>lista de valores</returns>
        public List<Valore> Read();

        /// <summary>
        /// Atualiza valor
        /// </summary>
        /// <param name="valor">valor a ser atualizado</param>
        public void Update(Valore valor);

        /// <summary>
        /// Deleta valor
        /// </summary>
        /// <param name="id">id do valor a ser deletado</param>
        public void Delete(int id);

        /// <summary>
        /// Lista valores pelo id
        /// </summary>
        /// <param name="idSetor">id do setor</param>
        /// <returns>Lista de valores</returns>
        public List<Valore> ReadBySetorId(int? idSetor);
    }
}
