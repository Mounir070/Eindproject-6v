using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eindproject_6v.Pages;

public class ExploreModel : PageModel
{
    private readonly ILogger<ExploreModel> _logger;

    public ExploreModel(ILogger<ExploreModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}
