using MediatR;
using Microsoft.Extensions.Logging;
using TaskTracker.Core.DTOs;
using TaskTracker.Core.Exceptions;
using TaskTracker.Core.Extensions;
using TaskTracker.Core.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Repositories;

namespace TaskTracker.Application.Services.Tasks.Handlers.Commands
{
    public class CreateTaskCommand : IRequest<Response<int>>
    {
        public string? Title { get; set; }
        public DateOnly? DueDate { get; set; }
    }
    public class CreateTaskCommandHandler(
        IUnitOfWork _unitOfWork, 
        IUserContextService _userContextService)
        : IRequestHandler<CreateTaskCommand, Response<int>>
    {
        public async Task<Response<int>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            //await _unitOfWork.BeginTransactionAsync(cancellationToken);
            
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                var task = new TaskItem
                {
                    IsCompleted = false,
                    UserId = currentUserId,
                    DueDate = request.DueDate.HasValue ? request.DueDate.Value.ToDateTime(new TimeOnly(0, 0)) : null,
                };
                task.SetTitle(request.Title ?? string.Empty);
                var result = await _unitOfWork.TaskRepository.AddAsync(task, cancellationToken);

                //await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return Response<int>.Success(result.Id, 201);
            }
            catch
            {
                //await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}