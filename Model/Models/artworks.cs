namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("artworks")]
    public class artworks
    {
        [Key]
        [Column(TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Artwork_ID { get; set; }

        [Column(TypeName = "uint")]
        public long Artist_ID { get; set; }

        [Required]
        [StringLength(45)]
        public string Artwork_name { get; set; }

        [StringLength(45)]
        public string Artwork_description { get; set; }

        [StringLength(45)]
        public string Artwork_dir { get; set; }

        [Column("Artdata")]
        [Required]
        [StringLength(250)]
        public string Artdata1 { get; set; }

        public DateTime Uploadtime { get; set; }

        public artworks()
        {

        }

        public artworks(long userId, string name, string data, DateTime time, string description = "", string dir = "")
        {
            Artist_ID = userId;
            Artwork_name = name;
            Artwork_description = description;
            Artwork_dir = dir;
            Artdata1 = data;
            Uploadtime = time;
        }

    }
}
