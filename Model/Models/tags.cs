namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tags")]
    public class tags
    {
        [Key]
        [Column(Order = 0, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Artwork_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(45)]
        public string Tag_name { get; set; }

        public tags()
        {

        }

        public tags(long workId, string tag)
        {
            Artwork_ID = workId;
            Tag_name = tag;
        }
    }
}
