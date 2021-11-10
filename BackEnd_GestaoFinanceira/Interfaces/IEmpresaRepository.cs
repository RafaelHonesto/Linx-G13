using BackEnd_GestaoFinanceira.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Interfaces
{
    public interface IEmpresaRepository
    {
        /// <summary>
        /// Cria empresa
        /// </summary>
        /// <param name="empresa">empresa a ser criada</param>
        public void Create(Empresa empresa);

        /// <summary>
        /// Lista empresas
        /// </summary>
        /// <returns>empresa a ser listada</returns>
        public List<Empresa> Read();

        /// <summary>
        /// Atualiza empresa
        /// </summary>
        /// <param name="empresa">empresa a ser atualizada</param>
        public void Update(Empresa empresa);

        /// <summary>
        /// Deleta empresa
        /// </summary>
        /// <param name="idEmpresa">id da empresa a ser deletada</param>
        public void Delete(int idEmpresa);

        /// <summary>
        /// Lista empresas de setor
        /// </summary>
        /// <param name="idSetor">id do setor</param>
        /// <returns>lista de empresas</returns>
        public List<Empresa> ReadBySetorId(int? idSetor);

        /// <summary>
        /// Busca empresa pelo id
        /// </summary>
        /// <param name="idEmpresa">id da empresa a ser usada</param>
        /// <returns>Empresa buscada se encontrada</returns>
        public Empresa SearchById(int? idEmpresa);
    }
}
