using BackEnd_GestaoFinanceira.Contexts;
using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Repositories
{
    public class MetaRepository : IMetaRepository
    {
        private readonly GestaoFinancasContext _ctx = new GestaoFinancasContext();
        public void Create(Meta meta)
        {
            _ctx.Metas.Add(meta);

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            Meta meta = _ctx.Metas.Find(id);

            _ctx.Metas.Remove(meta);

            _ctx.SaveChanges();
        }

        public Meta FindById(int id)
        {
            return _ctx.Metas.Find(id);
        }

        public List<Meta> Read(int? idSetor)
        {
            return _ctx.Metas.Where(x => x.IdSetor == idSetor).ToList();
        }

        public void Update(Meta meta)
        {
            Meta metaAntiga = _ctx.Metas.Find(meta.IdMeta);

            if (meta.Meta1 != null)
            {
                metaAntiga.Meta1 = meta.Meta1;
            }
            if (meta.Mes != null)
            {
                metaAntiga.Mes = meta.Mes;
            }

            _ctx.Metas.Update(metaAntiga);

            _ctx.SaveChanges();
        }
    }
}
