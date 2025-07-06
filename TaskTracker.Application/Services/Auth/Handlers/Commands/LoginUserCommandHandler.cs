using MediatR;
using TaskTracker.Domain.Repositories;
using TaskTracker.Core.DTOs;
using TaskTracker.Application.Helpers;
using TaskTracker.Application.Services.Auth.DTOs.Response;

namespace TaskTracker.Application.Services.Auth.Handlers.Commands
{
    public class LoginUserCommand : IRequest<Response<LoginResponse>>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<LoginResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;

        public LoginUserCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        public async Task<Response<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            //await _unitOfWork.BeginTransactionAsync(cancellationToken);
            
            try
            {
                var user = await _unitOfWork.UserRepository.GetByUsernameAsync(request.Username, cancellationToken);
                if (user == null)
                {
                    return Response<LoginResponse>.Fail("Kullanıcı bulunamadı.", 400);
                }

                if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return Response<LoginResponse>.Fail("Şifre hatalı.", 400);
                }

                var token = _jwtService.GenerateToken(user);

                var response = new LoginResponse
                {
                    Token = token,
                    Username = user.Username
                };

                //await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return Response<LoginResponse>.Success(response, 200);
            }
            catch
            {
                //await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
} 