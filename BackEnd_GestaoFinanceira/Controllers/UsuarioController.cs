using BackEnd_GestaoFinanceira.Domains;
using BackEnd_GestaoFinanceira.Interfaces;
using BackEnd_GestaoFinanceira.Model;
using BackEnd_GestaoFinanceira.Repositories;
using Microsoft.AspNetCore.Authorization;
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

            return StatusCode(200, new
            { 
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        //Metodos do administrador
        [Authorize(Roles = "1")]
        [HttpPost("Criar")]
        public IActionResult CriarUsuario(UsuarioFuncionario usuarioFuncionario)
        {
            try
            {
                Usuario usuario = usuarioFuncionario.usuario;

                Funcionario funcionario = usuarioFuncionario.funcionario;

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

        [Authorize(Roles = "1")]
        [HttpPut]
        public IActionResult EditarUsuario(UsuarioFuncionario usuarioFuncionario)
        {
            try
            {
                Usuario usuario = usuarioFuncionario.usuario;

                Funcionario funcionario = usuarioFuncionario.funcionario;

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

        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "2")]
        [HttpPost("Gestor")]
        public IActionResult CadastrarUsuarioNoSetor(UsuarioFuncionario usuarioFuncionario)
        {
            Usuario usuario = usuarioFuncionario.usuario;

            Funcionario funcionario = usuarioFuncionario.funcionario;
            Usuario usuarioCadastrado = _usuarioRepository.Create(usuario);

            funcionario.IdUsuario = usuarioCadastrado.IdUsuario;
            funcionario.IdSetor = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value);

            _funcionarioRepository.Create(funcionario);

            return StatusCode(201, "Funcionario criado");
        }

        [Authorize(Roles = "2")]
        [HttpPut("Gestor")]
        public IActionResult EditarUsuarioNoSetor(UsuarioFuncionario usuarioFuncionario)
        {
            try
            {
                Usuario usuario = usuarioFuncionario.usuario;

                Funcionario funcionario = usuarioFuncionario.funcionario;

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

        [Authorize(Roles = "2")]
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
