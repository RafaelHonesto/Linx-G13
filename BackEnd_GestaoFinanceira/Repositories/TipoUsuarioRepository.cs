using BackEnd_GestaoFinanceira.Contexts;
using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private GestaoFinancasContext _ctx = new GestaoFinancasContext();
        public void Create(TipoUsuario tipoUsuario)
        {
            _ctx.TipoUsuarios.Add(tipoUsuario);

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            TipoUsuario tipoUsuario = _ctx.TipoUsuarios.Find(id);

            _ctx.TipoUsuarios.Remove(tipoUsuario);

            _ctx.SaveChanges();
        }

        public List<TipoUsuario> Read()
        {
            return _ctx.TipoUsuarios.ToList();
        }

        public void Update(TipoUsuario tipoUsuario)
        {
            TipoUsuario tipoUsuarioAntigo = _ctx.TipoUsuarios.Find(tipoUsuario.IdTipoUsuario);

            if (tipoUsuario.Titulo != null)
            {
                tipoUsuarioAntigo.Titulo = tipoUsuario.Titulo;
            }

            _ctx.TipoUsuarios.Update(tipoUsuarioAntigo);

            _ctx.SaveChanges();
        }
    }
}
