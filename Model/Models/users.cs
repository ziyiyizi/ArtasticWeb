namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("users")]
    public class users
    {
        [Key]
        [Column(TypeName = "uint")]
        public long User_ID { get; set; }

        [Required]
        [StringLength(45)]
        public string User_name { get; set; }

        [Required]
        [StringLength(45)]
        public string User_password { get; set; }

        [Required]
        [StringLength(10)]
        public string User_sex { get; set; }

        [StringLength(10)]
        public string User_age { get; set; }

        [StringLength(100)]
        public string User_description { get; set; }

        [StringLength(45)]
        public string User_software { get; set; }

        [StringLength(45)]
        public string User_job { get; set; }

        [StringLength(45)]
        public string User_address { get; set; }

        [Required]
        [StringLength(45)]
        public string User_mail { get; set; }

        [StringLength(15)]
        public string User_phone { get; set; }

        public DateTime Registertime { get; set; }

        [StringLength(250)]
        public string User_icon { get; set; }

        [Required]
        [StringLength(15)]
        public string User_state { get; set; }

        [StringLength(45)]
        public string User_token { get; set; }

        public DateTime? token_time { get; set; }

        [StringLength(45)]
        public string User_salt { get; set; }
    }
}
