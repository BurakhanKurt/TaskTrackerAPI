using MediatR;
using TaskTracker.Domain.Repositories;
using TaskTracker.Core.DTOs;
using TaskTracker.Core.Services;
using TaskTracker.Core.Exceptions;

namespace TaskTracker.Application.Services.Tasks.Handlers.Commands
{
    public class UpdateTaskTitleCommand : IRequest<Response<NoContent>>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
    }
    public class UpdateTaskTitleCommandHandler(
        IUnitOfWork _unitOfWork, 
        IUserContextService _userContextService)
        : IRequestHandler<UpdateTaskTitleCommand, Response<NoContent>>
    {
        public async Task<Response<NoContent>> Handle(UpdateTaskTitleCommand request, CancellationToken cancellationToken)
        {
            //await _unitOfWork.BeginTransactionAsync(cancellationToken);
            
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                var task = await _unitOfWork.TaskRepository.GetByIdAndUserIdAsync(request.Id, currentUserId, cancellationToken);

                if (task == null)
                    throw new NotFoundException("Görev bulunamadı");

                task.SetTitle(request.Title ?? string.Empty);

                await _unitOfWork.TaskRepository.UpdateAsync(task, cancellationToken);

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