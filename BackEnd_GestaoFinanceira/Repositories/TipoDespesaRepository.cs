using BackEnd_GestaoFinanceira.Contexts;
using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Repositories
{
    public class TipoDespesaRepository : ITipoDespesaRepository
    {
        private GestaoFinancasContext _ctx = new GestaoFinancasContext();
        public void Create(TipoDespesa tipoDespesa)
        {
            _ctx.TipoDespesas.Add(tipoDespesa);

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            TipoDespesa tipoDespesa = _ctx.TipoDespesas.Find(id);

            _ctx.TipoDespesas.Remove(tipoDespesa);

            _ctx.SaveChanges();
        }

        public List<TipoDespesa> Read()
        {
            return _ctx.TipoDespesas.ToList();
        }

        public void Update(TipoDespesa tipoDespesa)
        {
            TipoDespesa tipoDespesaAntigo = _ctx.TipoDespesas.Find(tipoDespesa.IdTipoDespesa);

            if (tipoDespesa.Titulo != null)
            {
                tipoDespesaAntigo.Titulo = tipoDespesa.Titulo;
            }

            _ctx.TipoDespesas.Update(tipoDespesaAntigo);

            _ctx.SaveChanges();
        }
    }
}
