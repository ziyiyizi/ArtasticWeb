namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("likes")]
    public partial class likes
    {
        [Key]
        [Column(Order = 0, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long User_ID { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Artwork_ID { get; set; }

        public DateTime liketime { get; set; }

        public likes()
        {

        }

        public likes(long userId, long workId, DateTime time)
        {
            User_ID = userId;
            Artwork_ID = workId;
            liketime = time;
        }
    }
}
