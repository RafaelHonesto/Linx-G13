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

    /// <summary>
    /// Controller responsável pelos endpoints (URLs) referentes aos usuários
    /// </summary>

    // Define que a rota de uma requisição será no formato dominio/api/nomeController
    // ex: http://localhost:5000/api/usuarios
    [Route("api/[controller]")]

    // Define que é um controlador de API
    [ApiController]

    // Define que o tipo de resposta da API será no formato JSON
    [Produces("application/json")]

    // Define que somente o administrador pode acessar os métodos
    public class UsuarioController : ControllerBase
    {
        /// <summary>
        /// Objeto _usuarioRepository que irá receber todos os métodos definidos na interface IuUsuarioRepository
        /// </summary>
        private IUsuarioRepository _usuarioRepository { get; set; }

        /// <summary>
        /// Objeto _funcionarioRepository que irá receber todos os métodos definidos na interface IFuncionarioRepository
        /// </summary>
        private IFuncionarioRepository _funcionarioRepository { get; set; }

        /// <summary>
        /// Instancia o objeto _usuarioRepository e _funcionarioRepository para que haja a referência aos métodos no repositório
        /// </summary>
        public UsuarioController()
        {
            _usuarioRepository = new UsuarioRepository();
            _funcionarioRepository = new FuncionarioRepository();
        }


        /// <summary>
        /// Login 
        /// </summary>
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


        /// <summary>
        /// Adm cadastra um novo gestor
        /// </summary>
        /// <param name="usuarioFuncionario">Objeto usuarioFuncionario que será cadastrado</param>
        /// <returns>Um status code 201 - Created</returns>
        // Define que somente o administrador pode acessar o método
        [Authorize(Roles = "1")]
        [HttpPost("Criar"), DisableRequestSizeLimit]
        public IActionResult CriarUsuario(UsuarioFuncionario usuarioFuncionario)
        {
            try
            {
                Usuario usuario = usuarioFuncionario.usuario;

                usuario.IdTipoUsuario = 2;

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


        /// <summary>
        /// Adm Atualiza um usuário existente
        /// </summary>
        /// <param name="usuarioFuncionario">Objeto com as novas informações</param>
        /// <returns>Um status code 204 - No Content</returns>
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


        /// <summary>
        /// Adm deleta um usuário existente
        /// </summary>
        /// <param name="idUsuario">ID do usuário que será deletado</param>
        /// <returns>Um status code 204 - No Content</returns>
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


        /// <summary>
        /// Gestor cadastra um novo usuário
        /// </summary>
        /// <param name="usuarioFuncionario">Objeto usuarioFuncionario que será cadastrado</param>
        /// <returns>Um status code 201 - Created</returns>
        [Authorize(Roles = "2")]
        [HttpPost("Gestor")]
        public IActionResult CadastrarUsuarioNoSetor(UsuarioFuncionario usuarioFuncionario)
        {
            Usuario usuario = usuarioFuncionario.usuario;

            usuario.IdTipoUsuario = 3;

            Funcionario funcionario = usuarioFuncionario.funcionario;
            Usuario usuarioCadastrado = _usuarioRepository.Create(usuario);

            funcionario.IdUsuario = usuarioCadastrado.IdUsuario;
            Funcionario funcionarioCadastrado = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));
            funcionario.IdSetor = funcionarioCadastrado.IdSetor;

            _funcionarioRepository.Create(funcionario);

            return StatusCode(201, "Funcionario criado");
        }


        /// <summary>
        /// Gestor atualiza um usuário existente
        /// </summary>
        /// <param name="id">ID do usuário que será atualizado</param>
        /// <param name="usuarioFuncionario">Objeto com as novas informações</param>
        /// <returns>Um status code 204 - No Content</returns>
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
        [HttpGet("Setor")]
        public IActionResult ListarUsuarioIdSetor()
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            List<Funcionario> Funcionarios = _funcionarioRepository.ReadBySetorId(funcionario.IdSetor);

            return StatusCode(200, Funcionarios);
        }


        /// <summary>
        /// Gestor deleta um usuário existente
        /// </summary>
        /// <param name="idUsuario">ID do usuário que será deletado</param>
        /// <returns>Um status code 204 - No Content</returns>
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

        [Authorize(Roles = "2")]
        [HttpGet("Buscar")]
        public IActionResult BuscarPorId(int idUsuario)
        {
            Funcionario funcionario = _funcionarioRepository.FindByUserId(Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

            return StatusCode(200, funcionario);
        }

        [Authorize(Roles = "3")]
        [HttpGet]
        public IActionResult ListarUsuario ()
        {
            List <Usuario> usuarios = _usuarioRepository.Read();

            return Ok(usuarios);
        }
    }
}
