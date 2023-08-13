using ThirdParty.Json.LitJson;

namespace InvesTime.BackEnd.Models;

public class NewsResponseModel
{
    public List<NewsModel> Articles { get; set; }
}