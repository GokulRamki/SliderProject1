using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SliderProject1.Models;

namespace SliderProject1.Repository
{
    public class UnitOfWork : DbContext, IDisposable
    {
        private sampleProject_DBEntities context = new sampleProject_DBEntities();

        #region care user
        private GenericRepository<web_tbl_care_user> CARE_USER;
        public GenericRepository<web_tbl_care_user> care_user_repo
        {
            get
            {
                if(this.CARE_USER == null)
                {
                    this.CARE_USER = new GenericRepository<web_tbl_care_user>(context);
                }
                return CARE_USER;
            }
        }

        #endregion

        #region Slider
        private GenericRepository<bm_slider_cms> bm_slider_Repo;
        public GenericRepository<bm_slider_cms> bm_slider_repo
        {
            get
            {
                if(this.bm_slider_Repo == null)
                {
                    this.bm_slider_Repo = new GenericRepository<bm_slider_cms>(context);
                }
                return bm_slider_Repo;
            }
        }
        #endregion
        #region Db Save
        public void Save()
        {
            context.SaveChanges();
        }
        #endregion

        #region Disposal

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}