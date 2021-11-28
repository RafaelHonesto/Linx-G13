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
    public class ValoreRepository : IValoreRepository
    {
        private GestaoFinancasContext _ctx = new GestaoFinancasContext();
        public void Create(Valore valor)
        {
            _ctx.Valores.Add(valor);

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            Valore valor = _ctx.Valores.Find(id);

            _ctx.Valores.Remove(valor);

            _ctx.SaveChanges();
        }

        public List<Valore> Read()
        {
            return _ctx.Valores
                .Include(c=> c.IdEmpresaNavigation)
                .ToList();
        }

        public List<Valore> ReadBySetorId(int? idSetor)
        {
            return _ctx.Valores.Where(x => x.IdSetor == idSetor)
                .Include(c => c.IdEmpresaNavigation)
                .ToList();
        }

        public Valore SearchById(int idValor)
        {
            return _ctx.Valores.Find(idValor);
        }

        public void Update(Valore valor)
        {
            Valore valorAntigo = _ctx.Valores.Find(valor.IdValor);

            if (valor.Foto != null)
            {
                valorAntigo.Foto = valor.Foto;
            }
            if (valor.DataValor != null)
            {
                valorAntigo.DataValor = valor.DataValor;
            }
            if (valor.Descricao != null)
            {
                valorAntigo.Descricao = valor.Descricao;
            }
            if (valor.TipoEntrada != null)
            {
                valorAntigo.TipoEntrada = valor.TipoEntrada;
            }
            if (valor.Valor != null)
            {
                valorAntigo.Valor = valor.Valor;
            }
            if (valor.Titulo != null)
            {
                valorAntigo.Titulo = valor.Titulo;
            }
            if (valor.Pago != null)
            {
                valorAntigo.Pago = valor.Pago;
            }

            _ctx.Valores.Update(valorAntigo);

            _ctx.SaveChanges();
        }
    }
}
