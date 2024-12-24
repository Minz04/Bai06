namespace WindowsFormsApp2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SinhVien")]
    public partial class SinhVien
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string StudentID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string FullName { get; set; }

        [Key]
        [Column(Order = 2)]
        public double AverageScore { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FacultyID { get; set; }
    }
}
