using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class CAFCASSInvolvedID
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}