namespace GreenMaterialBackEnd.Models.User
{
    public class UserResponse
    {
        public int id { get; set; }
        public string? lastName { get; set; }    
        public string? firstName { get; set; }   
        public string? email { get; set; }

        public UserResponse() { id = 0; }
    }
}
