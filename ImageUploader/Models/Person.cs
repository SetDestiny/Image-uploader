using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageUploader.Models
{
    public class Person
    {
        public int RecordID { get; set; }
        [Required(ErrorMessage = "Please enter your name!"), RegularExpression("[A-Za-z]*", ErrorMessage = "Invalid name!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your surname!"), RegularExpression("[A-Za-z]*", ErrorMessage = "Invalid surname!")]
        public string Surname { get; set; }
        public int ImageSize { get; set; }
        public string ImageName { get; set; }
        public byte[] ImageData { get; set; }  
    }
}