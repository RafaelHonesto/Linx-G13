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
    // ex: http://localhost:5000/api/setor
    [Route("api/[controller]")]

    // Define que é um controlador de API
    [ApiController]

    // Define que o tipo de resposta da API será no formato JSON
    [Produces("application/json")]


    public class SetoresController : ControllerBase
    {
        /// <summary>
        /// Objeto _setorRepository que irá receber todos os métodos definidos na interface ISetorRepository
        /// </summary>
        private ISetorRepository _setorRepository { get; set; }
        private IFuncionarioRepository _funcionarioRepository { get; set; }
        private ITipoDespesaRepository _tipoDespesaRepository { get; set; }
        private IDespesaRepository _despesaRepository { get; set; }
        private IEmpresaRepository _empresaRepository { get; set; }
        private IValoreRepository _valoreRepository { get; set; }
        private IUsuarioRepository _usuarioRepository { get; set; }

        /// <summary>
        /// Instancia o objeto _setorRepository para que haja a referência aos métodos no repositório
        /// </summary>
        public SetoresController()
        {
            _setorRepository = new SetorRepository();
            _funcionarioRepository = new FuncionarioRepository();
            _tipoDespesaRepository = new TipoDespesaRepository();
            _despesaRepository = new DespesaRepository();
            _empresaRepository = new EmpresaRepository();
            _valoreRepository = new ValoreRepository();
        }


        /// <summary>
        /// Lista todos os setores
        /// </summary>
        /// <returns>Uma lista de setores e um status code 200 - Ok</returns>
        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult ListarSetores()
        {
            List<Setor> Setores = _setorRepository.Read();

            return StatusCode(200, Setores);
        }

        [Authorize]
        [HttpGet("buscar/{nome}")]
        public IActionResult BucarNome(string nome)
        {
            try
            {
                Setor buscado = _setorRepository.BuscarNome(nome);

                return Ok(buscado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CriarSetor(Setor setor)
        {
            _setorRepository.Create(setor);

            TipoDespesa tipoDespesa = new TipoDespesa() {
                Titulo = "Energia",
                IdSetor = setor.IdSetor
            };

            _tipoDespesaRepository.Create(tipoDespesa);

            TipoDespesa tipoDespesa2 = new TipoDespesa()
            {
                Titulo = "Agua",
                IdSetor = setor.IdSetor
            };

            _tipoDespesaRepository.Create(tipoDespesa2);

            TipoDespesa tipoDespesa3 = new TipoDespesa()
            {
                Titulo = "Funcionarios",
                IdSetor = setor.IdSetor
            };

            _tipoDespesaRepository.Create(tipoDespesa3);

            return StatusCode(201, "Setor criado");
        }


        /// <summary>
        /// Atualiza um setor existente
        /// </summary>
        /// <param name="setor">Objeto com as novas informações</param>
        /// <returns>Um status code 204 - No Content</returns>
        // Define que somente o administrador pode acessar o método
        [Authorize(Roles = "1")]
        [HttpPut]
        public IActionResult EditarSetor(Setor setor)
        {
            Setor setorBuscado = _setorRepository.SearchById(setor.IdSetor);

            if (setorBuscado == null)
            {
                return StatusCode(404, "Setor nao encontrado");
            }

            _setorRepository.Update(setor);

            return StatusCode(200, "Setor editado");
        }


        /// <summary>
        /// Deleta um setor existente
        /// </summary>
        /// <param name="idSetor">ID do setor que será deletado</param>
        /// <returns>Um status code 204 - No Content</returns>
        // Define que somente o administrador pode acessar o método
        [Authorize(Roles = "1")]
        [HttpDelete]
        public IActionResult DeletarSetor(int idSetor)
        {
            if (_setorRepository.SearchById(idSetor) == null)
            {
                return StatusCode(404, "Setor nao encontrado");
            }

            List<Despesa> Despesas = _despesaRepository.Read(idSetor);

            foreach (Despesa despesa in Despesas)
            {
                _despesaRepository.Delete(despesa.IdDespesa);
            }

            List<TipoDespesa> TiposDespesa = _tipoDespesaRepository.ReadBySetorId(idSetor);

            foreach (TipoDespesa tipoDespesa in TiposDespesa)
            {
                _tipoDespesaRepository.Delete(tipoDespesa.IdTipoDespesa);
            }

            List<Valore> Valores = _valoreRepository.ReadBySetorId(idSetor);

            foreach (Valore valore in Valores)
            {
                _valoreRepository.Delete(valore.IdValor);
            }

            List<Empresa> Empresas = _empresaRepository.ReadBySetorId(idSetor);

            foreach (Empresa empresa in Empresas)
            {
                _empresaRepository.Delete(empresa.IdEmpresa);
            }

            List<Funcionario> Funcionarios = _funcionarioRepository.ReadBySetorId(idSetor);

            foreach (Funcionario funcionario in Funcionarios)
            {
                _funcionarioRepository.Delete(funcionario.IdFuncionario);
                _usuarioRepository.Delete(funcionario.IdUsuarioNavigation.IdUsuario);
            }

            _setorRepository.Delete(idSetor);

            return StatusCode(200, "Setor deletado");
        }

        [Authorize(Roles = "2,3")]
        [HttpGet("IdSetor")]
        public IActionResult BuscarSetor()
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            Setor setor = _setorRepository.SearchById(funcionario.IdSetor);

            return StatusCode(200, setor);
        }
        
        [Authorize(Roles = "1")]
        [HttpGet("IdSetor/{id}")]
        public IActionResult BuscarSetor(int? id)
        {
            Setor setor = _setorRepository.SearchById(id);

            return StatusCode(200, setor);
        }
    }
}
