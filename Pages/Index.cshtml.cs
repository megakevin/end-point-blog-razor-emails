using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorEmails.Mailers;

namespace RazorEmails.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly MessageMailer _messageMailer;

    public IndexModel(ILogger<IndexModel> logger, MessageMailer messageMailer)
    {
        _logger = logger;
        _messageMailer = messageMailer;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogInformation("Sending email.");

        await _messageMailer.SendAsync(
            "recipient@example.com",
            "Recipient Name",
            "Mr. Recipient",
            "Have a good day."
        );

        _logger.LogInformation("Email sent successfully.");

        return RedirectToPage("Index");
    }
}
