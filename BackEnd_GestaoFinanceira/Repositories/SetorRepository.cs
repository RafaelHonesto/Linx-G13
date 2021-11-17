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
    public class SetorRepository : ISetorRepository
    {
        private GestaoFinancasContext _ctx = new GestaoFinancasContext();
        public void Create(Setor setor)
        {
            _ctx.Setors.Add(setor);

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            Setor setor = _ctx.Setors.Find(id);

            _ctx.Setors.Remove(setor);

            _ctx.SaveChanges();
        }

        public List<Setor> Read()
        {
            return _ctx.Setors
                .ToList();
        }

        public List<Setor> ReadId(int role)
        {
            return _ctx.Setors.ToList();
        }

        public Setor SearchById(int? id)
        {
            return _ctx.Setors.Include(x => x.Despesas).Include(x => x.Valores).FirstOrDefault(x => x.IdSetor == id);
        }

        public Setor BuscarNome(string nome)
        {
            return _ctx.Setors.FirstOrDefault(c => c.Nome == nome);
        }

        public void Update(Setor setor)
        {
            Setor setorAntigo = _ctx.Setors.Find(setor.IdSetor);

            if (setor.Nome != null)
            {
                setorAntigo.Nome = setor.Nome;
            }

            _ctx.Setors.Update(setor);

            _ctx.SaveChanges();
        }
    }
}
