using BackEnd_GestaoFinanceira.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Interfaces
{
    public interface ISetorRepository
    {
        /// <summary>
        /// Cria setor
        /// </summary>
        /// <param name="setor">setor a ser criado</param>
        public void Create(Setor setor);

        /// <summary>
        /// Lista setores
        /// </summary>
        /// <returns></returns>
        public List<Setor> Read();

        /// <summary>
        /// Atualiza setor
        /// </summary>
        /// <param name="setor">setor a ser atualizado</param>
        public void Update(Setor setor);

        /// <summary>
        /// Deleta setor
        /// </summary>
        /// <param name="id">setor a ser deletado</param>
        public void Delete(int id);

        /// <summary>
        /// Busca setor pelo id
        /// </summary>
        /// <param name="id">id do setor buscado</param>
        /// <returns>setor buscado se encontrado</returns>
        public Setor SearchById(int? id);

        /// <summary>
        /// Busca setor pelo id
        /// </summary>
        /// <param name="id">id do setor buscado</param>
        /// <returns>setor buscado se encontrado</returns>
        public Setor BuscarNome(string nome);
    }
}
