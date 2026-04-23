using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laba10_1
{
    [Table("researchers")]
    public class Researcher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Column("first_name")]
        public string FirstName { get; set; } 
        
        [Column("last_name")]
        public string LastName { get; set; } 
        
        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }
        
        [Column("email")]
        public string Email { get; set; } 
        
        [Column("password")]
        public string Password { get; set; } 
        
        [Column("phone_number")]
        public string PhoneNumber { get; set; } 
        
        [Column("research_field")]
        public string ResearchField { get; set; } 
        
        [Column("first_publication_date")]
        public DateTime FirstPublicationDate { get; set; }
        
        [Column("role")]
        public string Role { get; set; } = "User";
    }
    
    [Table("password_reset_codes")]
    public class PasswordResetCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Column("email")]
        public string Email { get; set; }
        
        [Column("code")]
        public string Code { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Column("is_used")]
        public bool IsUsed { get; set; } = false;
    }
}
