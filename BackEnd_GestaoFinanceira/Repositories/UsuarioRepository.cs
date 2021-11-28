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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly GestaoFinancasContext _ctx = new GestaoFinancasContext();
        /// <summary>
        /// Cria usuario
        /// </summary>
        /// <param name="usuario">usuario a ser criado</param>
        public Usuario Create(Usuario usuario)
        {
            usuario.SenhaDeAcesso = usuario.SenhaDeAcesso;

            _ctx.Usuarios.Add(usuario);

            _ctx.SaveChanges();

            return _ctx.Usuarios.FirstOrDefault(x => x.Acesso == usuario.Acesso);
        }


        public void Delete(int id)
        {
            Usuario usuario = _ctx.Usuarios.Find(id);

            if (usuario == null)
            {
                return;
            }

            _ctx.Usuarios.Remove(usuario);

            _ctx.SaveChanges();
        }

        public Usuario FindByUserId(int id)
        {
            Usuario usuario = _ctx.Usuarios.Find(id);

            usuario.SenhaDeAcesso = "";

            return usuario;
        }

        public List<Usuario> Read()
        {
            return _ctx.Usuarios
                .Include(x => x.Funcionario) 
                .Select(x => new Usuario() { 
                    Acesso = x.Acesso,
                    IdUsuario = x.IdUsuario,
                    IdTipoUsuario = x.IdTipoUsuario
                })
                .ToList();
        }

        public void Update(Usuario usuario)
        {
            Usuario usuarioAntigo = _ctx.Usuarios.Find(usuario.IdUsuario);
            if (usuario.Acesso != null)
            {
                usuarioAntigo.Acesso = usuario.Acesso;
            }
            if (usuario.SenhaDeAcesso != null)
            {
                usuarioAntigo.SenhaDeAcesso = usuario.SenhaDeAcesso;
            }

            _ctx.Usuarios.Update(usuarioAntigo);

            _ctx.SaveChanges();
        }

        public Usuario VerificarEmailESenha(Usuario usuario)
        {
            return _ctx.Usuarios.Include(x => x.Funcionario).FirstOrDefault(x => x.Acesso == usuario.Acesso && x.SenhaDeAcesso == usuario.SenhaDeAcesso);
        }

        private string Criptografar(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }
    }
}
