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

            List<Empresa> Empresas = _empresaRepository.ReadBySetorId(funcionario.IdSetor);

            return StatusCode(200, Empresas);
        }

        [HttpPost]
        public IActionResult CriarEmpresa(Empresa empresa)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            empresa.IdSetor = funcionario.IdSetor;

            _empresaRepository.Create(empresa);

            return StatusCode(200, "Empresa criada");
        }

        [HttpPut]
        public IActionResult EditarEmpresa(Empresa empresa)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            Empresa empresaBuscada = _empresaRepository.SearchById(empresa.IdEmpresa);

            if (empresaBuscada == null)
            {
                return StatusCode(404, "Empresa nao encontrada");
            }

            _empresaRepository.Update(empresa);

            return StatusCode(200, "Empresa editada");
        }

        [HttpDelete]
        public IActionResult DeletarEmpresa(int idEmpresa)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            Empresa empresa = _empresaRepository.SearchById(idEmpresa);

            if (empresa == null)
            {
                return StatusCode(404, "Empresa nao encontrada");
            }

            if (empresa.IdSetor != funcionario.IdFuncionario)
            {
                return StatusCode(401, "Empresa nao e do setor do usuario");
            }

            _empresaRepository.Delete(idEmpresa);

            return StatusCode(200, "Empresa deletada");
        }
    }
}
