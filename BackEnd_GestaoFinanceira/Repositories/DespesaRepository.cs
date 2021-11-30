using BackEnd_GestaoFinanceira.Contexts;
using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Repositories
{
    public class DespesaRepository : IDespesaRepository
    {
        private readonly GestaoFinancasContext _ctx = new GestaoFinancasContext();
        /// <summary>
        /// Cria despesa
        /// Verifique se o id do setor no JWT e o mesmo da despesa
        /// </summary>
        /// <param name="despesa">Despesa a ser criada</param>
        public void Create(Despesa despesa)
        {
            _ctx.Despesas.Add(despesa);

            _ctx.SaveChanges();
        }

        /// <summary>
        /// Deleta despesa
        /// </summary>
        /// <param name="idDespesa">id da despesa a ser deletada</param>
        /// <returns>confirmacao de exclusao</returns>
        public bool Delete(int idDespesa)
        {
            Despesa despesa = _ctx.Despesas.Find(idDespesa);

            if (despesa == null)
            {
                return false;
            }

            _ctx.Despesas.Remove(despesa);

            _ctx.SaveChanges();

            return true;
        }

        /// <summary>
        /// Lista despesas do setor
        /// </summary>
        /// <param name="idSetor">id do setor a listar despesas</param>
        /// <returns>Lista de despesas do setor</returns>
        public List<Despesa> Read(int? idSetor)
        {
            return _ctx.Despesas.Where(x => x.IdSetor == idSetor)
                .Include(c=> c.IdTipoDespesaNavigation)
                .ToList();
        }

        public Despesa SearchById(int idDespesa)
        {
            return _ctx.Despesas.Find(idDespesa);
        }

        /// <summary>
        /// Atualiza despesa
        /// </summary>
        /// <param name="despesa">despesa do setor a ser atualizada</param>
        /// <returns>confirmacao de atualizacao</returns>
        public bool Update(Despesa despesa)
        {
            Despesa despesaAntiga = _ctx.Despesas.Find(despesa.IdDespesa);

            if (despesaAntiga == null)
            {
                return false;
            }

            if (despesa.Descricao != null)
            {
                despesaAntiga.Descricao = despesa.Descricao;
            }
            if (despesa.IdTipoDespesa != null)
            {
                despesaAntiga.IdTipoDespesa = despesa.IdTipoDespesa;
            }
            if (despesa.Valor != null)
            {
                despesaAntiga.Valor = despesa.Valor;
            }
            if (despesa.Pago != null)
            {
                despesaAntiga.Pago = despesa.Pago;
            }

            _ctx.Despesas.Update(despesaAntiga);

            _ctx.SaveChanges();

            return true;
        }
    }
}
