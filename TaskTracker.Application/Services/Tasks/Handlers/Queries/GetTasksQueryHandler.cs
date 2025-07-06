using MediatR;
using TaskTracker.Application.Services.Tasks.DTOs.Request;
using TaskTracker.Application.Services.Tasks.DTOs.Response;
using TaskTracker.Core.DTOs;
using TaskTracker.Core.Exceptions;
using TaskTracker.Core.Services;
using TaskTracker.Domain.Repositories;

namespace TaskTracker.Application.Services.Tasks.Handlers.Queries
{
    public class GetTasksQuery : IRequest<Response<GetTasksResponse>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public TaskStatusFilter? StatusFilter { get; set; }
        public string? SearchTerm { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, Response<GetTasksResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;

        public GetTasksQueryHandler(IUnitOfWork unitOfWork, IUserContextService userContextService)
        { 
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }

        public async Task<Response<GetTasksResponse>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _userContextService.GetCurrentUserId();

            // Filtreleme parametreleri
            bool? isCompleted = null;
            if (request.StatusFilter.HasValue)
            {
                switch (request.StatusFilter.Value)
                {
                    case TaskStatusFilter.Completed:
                        isCompleted = true;
                        break;
                    case TaskStatusFilter.Pending:
                        isCompleted = false;
                        break;
                    case TaskStatusFilter.All:
                    default:
                        isCompleted = null;
                        break;
                }
            }

            // Filtrelenmiş görevler (pagination' a göre)
            var tasks = await _unitOfWork.TaskRepository.GetFilteredTasksAsync(
                currentUserId, 
                request.Page, 
                request.PageSize, 
                isCompleted,
                request.SearchTerm,
                request.StartDate,
                request.EndDate,
                cancellationToken);

            // Filtrelenmiş toplam sayıyı al (tüm)
            var filteredTotalCount = await _unitOfWork.TaskRepository.GetFilteredTasksCountAsync(
                currentUserId,
                isCompleted,
                request.SearchTerm,
                request.StartDate,
                request.EndDate,
                cancellationToken);

            // Genel istatistikler (filtreleme olmadan)
            var totalCount = await _unitOfWork.TaskRepository.GetTotalCountByUserIdAsync(currentUserId, cancellationToken);
            var completedCount = await _unitOfWork.TaskRepository.GetCompletedCountByUserIdAsync(currentUserId, cancellationToken);
            var pendingCount = await _unitOfWork.TaskRepository.GetPendingCountByUserIdAsync(currentUserId, cancellationToken);
            
            var totalPages = (int)Math.Ceiling((double)filteredTotalCount / request.PageSize);
            
            // Progress hesapla (genel progress, filtreleme olmadan)
            var progress = totalCount > 0 ? (int)Math.Round((double)completedCount / totalCount * 100) : 0;
            
            var taskDtos = tasks.Select(x => new TaskItemDto
            {
                Id = x.Id,
                Title = x.Title,
                IsCompleted = x.IsCompleted,
                CreatedAt = x.CreatedAt,
                CompletedAt = x.CompletedAt,
                DueDate = x.DueDate
            });
            
            var response = new GetTasksResponse
            {
                Tasks = taskDtos,
                TotalCount = filteredTotalCount, // Filtrelenmiş toplam sayı
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                TotalTasks = totalCount, // Genel toplam
                Completed = completedCount, // Genel tamamlanan
                Pending = pendingCount, // Genel bekleyen
                Progress = progress
            };
            return Response<GetTasksResponse>.Success(response, 200);
        }
    }
} 