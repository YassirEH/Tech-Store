using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;

namespace webApi.Controllers
{
    public class APIController : ControllerBase
    {
        //protected readonly IMapper _mapper;
        protected readonly INotificationService _notificationService;

        public APIController(/*IMapper mapper,*/ INotificationService notificationService)
        {
            //_mapper = mapper;
            _notificationService = notificationService;
        }


        protected new IActionResult Response(object? obj)
        {
            if(obj == null) return NotFound("System could not found any related data....");

            if (!_notificationService.HasAnyNotification()) return Ok(new { data = obj });

            var messageTypes = new List<string>()
            {
                ErrorType.Success.ToString(),
                ErrorType.Info.ToString()
            };

            if (HasError())
            {
                var errors = _notificationService.GetAllNotifications.Where(x => !messageTypes.Contains(x.Type))
                                                                     .Select(x => new { x.StatusCode, x.Message });

                return StatusCode(0, errors);
            }

            return Ok(new { data = obj, notifications = _notificationService.GetAllNotifications.Where(x => messageTypes.Contains(x.Type)) });
        }

        private bool HasError() => _notificationService.GetAllNotifications.Exists(_ => _.Type == ErrorType.Error.ToString());
    }
}
