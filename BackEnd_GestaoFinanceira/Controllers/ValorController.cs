using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using BackEnd_GestaoFinanceira.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public ValorController()
        {
            _valoreRepository = new ValoreRepository();
            _funcionarioRepository = new FuncionarioRepository();
        }

        [HttpGet]
        public IActionResult ListarValores()
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            List<Valore> Valores = _valoreRepository.FindBySetorId(funcionario.IdSetor)
        }
    }
}
