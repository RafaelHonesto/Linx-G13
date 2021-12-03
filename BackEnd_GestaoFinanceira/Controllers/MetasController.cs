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
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MetasController : ControllerBase
    {
        IMetaRepository _metaRepository { get; set; }
        IFuncionarioRepository _funcionarioRepository { get; set; }

        public MetasController()
        {
            _metaRepository = new MetaRepository();
        }

        [Authorize(Roles = "2,3")]
        [HttpGet]
        public IActionResult ListarMetas()
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            if (funcionario == null)
            {
                return StatusCode(401, "Funcionario nao existe");
            }

            List<Meta> Metas = _metaRepository.Read(funcionario.IdSetor);

            return StatusCode(200, Metas);
        }

        [Authorize(Roles = "2,3")]
        [HttpPost]
        public IActionResult CriarMeta(Meta meta)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            if (funcionario == null)
            {
                return StatusCode(401, "Funcionario nao existe");
            }

            meta.IdSetor = funcionario.IdSetor;

            _metaRepository.Create(meta);

            return StatusCode(201, "Imagem cadastrada");
        }

        [Authorize(Roles = "2")]
        [HttpPut]
        public IActionResult EditarMeta(Meta meta)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            if (funcionario == null)
            {
                return StatusCode(401, "Funcionario nao existe");
            }

            Meta metaBuscada = _metaRepository.FindById(meta.IdMeta);

            if (metaBuscada == null)
            {
                return StatusCode(404, "Meta nao existe");
            }
            if (metaBuscada.IdSetor != funcionario.IdSetor)
            {
                return StatusCode(401, "Meta nao e do setor do funcionanrio");
            }

            _metaRepository.Update(meta);

            return StatusCode(200, "Meta atualizada");
        }

        [Authorize(Roles = "2")]
        [HttpDelete("deletar/{idMeta}")]
        public IActionResult DeletarMeta(int idMeta)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            if (funcionario == null)
            {
                return StatusCode(401, "Usuario nao existe");
            }

            Meta meta = _metaRepository.FindById(idMeta);

            if (meta == null)
            {
                return StatusCode(404, "Meta nao existe");
            }
            if (meta.IdSetor != funcionario.IdSetor)
            {
                return StatusCode(401, "Meta nao e do setor do funcionario");
            }

            _metaRepository.Delete(idMeta);

            return StatusCode(200, "Meta deletada");
        }

        [Authorize(Roles = "2,3")]
        [HttpGet("buscar/{idMeta}")]
        public IActionResult BuscarMetaPorId(int idMeta)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            if (funcionario == null)
            {
                return StatusCode(401, "Funcionario nao existe");
            }

            Meta meta = _metaRepository.FindById(idMeta);

            if (meta == null)
            {
                return StatusCode(404, "Meta nao encontrada");
            }
            if (meta.IdSetor != funcionario.IdSetor)
            {
                return StatusCode(401, "Meta nao e do setor do usuario");
            }

            return StatusCode(200, meta);
        }
    }
}
