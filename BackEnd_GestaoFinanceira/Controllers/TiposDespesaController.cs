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
    public class TiposDespesaController : ControllerBase
    {
        private ITipoDespesaRepository _tipoDespesaRepository { get; set; }
        private IFuncionarioRepository _funcionarioRepository { get; set; }
        public TiposDespesaController()
        {
            _tipoDespesaRepository = new TipoDespesaRepository();
            _funcionarioRepository = new FuncionarioRepository();
        }

        [Authorize(Roles = "2, 3")]
        [HttpGet]
        public IActionResult ListarTiposDespesa()
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            List<TipoDespesa> TiposDespesa = _tipoDespesaRepository.ReadBySetorId(funcionario.IdSetor);

            return StatusCode(200, TiposDespesa);
        }

        [Authorize(Roles = "2, 3")]
        [HttpPost]
        public IActionResult CriarTipoDespesa(TipoDespesa tipoDespesa)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            tipoDespesa.IdSetor = funcionario.IdSetor;

            _tipoDespesaRepository.Create(tipoDespesa);

            return StatusCode(201, "Tipo de despesa criada");
        }

        [Authorize(Roles = "2, 3")]
        [HttpPut]
        public IActionResult EditarTipoDespesa(TipoDespesa tipoDespesa)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            TipoDespesa tipoDespesaBuscada = _tipoDespesaRepository.SearchById(tipoDespesa.IdTipoDespesa);

            if (tipoDespesaBuscada == null)
            {
                return StatusCode(404, "Tipo despesa nao encontrada");
            }

            if (tipoDespesa.IdSetor != funcionario.IdSetor)
            {
                return StatusCode(401, "Tipo de despesa nao e do setor do usuario");
            }

            _tipoDespesaRepository.Update(tipoDespesa);

            return StatusCode(200, "Tipo de despesa editado");
        }

        [Authorize(Roles = "2, 3")]
        [HttpDelete]
        public IActionResult DeletarTipoDespesa(int idTipoDespesa)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            TipoDespesa tipoDespesa = _tipoDespesaRepository.SearchById(idTipoDespesa);

            if (tipoDespesa == null)
            {
                return StatusCode(404, "Tipo despesa nao encontrada");
            }

            if (tipoDespesa.IdSetor != funcionario.IdSetor)
            {
                return StatusCode(401, "Tipo de despesa nao e do setor do usuario");
            }

            _tipoDespesaRepository.Delete(idTipoDespesa);

            return StatusCode(200, "Tipo de despesa deletada");
        }
    }
}
