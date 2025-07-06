using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskTracker.Application.Common;
using TaskTracker.Application.Services.Tasks.DTOs.Request;
using TaskTracker.Application.Services.Tasks.Handlers.Commands;
using TaskTracker.Application.Services.Tasks.Handlers.Queries;
using TaskTracker.Core.ControllerBases;

namespace TaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController(IMediator _mediator) : CustomControllerBase
    {
        /// <summary>
        /// Tüm görevleri sayfalı olarak getirir
        /// </summary>
        /// <param name="page">Sayfa numarası (varsayılan: 1)</param>
        /// <param name="pageSize">Sayfa başına görev sayısı (varsayılan: 10)</param>
        /// <param name="statusFilter">Durum filtresi (0: Tümü, 1: Tamamlanan, 2: Bekleyen)</param>
        /// <param name="searchTerm">Başlık arama terimi</param>
        /// <param name="startDate">Başlangıç tarihi (YYYY-MM-DD)</param>
        /// <param name="endDate">Bitiş tarihi (YYYY-MM-DD)</param>
        /// <returns>Görev listesi ve toplam sayı</returns>
        /// <response code="200">Görevler başarıyla getirildi</response>
        /// <response code="401">Yetkilendirme gerekli</response>
        /// <response code="429">Rate limit aşıldı</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] TaskStatusFilter? statusFilter = null,
            [FromQuery] string? searchTerm = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var query = new GetTasksQuery
            {
                Page = page,
                PageSize = pageSize,
                StatusFilter = statusFilter,
                SearchTerm = searchTerm,
                StartDate = startDate,
                EndDate = endDate
            };
            
            var result = await _mediator.Send(query);
            return CreateActionResultInstance(result);
        }

        /// <summary>
        /// Yeni bir görev oluşturur
        /// </summary>
        /// <param name="command">Görev bilgileri (Title)</param>
        /// <returns>Oluşturulan görevin ID'si</returns>
        /// <response code="201">Görev başarıyla oluşturuldu</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Yetkilendirme gerekli</response>
        /// <response code="429">Rate limit aşıldı</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }

        /// <summary>
        /// Görevin başlığını günceller
        /// </summary>
        /// <param name="id">Görev ID'si</param>
        /// <param name="request">Güncellenecek başlık</param>
        /// <returns>Güncelleme başarılı</returns>
        /// <response code="204">Görev başarıyla güncellendi</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Yetkilendirme gerekli</response>
        /// <response code="404">Görev bulunamadı</response>
        /// <response code="429">Rate limit aşıldı</response>
        [HttpPut("{id}/title")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> UpdateTitle(int id, [FromBody] UpdateTaskTitleRequest request)
        {
            var command = new UpdateTaskTitleCommand
            {
                Id = id,
                Title = request.Title
            };
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }

        /// <summary>
        /// Görevin durumunu günceller (tamamlandı/tamamlanmadı)
        /// </summary>
        /// <param name="id">Görev ID'si</param>
        /// <param name="isCompleted">Görevin tamamlanma durumu (true/false)</param>
        /// <returns>İşlem başarılı</returns>
        /// <response code="204">Görev durumu başarıyla güncellendi</response>
        /// <response code="401">Yetkilendirme gerekli</response>
        /// <response code="404">Görev bulunamadı</response>
        /// <response code="429">Rate limit aşıldı</response>
        [HttpPut("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTaskStatusRequest request)
        {
            var result = await _mediator.Send(new UpdateTaskStatusCommand
            {
                Id = id,
                IsCompleted = request.IsCompleted
            });

            return CreateActionResultInstance(result);
        }

        /// <summary>
        /// Görevin bitiş tarihini günceller
        /// </summary>
        /// <param name="id">Görev ID'si</param>
        /// <param name="request">Güncellenecek tarih</param>
        /// <returns>Güncelleme başarılı</returns>
        /// <response code="204">Görev tarihi başarıyla güncellendi</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Yetkilendirme gerekli</response>
        /// <response code="404">Görev bulunamadı</response>
        /// <response code="429">Rate limit aşıldı</response>
        [HttpPut("{id}/date")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> UpdateDate(int id, [FromBody] UpdateTaskDateRequest request)
        {
            var command = new UpdateTaskDateCommand
            {
                Id = id,
                DueDate = request.DueDate
            };
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }

        /// <summary>
        /// Görevi siler
        /// </summary>
        /// <param name="id">Görev ID'si</param>
        /// <returns>Silme işlemi başarılı</returns>
        /// <response code="204">Görev başarıyla silindi</response>
        /// <response code="401">Yetkilendirme gerekli</response>
        /// <response code="404">Görev bulunamadı</response>
        /// <response code="429">Rate limit aşıldı</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _mediator.Send(new DeleteTaskCommand { Id = id });
            return CreateActionResultInstance(result);
        }
    }
}