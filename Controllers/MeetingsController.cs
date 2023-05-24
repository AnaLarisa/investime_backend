using backend.Data.Repositories;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : Controller
    {
        private readonly IMeetingService _meetingService;

        public MeetingsController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpGet(Name ="GetAllMeetings")]
        public IActionResult GetAllMeetings()
        {
            var meetings = _meetingService.GetMeetings();
            return Ok(meetings);
        }

        [HttpPost(Name ="AddMeeting")]
        public IActionResult AddMeeting(Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                _meetingService.AddMeeting(meeting);
                return Ok($"Added meeting with id = {meeting.Id}");

            }

            // If the model is not valid, return the create view with the model to show validation errors
            return Ok(meeting);
        }

        [HttpDelete("{id}", Name = "RemoveMeeting")]
        public IActionResult RemoveMeeting(Guid id)
        {
            if (id != Guid.Empty)
            {
                _meetingService.DeleteMeeting(id);

                return Ok($"Deleted meeting with id = {id}");
            }

            // If the id is empty or null, return an error or redirect to an appropriate action
            return Ok("error");
        }
    }
}
