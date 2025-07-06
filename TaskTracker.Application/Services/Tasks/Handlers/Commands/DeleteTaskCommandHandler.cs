using MediatR;
using TaskTracker.Domain.Repositories;
using TaskTracker.Core.DTOs;
using TaskTracker.Core.Services;
using TaskTracker.Core.Exceptions;

namespace TaskTracker.Application.Services.Tasks.Handlers.Commands
{
    public class DeleteTaskCommand : IRequest<Response<NoContent>>
    {
        public int Id { get; set; }
    }

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Response<NoContent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;

        public DeleteTaskCommandHandler(
            IUnitOfWork unitOfWork, 
            IUserContextService userContextService)
        { 
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }

        public async Task<Response<NoContent>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            //await _unitOfWork.BeginTransactionAsync(cancellationToken);
            
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                var task = await _unitOfWork.TaskRepository.GetByIdAndUserIdAsync(request.Id, currentUserId, cancellationToken);
                if (task == null) 
                {
                    throw new NotFoundException("Görev bulunamadı");
                }

                await _unitOfWork.TaskRepository.DeleteAsync(task, currentUserId, cancellationToken);
                
                //await _unitOfWork.CommitTransactionAsync(cancellationToken);
                
                return Response<NoContent>.Success(204);
            }
            catch
            {
                //await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
} 