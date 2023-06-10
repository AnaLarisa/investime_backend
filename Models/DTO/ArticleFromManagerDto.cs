using System.ComponentModel.DataAnnotations;

namespace InvesTime.BackEnd.Models.DTO;

public class ArticleFromManagerDto
{
    [Required] public string Title { get; set; } = "New article";

    [Required]
    [Url]
    public string Content { get; set; } = string.Empty;

    public string Observations { get; set; } = string.Empty;
}