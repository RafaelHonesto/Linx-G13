using BackEnd_GestaoFinanceira.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Interfaces
{
    public interface IMetaRepository
    {
        public void Create(Meta meta);

        public List<Meta> Read(int? idSetor);

        public void Update(Meta meta);

        public void Delete(int id);

        public Meta FindById(int id);
    }
}
