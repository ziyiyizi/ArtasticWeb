namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("comments")]
    public class comments
    {
        [Key]
        [Column(Order = 0, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Artwork_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(45)]
        public string User_Name { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(45)]
        public string Commentor_Name { get; set; }

        [StringLength(45)]
        public string Comment { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime Comment_time { get; set; }

        public comments()
        {

        }

        public comments(long id, string commentor, string commented, DateTime time, string comment)
        {
            Artwork_ID = id;
            Commentor_Name = commentor;
            User_Name = commented;
            Comment = comment;
            Comment_time = time;
        }
    }
}
