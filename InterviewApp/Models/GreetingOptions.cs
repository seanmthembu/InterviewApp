using System.ComponentModel.DataAnnotations;

namespace InterviewApp.Models;

public class GreetingOptions
{
    [Required(ErrorMessage = "'Message' is required in appsettings.json.")]
    public required string Message { get; set; }

    [Required(ErrorMessage = "'Language' is required in appsettings.json.")]
    public required string Language { get; set; }
}
