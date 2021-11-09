using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using BackEnd_GestaoFinanceira.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioRepository _usuarioRepository { get; set; }
        private IFuncionarioRepository _funcionarioRepository { get; set; }

        public UsuarioController()
        {
            _usuarioRepository = new UsuarioRepository();
            _funcionarioRepository = new FuncionarioRepository();
        }

        // Metodos comuns

        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            usuario = _usuarioRepository.VerificarEmailESenha(usuario);

            if (usuario == null)
            {
                return StatusCode(400, "Email ou senha incorreto");
            }

            int idFuncionario = _funcionarioRepository.FindByUserId(usuario.IdUsuario).IdFuncionario;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("F@CPgNl1HZ8nxb*&GgN5D&Gq*BiR@00757s9ylbtMo#!op%ZJe"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.FamilyName, usuario.Acesso),
                new Claim(ClaimTypes.Role, usuario.IdTipoUsuario.ToString()),
                new Claim("Role", usuario.IdTipoUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, usuario.IdUsuario.ToString())
            };

            var token = new JwtSecurityToken
                (
                    "GestaoFinancas",
                    "GestaoFinancas",
                    claims,
                    signingCredentials: credentials
                );

            return StatusCode(200, token);
        }

        //Metodos do administrador
        [HttpPost]
        public IActionResult CriarUsuario(Usuario usuario, Funcionario funcionario)
        {
            try
            {
                Usuario usuarioCadastrado = _usuarioRepository.Create(usuario);

                funcionario.IdUsuario = usuario.IdUsuario;

                _funcionarioRepository.Create(funcionario);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public IActionResult EditarUsuario(Usuario usuario, Funcionario funcionario)
        {
            try
            {
                if (_usuarioRepository.FindByUserId(usuario.IdUsuario) == null)
                {
                    return StatusCode(404, "Usuario nao encontrado");
                }
                if (_funcionarioRepository.FindByUserId(funcionario.IdFuncionario) == null)
                {
                    return StatusCode(404, "Funcionario nao encontrado");
                }

                _usuarioRepository.Update(usuario);

                _funcionarioRepository.Update(funcionario);

                return StatusCode(200, "Usuario atualizado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete]
        public IActionResult DeletarUsuario(int idUsuario)
        {
            try
            {
                Usuario usuario = _usuarioRepository.FindByUserId(idUsuario);
                if (usuario == null)
                {
                    return StatusCode(404, "Usuario nao encontrado");
                }

                _funcionarioRepository.Delete(usuario.Funcionario.IdFuncionario);

                _usuarioRepository.Delete(idUsuario);

                return StatusCode(200, "Usuario deletado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        //Metodos do Gestor
        [HttpPost("Gestor")]
        public IActionResult CadastrarUsuarioNoSetor(Usuario usuario, Funcionario funcionario)
        {
            Usuario usuarioCadastrado = _usuarioRepository.Create(usuario);

            funcionario.IdUsuario = usuarioCadastrado.IdUsuario;
            funcionario.IdSetor = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value);

            _funcionarioRepository.Create(funcionario);

            return StatusCode(201, "Funcionario criado");
        }

        [HttpPut("Gestor")]
        public IActionResult EditarUsuarioNoSetor(Usuario usuario, Funcionario funcionario)
        {
            try
            {
                if (_usuarioRepository.FindByUserId(usuario.IdUsuario) == null)
                {
                    return StatusCode(404, "Usuario nao encontrado");
                }
                if (_funcionarioRepository.FindByUserId(usuario.IdUsuario) == null)
                {
                    return StatusCode(404, "Funcionario nao encontrado");
                }

                _usuarioRepository.Update(usuario);

                _funcionarioRepository.Update(funcionario);

                return StatusCode(200, "Funcionario alterado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("Gestor")]
        public IActionResult DeletarUsuarioNoSetor(int idUsuario)
        {
            if (_usuarioRepository.FindByUserId(idUsuario) == null)
            {
                return StatusCode(404, "Usuario nao encontrado");
            }
            Funcionario funcionario = _funcionarioRepository.FindByUserId(idUsuario);
            if (funcionario == null)
            {
                return StatusCode(404, "Funcionario nao encontrado");
            }

            _funcionarioRepository.Delete(funcionario.IdFuncionario);

            _usuarioRepository.Delete(idUsuario);

            return StatusCode(200, "funcionario deletado");
        }
    }
}
