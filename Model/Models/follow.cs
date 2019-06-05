namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("follow")]
    public class follow
    {
        [Key]
        [Column(Order = 0, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Follower_ID { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Artist_ID { get; set; }

        public DateTime followtime { get; set; }

        public follow()
        {

        }

        public follow(long followerId, long artistId, DateTime time)
        {
            Follower_ID = followerId;
            Artist_ID = artistId;
            followtime = time;
        }
    }
}
