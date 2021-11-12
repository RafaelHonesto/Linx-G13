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
    /// Controller responsável pelos endpoints (URLs) referentes aos usuários
    /// </summary>

    // Define que a rota de uma requisição será no formato dominio/api/nomeController
    // ex: http://localhost:5000/api/valores
    [Route("api/[controller]")]

    // Define que é um controlador de API
    [ApiController]

    // Define que o tipo de resposta da API será no formato JSON
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


        /// <summary>
        /// Lista todos os valores
        /// </summary>
        /// <returns>Uma lista de eventos e um status code 200 - Ok</returns>
        [Authorize(Roles = "2, 3")]
        [HttpGet]
        public IActionResult ListarValores()
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            List<Valore> Valores = _valoreRepository.ReadBySetorId(funcionario.IdSetor);

            return StatusCode(200, Valores);
        }


        /// <summary>
        /// Cadastra um novo valor
        /// </summary>
        /// <param name="valor">Objeto valor que será cadastrado</param>
        /// <returns>Um status code 201 - Created</returns>
        // Define que somente o administrador pode acessar o método
        [Authorize(Roles = "2, 3")]
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
