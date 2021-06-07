namespace LoginShmogin.Application.DTOs
{
    public class UserDTO
    {
        public UserDTO(string id, string username, string email, bool emailConfirmed)
        {
            Id = id;
            UserName = username;
            Email = email;
            EmailConfirmed = emailConfirmed;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}