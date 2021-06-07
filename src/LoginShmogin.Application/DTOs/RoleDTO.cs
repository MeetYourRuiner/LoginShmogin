namespace LoginShmogin.Application.DTOs
{
    public class RoleDTO
    {
        public RoleDTO(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}