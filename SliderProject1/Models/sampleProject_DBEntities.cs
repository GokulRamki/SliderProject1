using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SliderProject1.Models
{
    public class sampleProject_DBEntities  :DbContext
    {
        public sampleProject_DBEntities() : base("sampleProject_DBEntities") { }

        public DbSet<web_tbl_care_user> care_users { get; set; }
       
        public DbSet<bm_slider_cms> bm_slider { get; set; }
    }
}