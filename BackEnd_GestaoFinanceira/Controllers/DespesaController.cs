
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
    // ex: http://localhost:5000/api/despesa
    [Route("api/[controller]")]

    // Define que é um controlador de API
    [ApiController]

    // Define que o tipo de resposta da API será no formato JSON
    [Produces("application/json")]

    // Define que qualquer usuário autenticado pode acessar aos métodos
    // [Authorize]
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


        /// <summary>
        /// Lista todas as despesas
        /// </summary>
        /// <returns>Uma lista de despesa e um status code 200 - Ok</returns>
        [Authorize(Roles = "2,3")]
        [HttpGet]
        public IActionResult ListarDespesasDoSetor()
        {

            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            List<Despesa> Despesas = _despesaRepository.Read(funcionario?.IdSetor);

            return StatusCode(200, Despesas);
        }


        /// <summary>
        /// Cadastra uma nova despesa
        /// </summary>
        /// <param name="despesa">Objeto despesa que será cadastrado</param>
        /// <returns>Um status code 201 - Created</returns>
        // Define que somente o setor funcionario pode acessar o método
        [Authorize(Roles = "2,3")]
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


        /// <summary>
        /// Atualiza uma despesa existente
        /// </summary>
        /// <param name="id">ID do despesa que será atualizada</param>
        /// <param name="despesa">Objeto com as novas informações</param>
        /// <returns>Um status code 204 - No Content</returns>
        // Define que somente o administrador pode acessar o método
        [Authorize(Roles = "2")]
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


        /// <summary>
        /// Deleta uma despesa existente
        /// </summary>
        /// <param name="id">ID da despesa que será deletado</param>
        /// <param name="despesa">Objeto com as novas informações</param>
        /// <returns>Um status code 204 - No Content</returns>
        // Define que somente o gestor e usuario pode acessar o método
        [Authorize(Roles = "2")]
        [HttpDelete("{despesa}")]
        public IActionResult DeletarDespesaDoSetor(int despesa)
        {

            Despesa despesaBuscada = _despesaRepository.SearchById(despesa);

            if (despesaBuscada == null)
            {
                return StatusCode(400, "A despesa nao existe");
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
