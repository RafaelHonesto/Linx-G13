using BackEnd_GestaoFinanceira.Domains;
using System.Collections.Generic;

namespace BackEnd_GestaoFinanceira.Interfaces
{
    public interface IDespesaRepository
    {
        /// <summary>
        /// Cria despesa
        /// </summary>
        /// <param name="despesa">despesa a ser adicionada</param>
        public void Create(Despesa despesa);

        /// <summary>
        /// Lista despesas
        /// </summary>
        /// <param name="idSetor">id do setor a listar as despesas</param>
        /// <returns>Lista de despesas</returns>
        public List<Despesa> Read(int idSetor);

        /// <summary>
        /// Atualiza despesa
        /// </summary>
        /// <param name="despesa">despesa a ser atualizada</param>
        /// <param name="idSetor">id do setor no JWT</param>
        /// <returns>confirmacao de atualizacao</returns>
        public bool Update(Despesa despesa, int idSetor);

        /// <summary>
        /// Deleta despesa
        /// </summary>
        /// <param name="idDespesa">id da empresa a ser deletada</param>
        /// <param name="idSetor">id do setor no JWT</param>
        /// <returns>conirmacao de exclusao</returns>
        public bool Delete(int idDespesa, int idSetor);
    }
}
