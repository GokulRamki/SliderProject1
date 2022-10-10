using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SliderProject1.Models
{
    public class bm_slider_cms
    {
        [Key()]
        public long Id { get; set; }

        [Required()]
        [RegularExpression (("^[a-zA-Z0-9]*$"), ErrorMessage = "Please Enter valid Slider Name not allowed special characters")]
        
        public string Slider_name { get; set; }
        [Required(ErrorMessage ="Allows only Jpeg file only")]
        public string Slider_Img { get; set; }

        public string Redirect_url { get; set; }

        [Required()]
        [Display(Name = "Order Id")]
        public int Img_Order { get; set; }

        public bool Is_Active { get; set; }

        public bool Is_Deleted { get; set; }

        public Nullable<DateTime> Created_On { get; set; }

        public Nullable<DateTime> Modified_On { get; set; }


    }
}