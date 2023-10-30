﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace thimau.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Sinhvien
    {
        [Display(Name ="Mã sinh viên")]
        [Required(ErrorMessage = "Khong duoc bo trong")]
        public string Masv { get; set; }
        [Display(Name = "Họ sinh viên")]
        [Required(ErrorMessage = "Khong duoc bo trong")]
        public string Hosv { get; set; }
        [Display(Name = "Tên sinh viên")]
        [Required(ErrorMessage = "Khong duoc bo trong")]
        public string Tensv { get; set; }
        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Khong duoc bo trong")]
        public Nullable<System.DateTime> Ngaysinh { get; set; }
        [Display(Name = "Giới tính")]
        public Nullable<bool> Gioitinh { get; set; }
        [Display(Name = "Ảnh sinh viên")]
        public string Anhsv { get; set; }
        [Display(Name ="Địa chỉ")]
        public string Diachi { get; set; }
        [Display(Name = "Mã lớp")]
        public string Malop { get; set; }

        public virtual Lop Lop { get; set; }
    }
}
