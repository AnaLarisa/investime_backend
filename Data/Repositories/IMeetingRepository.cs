﻿using backend.Models;

namespace backend.Data.Repositories;

public interface IMeetingRepository
{
    IEnumerable<Meeting> GetMeetings();
    Meeting GetMeetingById(string id);
    Task AddMeeting(Meeting meeting);
    void UpdateMeeting(Meeting meeting);
    void DeleteMeeting(string id);
}