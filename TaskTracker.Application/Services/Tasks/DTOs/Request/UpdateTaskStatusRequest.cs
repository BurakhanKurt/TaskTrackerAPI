using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Application.Services.Tasks.DTOs.Request
{
    public class UpdateTaskStatusRequest
    {
        public bool IsCompleted { get; set; }
    }
}
