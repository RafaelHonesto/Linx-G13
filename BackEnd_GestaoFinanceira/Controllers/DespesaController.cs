
using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using BackEnd_GestaoFinanceira.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DespesaController : ControllerBase
    {
        private IDespesaRepository _despesaRepository { get; set; }
        private ISetorRepository _setorRepository { get; set; }
        private IFuncionarioRepository _funcionarioRepository { get; set; }
        private ITipoDespesaRepository _tipoDespesaRepository { get; set; }
        public DespesaController()
        {
            _despesaRepository = new DespesaRepository();
            _setorRepository = new SetorRepository();
            _funcionarioRepository = new FuncionarioRepository();
            _tipoDespesaRepository = new TipoDespesaRepository();
        }

        [Authorize(Roles = "2, 3")]
        [HttpGet]
        public IActionResult ListarDespesasDoSetor(int idSetor)
        {
            if (_setorRepository.SearchById(idSetor) == null)
            {
                return StatusCode(404, "Setor nao encontrado");
            }

            List<Despesa> Despesas = _despesaRepository.Read(idSetor);

            return StatusCode(200, Despesas);
        }

        [Authorize(Roles = "2, 3")]
        [HttpPost]
        public IActionResult CriarDespesaDoSetor(Despesa despesa)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            TipoDespesa tipoDespesa = _tipoDespesaRepository.SearchById(despesa.IdTipoDespesa);

            if (tipoDespesa == null)
            {
                return StatusCode(404, "Tipo despesa nao existe");
            }

            despesa.IdSetor = funcionario.IdSetor;

            _despesaRepository.Create(despesa);

            return StatusCode(201, "Despesa criada");
        }

        [Authorize(Roles = "2, 3")]
        [HttpPut]
        public IActionResult EditarDespesaDoSetor(Despesa despesa)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            Despesa despesaAntiga = _despesaRepository.SearchById(despesa.IdDespesa);

            if (despesa.IdTipoDespesa != null)
            {
                TipoDespesa tipoDespesa = _tipoDespesaRepository.SearchById(despesa.IdTipoDespesa);

                if (tipoDespesa == null)
                {
                    return StatusCode(404, "Tipo despesa nao existe");
                }
            }

            if (despesaAntiga == null)
            {
                return StatusCode(404, "despesa nao encontrada");
            }

            if (funcionario.IdSetor != despesaAntiga.IdSetor)
            {
                return StatusCode(400, "O funcionario nao pode alterar informacoes desse setor");
            }

            _despesaRepository.Update(despesa);

            return StatusCode(200, "Despesa atualizada");
        }

        [Authorize(Roles = "2, 3" +
            "")]
        [HttpDelete]
        public IActionResult DeletarDespesaDoSetor(int despesa)
        {
            Despesa despesaBuscada = _despesaRepository.SearchById(despesa);

            if (despesaBuscada == null)
            {
                return StatusCode(400, "O setor nao existe");
            }

            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            if (despesaBuscada.IdSetor != funcionario.IdSetor)
            {
                return StatusCode(401, "Funcionario nao faz parte do setor");
            }

            _despesaRepository.Delete(despesa);

            return StatusCode(200, "despesa deletada");
        }
    }
}
