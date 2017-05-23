using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wardship.Models
{
    public class Role
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int strength { get; set; }
        [Required, MaxLength(20)]
        public string Detail { get; set; }
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required,MaxLength(150), Display(Name="Login Name")]
        public string Name { get; set; }
        [MaxLength(30), Display(Name="Display Name")]
        public string DisplayName { get; set; }
        public DateTime? LastActive { get; set; }
        [Required, Display(Name = "Role")]
        public int RoleStrength { get; set; }
        public virtual Role Role { get; set; }
    }
    public class ADGroup
    {
        [Key]
        public int ADGroupID { get; set; }
        [Required, MaxLength(80)]
        public string Name { get; set; }
        [Required]
        public int RoleStrength { get; set; }
        public virtual Role Role { get; set; }
    }

    public class GroupList
    {
        public string Name { get; set; }
        public List<GroupMember> Members { get; set; }
        public string ErrorMessage { get; set; }
        public GroupList()
        {
            Members = new List<GroupMember>();
        }
    }

    public class GroupMember
    {
        public string Name { get; set; }
        public string ErrorMessage { get; set; }
        public GroupMember(string name)
        {
            Name = name;
        }
    }
}