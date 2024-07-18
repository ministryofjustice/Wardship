using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class CAFCASSInvolved
    {
        [Key]
        public int CAFCASSInvolvedID { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}