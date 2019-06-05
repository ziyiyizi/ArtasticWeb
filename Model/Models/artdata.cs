namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("artdata")]
    public class artdata
    {
        [Key]
        [Column(TypeName = "uint")]
        public long Artdata_ID { get; set; }

        [Column(TypeName = "uint")]
        public long Artwork_ID { get; set; }

        [Column("Artdata")]
        [Required]
        [StringLength(250)]
        public string Artdata1 { get; set; }

        public artdata()
        {

        }

        public artdata(long dataId, long workId, string data)
        {
            Artdata_ID = dataId;
            Artwork_ID = workId;
            Artdata1 = data;
        }
    }
}
