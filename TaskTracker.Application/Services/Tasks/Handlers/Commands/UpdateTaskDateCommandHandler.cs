using MediatR;
using TaskTracker.Domain.Repositories;
using TaskTracker.Core.DTOs;
using TaskTracker.Core.Services;
using TaskTracker.Core.Exceptions;

namespace TaskTracker.Application.Services.Tasks.Handlers.Commands
{
    public class UpdateTaskDateCommand : IRequest<Response<NoContent>>
    {
        public int Id { get; set; }
        public DateTime? DueDate { get; set; }
    }
    
    public class UpdateTaskDateCommandHandler(
        IUnitOfWork _unitOfWork, 
        IUserContextService _userContextService)
        : IRequestHandler<UpdateTaskDateCommand, Response<NoContent>>
    {
        public async Task<Response<NoContent>> Handle(UpdateTaskDateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                var task = await _unitOfWork.TaskRepository.GetByIdAndUserIdAsync(request.Id, currentUserId, cancellationToken);

                if (task == null)
                    throw new NotFoundException("Görev bulunamadı");

                task.SetDueDate(request.DueDate);

                await _unitOfWork.TaskRepository.UpdateAsync(task, cancellationToken);

                return Response<NoContent>.Success(204);
            }
            catch
            {
                throw;
            }
        }
    }
} 