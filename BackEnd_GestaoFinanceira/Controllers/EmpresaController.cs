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
    /// <summary>
    /// Controller responsável pelos endpoints (URLs) referentes aos eventos
    /// </summary>

    // Define que a rota de uma requisição será no formato dominio/api/nomeController
    // ex: http://localhost:5000/api/empresa
    [Route("api/[controller]")]

    // Define que é um controlador de API
    [ApiController]

    // Define que o tipo de resposta da API será no formato JSON
    [Produces("application/json")]

    // Define que qualquer usuário autenticado pode acessar aos métodos
    // [Authorize]
    public class EmpresaController : ControllerBase
    {
        /// <summary>
        /// Objeto _empresaRepository que irá receber todos os métodos definidos na interface IEmpresaRepository
        /// </summary>
        private IEmpresaRepository _empresaRepository { get; set; }
        /// <summary>
        /// Objeto _funcionarioRepository que irá receber todos os métodos definidos na interface IFuncionarioRepository
        /// </summary>
        private IFuncionarioRepository _funcionarioRepository { get; set; }

        /// <summary>
        /// Instancia o objeto _eventoRepository para que haja a referência aos métodos no repositório
        /// </summary>
        public EmpresaController()
        {
            _empresaRepository = new EmpresaRepository();
            _funcionarioRepository = new FuncionarioRepository();
        }

        /// <summary>
        /// Lista todas as empresas
        /// </summary>
        /// <returns>Uma lista de empreas e um status code 200 - Ok</returns>
        [Authorize(Roles = "2, 3")]
        [HttpGet]
        public IActionResult ListarEmpresas()
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            List<Empresa> Empresas = _empresaRepository.ReadBySetorId(funcionario.IdSetor);

            return StatusCode(200, Empresas);
        }


        /// <summary>
        /// Cadastra uma nova empresa
        /// </summary>
        /// <param name="empresa">Objeto empresa que será cadastrado</param>
        /// <returns>Um status code 201 - Created</returns>
        // Define que somente o gestor e usuario pode acessar o método
        [Authorize(Roles = "2, 3")]
        [HttpPost]
        public IActionResult CriarEmpresa(Empresa empresa)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            empresa.IdSetor = funcionario.IdSetor;

            _empresaRepository.Create(empresa);

            return StatusCode(200, "Empresa criada");
        }


        /// <summary>
        /// Atualiza uma empresa existente
        /// </summary>
        /// <param name="id">ID da empresa que será atualizado</param>
        /// <param name="empresa">Objeto com as novas informações</param>
        /// <returns>Um status code 204 - No Content</returns>
        // Define que somente o gestor e ususario pode acessar o método
        [Authorize(Roles = "2, 3")]
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

        /// <summary>
        /// Deleta uma empresa existente
        /// </summary>
        /// <param name="idEmpresa">ID do empresa que será deletado</param>
        /// <returns>Um status code 204 - No Content</returns>
        // Define que somente o gestor e ususario pode acessar o método
        [Authorize(Roles = "2, 3")]
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
