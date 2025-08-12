using RazorEmails.Rendering;
using RazorEmails.Rendering.ViewModels;
using RazorEmails.Rendering.Views;

namespace RazorEmails.Mailers;

public class MessageMailer
{
    private readonly Mailer _mailer;
    private readonly RazorViewRenderer _razorViewRenderer;

    public MessageMailer(Mailer mailer, RazorViewRenderer razorViewRenderer)
    {
        _mailer = mailer;
        _razorViewRenderer = razorViewRenderer;
    }

    public async Task SendAsync(string to, string toName, string greeting, string message)
    {
        string body = await _razorViewRenderer.Render<MessageEmail, MessageEmailViewModel>(
            new MessageEmailViewModel() { Greeting = greeting, Message = message }
        );

        await _mailer.SendMailAsync(new()
        {
            To = to,
            ToName = toName,
            Subject = $"{greeting}, we have a message for you.",
            Body = body
        });
    }
}
