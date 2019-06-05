namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("notification")]
    public class notification
    {
        [Key]
        [Column(Order = 0, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Noti_ID { get; set; }

        [StringLength(45)]
        public string Sender_Name { get; set; }

        [StringLength(45)]
        public string Receiver_name { get; set; }

        public DateTime Noti_Time { get; set; }

        [Required]
        [StringLength(5)]
        public string Noti_State { get; set; }

        [StringLength(45)]
        public string Noti_Content { get; set; }

        [Required]
        [StringLength(15)]
        public string type { get; set; }

        [StringLength(45)]
        public string Work_Name { get; set; }

        public long Work_ID { get; set; }

        public notification()
        {

        }

        public notification(string sender, string receiver, DateTime time, string state, string type, string content = "", string workName = "", long workId = 0)
        {
            Sender_Name = sender;
            Receiver_name = receiver;
            Noti_Time = time;
            Noti_State = state;
            this.type = type;
            Noti_Content = content;
            Work_Name = workName;
            Work_ID = workId;
        }
    }
}
