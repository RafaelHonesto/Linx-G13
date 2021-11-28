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

    // Define que é um controlador de API
    [Route("api/[controller]")]

    // Define que a rota de uma requisição será no formato dominio/api/nomeController
    // ex: http://localhost:5000/api/despesa
    [ApiController]

    // Define que o tipo de resposta da API será no formato JSON
    [Produces("application/json")]


    // Define que qualquer usuário autenticado pode acessar aos métodos
    // [Authorize]
    public class TiposDespesaController : ControllerBase
    {
        private ITipoDespesaRepository _tipoDespesaRepository { get; set; }
        private IFuncionarioRepository _funcionarioRepository { get; set; }
        public TiposDespesaController()
        {
            _tipoDespesaRepository = new TipoDespesaRepository();
            _funcionarioRepository = new FuncionarioRepository();
        }


        /// <summary>
        /// Lista todos os tipos de despesas
        /// </summary>
        /// <returns>Uma lista de despesa e um status code 200 - Ok</returns>
        [Authorize(Roles = "2,3")]
        [HttpGet]
        public IActionResult ListarTiposDespesa()
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            List<TipoDespesa> TiposDespesa = _tipoDespesaRepository.ReadBySetorId(funcionario.IdSetor);

            return StatusCode(200, TiposDespesa);
        }


        /// <summary>
        /// Cria uma nova despesa
        /// </summary>
        /// <param name="tipoDespesa">Objeto novoEvento que será cadastrado</param>
        /// <returns>Um status code 201 - Created</returns>
        // Define que somente o administrador e gestor pode acessar o método
        [Authorize(Roles = "2,3")]
        [HttpPost]
        public IActionResult CriarTipoDespesa(TipoDespesa tipoDespesa)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            tipoDespesa.IdSetor = funcionario.IdSetor;

            _tipoDespesaRepository.Create(tipoDespesa);

            return StatusCode(201, "Tipo de despesa criada");
        }


        /// <summary>
        /// Atualiza uma despesa existente
        /// </summary>
        /// <param name="id">ID da despesa que será atualizado</param>
        /// <param name="tipoDespesa">Objeto com as novas informações</param>
        /// <returns>Um status code 204 - No Content</returns>
        // Define que somente o administrador e gestor pode acessar o método
        [Authorize(Roles = "2")]
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


        /// <summary>
        /// Deleta uma despesa existente
        /// </summary>
        /// <param name="idTipoDespesa">ID da despesa que será deletado</param>
        /// <returns>Um status code 204 - No Content</returns>
        // Define que somente o administrador e gestor pode acessar o método
        [Authorize(Roles = "2")]
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
