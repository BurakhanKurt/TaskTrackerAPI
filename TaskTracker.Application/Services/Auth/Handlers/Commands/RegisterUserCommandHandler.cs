using MediatR;
using Microsoft.Extensions.Logging;
using TaskTracker.Core.DTOs;
using TaskTracker.Core.Exceptions;
using TaskTracker.Application.Helpers;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Repositories;

namespace TaskTracker.Application.Services.Auth.Handlers.Commands
{
    public class RegisterUserCommand : IRequest<Response<int>>
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        // private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            IJwtService jwtService
            //, ILogger<RegisterUserCommandHandler> logger
        )
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _jwtService = jwtService;
            // _logger = logger;
        }

        public async Task<Response<int>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var exists = await _unitOfWork.UserRepository.ExistsByEmailAsync(request.Email, cancellationToken);
            if (exists)
                throw new BadRequestException("Bu email adresi zaten kullanılıyor");

            var (passwordHash, passwordSalt) = _passwordService.HashPassword(request.Password);
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _unitOfWork.UserRepository.CreateAsync(user, cancellationToken);

            return Response<int>.Success(user.Id, 201);
        }
    }
} 