using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eindproject_6v.Pages;

public class NotFoundModel : PageModel
{
    private readonly ILogger<NotFoundModel> _logger;

    public NotFoundModel(ILogger<NotFoundModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}
