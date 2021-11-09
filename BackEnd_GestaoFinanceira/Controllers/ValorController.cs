using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using BackEnd_GestaoFinanceira.Repositories;
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
    public class ValorController : ControllerBase
    {
        private IValoreRepository _valoreRepository { get; set; }
        private IFuncionarioRepository _funcionarioRepository { get; set; }
        private IEmpresaRepository _empresaRepository { get; set; }
        public ValorController()
        {
            _valoreRepository = new ValoreRepository();
            _funcionarioRepository = new FuncionarioRepository();
            _empresaRepository = new EmpresaRepository();
        }

        [HttpGet]
        public IActionResult ListarValores()
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            List<Valore> Valores = _valoreRepository.ReadBySetorId(funcionario.IdSetor);

            return StatusCode(200, Valores);
        }

        [HttpPost]
        public IActionResult CriarValor(Valore valor)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            valor.IdSetor = funcionario.IdSetor;

            Empresa empresa = _empresaRepository.SearchById(valor.IdEmpresa);

            if (empresa == null)
            {
                return StatusCode(404, "Empresa nao encontrada");
            }

            if (empresa.IdSetor != funcionario.IdSetor)
            {
                return StatusCode(401, "Usuario nao pode adicionar valores no setor");
            }

            _valoreRepository.Create(valor);

            return StatusCode(200, "Valor criado");
        }
    }
}
