using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using BackEnd_GestaoFinanceira.Repositories;
using Microsoft.AspNetCore.Authorization;
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
    public class SetoresController : ControllerBase
    {
        private ISetorRepository _setorRepository { get; set; }

        public SetoresController()
        {
            _setorRepository = new SetorRepository();
        }

        [Authorize]
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

            return StatusCode(201, "Setor criado");
        }

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

        [Authorize(Roles = "1")]
        [HttpDelete]
        public IActionResult DeletarSetor(int idSetor)
        {
            if (_setorRepository.SearchById(idSetor) == null)
            {
                return StatusCode(404, "Setor nao encontrado");
            }

            _setorRepository.Delete(idSetor);

            return StatusCode(200, "Setor deletado");
        }
    }
}
