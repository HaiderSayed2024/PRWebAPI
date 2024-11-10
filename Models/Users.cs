using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PRWebAPI.Models
{
    public class Users
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Boolean EmailConfirmed { get; set; } = false;

        [JsonIgnore]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public string UserType { get; set; }
        public int IsActive { get; set; } = 1;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
}
