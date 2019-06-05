namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("clicks")]
    public class clicks
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(45)]
        public string Click_ID { get; set; }

        [Column(Order = 1, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long User_ID { get; set; }

        [Column(Order = 2, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Artwork_ID { get; set; }

        [Column(Order = 3)]
        public DateTime Clicktime { get; set; }

        public clicks()
        {

        }

        public clicks(string id, long userId, long workId, DateTime time)
        {
            Click_ID = id;
            User_ID = userId;
            Artwork_ID = workId;
            Clicktime = time;
        }
    }
}
