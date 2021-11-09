using BackEnd_GestaoFinanceira.Contexts;
using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly GestaoFinancasContext _ctx = new GestaoFinancasContext();
        /// <summary>
        /// Cria empresa
        /// Insira o setor da empresa que estiver no JWT
        /// </summary>
        /// <param name="empresa">empresa a ser criada</param>
        public void Create(Empresa empresa)
        {
            _ctx.Empresas.Add(empresa);

            _ctx.SaveChanges();
        }

        /// <summary>
        /// Deleta empresa
        /// </summary>
        /// <param name="idEmpresa">id da empresa a ser deletada</param>
        /// <param name="idSetor">id do setor no JWT</param>
        /// <returns>confirmacao de exclusao</returns>
        public bool Delete(int idEmpresa, int idSetor)
        {
            Empresa empresa = _ctx.Empresas.Find(idEmpresa);

            if (empresa == null)
            {
                return false;
            }

            if (empresa.IdSetor != idSetor)
            {
                return false;
            }

            _ctx.Empresas.Remove(empresa);

            _ctx.SaveChanges();

            return true;
        }

        /// <summary>
        /// Lista empresas pelo id do setor no JWT
        /// </summary>
        /// <returns>lista de empresas</returns>
        public List<Empresa> Read()
        {
            return _ctx.Empresas.ToList();;
        }

        /// <summary>
        /// Lista empresas pelo id do setor
        /// </summary>
        /// <param name="idSetor">id do setor</param>
        /// <returns>Lista de empresas</returns>
        public List<Empresa> ReadBySetorId(int? idSetor)
        {
            return _ctx.Empresas.Where(x => x.IdSetor == idSetor).ToList();
        }

        public Empresa SearchById(int? idEmpresa)
        {
            return _ctx.Empresas.Find(idEmpresa);
        }

        /// <summary>
        /// Atualiza empresa
        /// verifique se o id do setor no JWT 
        /// </summary>
        /// <param name="empresa">empresa a ser atualizada</param>
        /// <param name="idSetor">id do setor no JWT</param>
        /// <returns>confirmacao de atualizacao</returns>
        public bool Update(Empresa empresa, int idSetor)
        {
            Empresa empresaAntiga = _ctx.Empresas.Find(empresa.IdEmpresa);

            if (empresaAntiga.IdSetor == idSetor)
            {
                return false;
            }

            if (empresa.NomeEmpresa != null)
            {
                empresaAntiga.NomeEmpresa = empresa.NomeEmpresa;
            }
            if (empresa.Cnpj != null)
            {
                empresaAntiga.Cnpj = empresa.Cnpj;
            }

            _ctx.Empresas.Update(empresa);

            _ctx.SaveChanges();

            return true;
        }

        public bool Update(Empresa empresa)
        {
            throw new NotImplementedException();
        }
    }
}
