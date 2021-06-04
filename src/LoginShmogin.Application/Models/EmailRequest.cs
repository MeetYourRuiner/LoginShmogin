namespace LoginShmogin.Application.Models
{
    public class EmailRequest
    {
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }

        public EmailRequest(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}
