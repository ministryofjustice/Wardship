using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class FAQ
    {
        [Key]
        public int faqID { get; set; }
        [Required]
        public bool loggedInUser { get; set; }
        [Required, MaxLength(150)]
        public string question { get; set; }
        [Required, MaxLength(4000)]
        [DataType(DataType.MultilineText)]
        public string answer { get; set; }


    }

}