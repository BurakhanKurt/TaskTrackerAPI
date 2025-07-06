using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Core.DTOs;


namespace TaskTracker.Core.ControllerBases
{
    public class CustomControllerBase : ControllerBase
    {
        /// <summary>
        /// Respone modele uygun IActionResult örneği oluşturur.
        /// </summary>
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            if (response.StatusCode == 204)
                return NoContent();
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
