using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskTracker.Application.Services.Auth.Handlers.Commands;
using TaskTracker.Core.ControllerBases;

namespace TaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator _mediator) : CustomControllerBase
    {

        /// <summary>
        /// Yeni kullanıcı kaydı oluşturur
        /// </summary>
        /// <param name="command">Kullanıcı bilgileri (Username, Email, Password)</param>
        /// <returns>Kayıt işlemi başarılı</returns>
        /// <response code="200">Kullanıcı başarıyla kaydedildi</response>
        /// <response code="400">Geçersiz veri veya kullanıcı adı/email zaten mevcut</response>
        /// <response code="429">Rate limit aşıldı</response>
        [HttpPost("register")]
        [EnableRateLimiting("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }

        /// <summary>
        /// Kullanıcı girişi yapar ve JWT token döner
        /// </summary>
        /// <param name="command">Giriş bilgileri (Email, Password)</param>
        /// <returns>JWT token</returns>
        /// <response code="200">Giriş başarılı, token döner</response>
        /// <response code="400">Geçersiz email veya şifre</response>
        /// <response code="429">Rate limit aşıldı</response>
        [HttpPost("login")]
        [EnableRateLimiting("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}