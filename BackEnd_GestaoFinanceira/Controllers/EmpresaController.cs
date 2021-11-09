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
    public class EmpresaController : ControllerBase
    {
        private IEmpresaRepository _empresaRepository { get; set; }
        private IFuncionarioRepository _funcionarioRepository { get; set; }
        public EmpresaController()
        {
            _empresaRepository = new EmpresaRepository();
            _funcionarioRepository = new FuncionarioRepository();
        }

        [HttpGet]
        public IActionResult ListarEmpresas()
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            List<Empresa> Empresas = _empresaRepository.ReadByIdSetor(funcionario.IdSetor);
        }
    }
}
