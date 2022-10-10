using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SliderProject1.Repository;
using SliderProject1.Models;
using MvcPaging;
using System.Configuration;
using System.Drawing;
using SliderProject1.Utility;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SliderProject1.Controllers
{
    public class AdminController : Controller
    {
        private int slider_width;
        private int slider_height;
        private UnitOfWork _UOW;
        private IUtilRepository _util_repo;
        private int defaultPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pgSize"]);
        public AdminController()
        {
            this._UOW = new UnitOfWork();
            this.slider_width = Convert.ToInt32(ConfigurationManager.AppSettings["slider_width"]);
            this.slider_height = Convert.ToInt32(ConfigurationManager.AppSettings["slider_height"]);
            this._util_repo = new UtilRepository();
        }
        #region Index
        public ActionResult Index()
        {
            return View("Login");
        }
        #endregion

        #region Admin Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(adminmodel _obj)
        {
            try
            {
                web_tbl_care_user be_admin = new web_tbl_care_user();
                be_admin = _UOW.care_user_repo.Get(f => f.user_name == _obj.username && f.user_pwd == _obj.password && f.is_active == true && f.is_deleted == false).FirstOrDefault();
                if (be_admin != null)
                {
                    ViewBag.Msg = "Username & Password is Correct";
                    return RedirectToAction("Slider");

                }
            }
            catch (Exception)
            {
                ViewBag.Msg = "Incorrect Username & Password";
            }
            return View(_obj);
        }
        #endregion

        #region Slider
        public ActionResult Slider(string slider_name, int? page)
        {
            
            IList<bm_slider_cms> slid_obj = new List<bm_slider_cms>();
            try
            {
                ViewData["slider_name"] = slider_name;
                int currentPageIndex = page.HasValue ? page.Value : 1;
                slid_obj = _UOW.bm_slider_repo.Get(f => f.Is_Deleted == false, orderBy: s => s.OrderBy(p => p.Img_Order)).ToList();
                
                if (!string.IsNullOrWhiteSpace(slider_name))
                    slid_obj = slid_obj.Where(p => p.Slider_name.ToLower().Contains(slider_name.ToLower())).ToList();
                slid_obj = slid_obj.ToPagedList(currentPageIndex, defaultPageSize);
                
                if (Request.IsAjaxRequest())
                    return PartialView("_AjaxSlider", slid_obj);
                else
                    return View(slid_obj);
            }
            catch (Exception e)
            {

            }
            return View(slid_obj);
        }
        [HttpGet]
        public ActionResult bm_slider(long? id)
        {
           
            long s_id = id.HasValue ? id.Value : 0;
            bm_slider_cms s_obj = new bm_slider_cms();
            try
            {
                if (s_id > 0)
                    s_obj = _UOW.bm_slider_repo.GetByID(s_id);
                return View(s_obj);

            }
            catch (Exception e)
            {

            }
            return View(s_obj);

        }
        [HttpPost]
        public ActionResult bm_slider(bm_slider_cms _obj, HttpPostedFileBase slid_img)
        {
            
            
            bm_slider_cms s_obj = new bm_slider_cms();
            try
            {
                if (slid_img == null && _obj.Slider_Img == null)
                {
                    ViewBag.Msg = "Please Upload the Image";
                    return RedirectToAction("bm_slider", "Admin", new { id = 0 });
                }
                //if (ModelState.IsValid)
                //{ 
                  var r = new Regex(@" ^[a - zA - Z0 - 9] * $");
                  s_obj = _obj;
                    Image slider_image = null;
                    string filename = "";
                    string filepath = "";
                      // if(!r.IsMatch(_obj.Slider_name) )
                      //{
                      //   ViewBag.Msg = "Please avoid special characters ..";
                     // }
                      
                        if (slid_img != null)
                        {
                            string Extension = slid_img.FileName.Remove(0, slid_img.FileName.LastIndexOf('.'));
                            if (Extension != "")
                            {
                                if (Extension.ToLower() == ".jpg" || Extension.ToLower() == ".jpeg")                                                                         //|| Extension.ToLower() == ".png" || Extension.ToLower() == ".gif"
                                {
                                    slider_image = _util_repo.ResizeImage(slid_img.InputStream, slider_width, slider_height);
                                    filename = Guid.NewGuid().ToString() + Extension.ToLower();
                                    filepath = Server.MapPath("~/Upload/") + filename;
                                    s_obj.Slider_Img = filename;
                                }
                                else
                                {
                                    ViewBag.Msg = "Only jpg, jpeg  file formats are allowed to upload..";
                                    return View(_obj);
                                }
                            }

                       // }
                        if (s_obj.Id == 0)
                        {
                            s_obj.Created_On = DateTime.Now;
                            _UOW.bm_slider_repo.Insert(s_obj);
                        }
                        else
                        {
                            s_obj.Modified_On = DateTime.Now;
                            _UOW.bm_slider_repo.Update(s_obj);
                        }
                        if (slider_image != null)
                            slider_image.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
                        _UOW.Save();
                        TempData["success_msg"] = "Slider details has been inserted/updated Successfully";
                        return RedirectToAction("slider", "Admin");

                    }
                    
                
            }
            catch (Exception e)
            {
                ViewBag.Msg = "Falied to insert/update in Slider  Details";
            }
            return View(s_obj);
        }
        
              public ActionResult Delete_Slider(long id)
              {
                  try
                  {
                      if (id > 0)
                      {
                          bm_slider_cms _obj = new bm_slider_cms();
                          _obj = _UOW.bm_slider_repo.GetByID(id);
                          if (_obj != null)
                          {
                              bm_slider_cms s_obj = new bm_slider_cms();
                              s_obj = _obj;
                              s_obj.Is_Active = false;
                              s_obj.Is_Deleted = true;
                              _UOW.bm_slider_repo.Update(s_obj);
                              _UOW.Save();

                              string fileUrl = "/Upload/" + _obj.Slider_Img;
                              bool flag = DeleteFile(fileUrl);
                        return RedirectToAction("Slider");
                          }
                      }
                  }
                  catch (Exception ex)
                  {

                     // return Json(new { Status = "false" });
                  }
                  return null;
              }
       
       

        #endregion



        #region DeleteFile
        private bool DeleteFile(string path)
        {
            bool result = true;
            string filepath = Path.Combine(Server.MapPath("~" + path));
            if(System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
                result = true;
            }
            return result;
        }

        #endregion
    }
}
