using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet5.GoodPracticies.DTO.Data
{
    public class UserDBModel
    {
        public UserDBModel()
        {

        }

        public UserDBModel(int userId)
        {
            UserId = userId;
        }
        
        [Required]
        public int UserId { get; private set; }

        [Required]
        [MaxLength(40)]
        [Column(TypeName = "varchar(40)")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(80)]
        [Column(TypeName = "varchar(80)")]
        public string LastName { get; set; }

        [Required]
        public uint Age { get; set; }

        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string UserType { get; set; }
    }
}