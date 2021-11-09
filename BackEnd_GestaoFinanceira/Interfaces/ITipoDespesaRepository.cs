using BackEnd_GestaoFinanceira.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Interfaces
{
    public interface ITipoDespesaRepository
    {
        public void Create(TipoDespesa tipoDespesa);

        public List<TipoDespesa> Read();

        public void Update(TipoDespesa tipoDespesa);

        public void Delete(int id);

        public List<TipoDespesa> ReadBySetorId(int? idSetor);

        public TipoDespesa SearchById(int? idTipoDespesa);
    }
}
