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
    public class FuncionarioRepository : IFuncionarioRepository
    {
        GestaoFinancasContext _ctx = new GestaoFinancasContext();
        public void Create(Funcionario funcionario)
        {
            _ctx.Funcionarios.Add(funcionario);

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            Funcionario funcionario = _ctx.Funcionarios.Find(id);

            _ctx.Funcionarios.Remove(funcionario);

            _ctx.SaveChanges();
        }

        public Funcionario FindByUserId(int idUsuario)
        {
            return _ctx.Funcionarios.FirstOrDefault(c=> c.IdUsuario == idUsuario);
        }

        public List<Funcionario> Read()
        {
            return _ctx.Funcionarios.ToList();
        }

        public List<Funcionario> ReadBySetorId(int? idSetor)
        {
            return _ctx.Funcionarios.Include(x => x.IdUsuarioNavigation).Where(x => x.IdSetor == idSetor).ToList();
        }

        public void Update(Funcionario funcionario)
        {
            Funcionario funcionarioAntigo = _ctx.Funcionarios.Find(funcionario.IdFuncionario);

            if (funcionario.Cpf != null)
            {
                funcionarioAntigo.Cpf = funcionario.Cpf;
            }
            if (funcionario.Foto != null)
            {
                funcionarioAntigo.Foto = funcionario.Foto;
            }
            if (funcionario.Funcao != null)
            {
                funcionarioAntigo.Funcao = funcionario.Funcao;
            }
            if (funcionario.Nome != null)
            {
                funcionarioAntigo.Nome = funcionario.Nome;
            }

            _ctx.Funcionarios.Update(funcionarioAntigo);

            _ctx.SaveChanges();
        }
    }
}
