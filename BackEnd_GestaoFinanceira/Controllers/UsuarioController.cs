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
                usuario.Funcionario == null ?
                    new Claim("IdUsuario", "Usuario nao e funcionario") :
                    new Claim("IdUsuario", idFuncionario.ToString()),
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
    }
}
