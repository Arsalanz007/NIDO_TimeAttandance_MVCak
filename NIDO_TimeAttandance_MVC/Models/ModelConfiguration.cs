using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelConfiguration
    {
        [AllowHtml]
        [Display(Name = "Message")]
        public string Discription { get; set; }


        public int ID { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int TemplateID { get; set; }
        public int TemplateName { get; set; }
        public string TemplateNameString { get; set; }
        public bool TemplateIsActive { get; set; }
        public async Task<bool> SaveEmailSetting()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                tbl_Setting obj = new tbl_Setting();
                obj.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                obj.CreatedDate = DateTime.Now;
                obj.Host = Host;
                obj.Password = EncryptDecrypt.EncryptEX(Password, true);
                obj.Port = Port;
                obj.IsActive = true;
                obj.SenderEmail = SenderEmail;
                db.tbl_Setting.Add(obj);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelConfiguration", "SaveEmailSetting");
                throw;
            }
        }


        public async Task<bool> SaveTemplate()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                tbl_Template obj = new tbl_Template();
                obj.Discription = Discription;
                obj.IsActive = true;
                obj.TemplateName = TemplateName;
                db.tbl_Template.Add(obj);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelConfiguration", "SaveTemplate");
                throw;
            }
        }
        public async Task<bool> SaveWishTemplate()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await db.WishTempaltes.FirstOrDefaultAsync();
               
                data.WishTemplate = Discription;
               
               
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelConfiguration", "SaveTemplate");
                throw;
            }
        }

        public async Task<IList<ModelConfiguration>> GetTemplate()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelConfiguration> List = await (from q in _db.tbl_Template
                                                            orderby q.TemplateName ascending
                                                            select new ModelConfiguration
                                                            {
                                                                TemplateNameString = q.tbl_TemplateName.TemplateName,
                                                                IsActive = q.IsActive,
                                                                ID = q.ID,
                                                            }
                                            ).ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelConfiguration", "GetTemplate");
                return null;
            }


        }
        public async Task<ModelConfiguration> GetTemplateBy_Id(int id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    var List = await (from q in _db.tbl_Template
                                      where q.ID == id
                                      select new ModelConfiguration
                                      {
                                          Discription = q.Discription,
                                          TemplateID = q.TemplateName,
                                          ID = q.ID,
                                      }).SingleOrDefaultAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelConfiguration", "GetTemplateBy_Id");
                return null;
            }
        }

        public async Task<bool> Update()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await db.tbl_Template.Where(x => x.ID == ID).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.Discription = Discription;
                    data.TemplateName = TemplateID;
                    data.IsActive = true;
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelConfiguration", "Update");
                throw;
            }
        }
        public async Task<bool> UpdateEmailSetting()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await db.tbl_Setting.Where(x => x.ID == ID).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.Host = Host;
                    data.IsActive = IsActive;
                    data.Password = EncryptDecrypt.EncryptEX(Password, true);
                    data.SenderEmail = SenderEmail;
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelConfiguration", "Update");
                throw;
            }
        }


        public async Task<IList<ModelConfiguration>> GetEmailSettings()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelConfiguration> List = await (from q in _db.tbl_Setting
                                                            orderby q.SenderEmail ascending
                                                            select new ModelConfiguration
                                                            {
                                                                SenderEmail = q.SenderEmail,
                                                                Host = q.Host,
                                                                Port = q.Port,
                                                                IsActive = q.IsActive,
                                                                ID = q.ID
                                                            }).ToListAsync();
                    return List;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelConfiguration", "GetEmailSettings");
                return null;
            }


        }

        public async Task<ModelConfiguration> GetEmailSetting_Id(int id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    var List = await (from q in _db.tbl_Setting
                                      where q.ID == id
                                      select new ModelConfiguration
                                      {
                                          SenderEmail = q.SenderEmail,
                                          ID = q.ID,
                                          Host = q.Host,
                                          Port = q.Port,
                                          IsActive = q.IsActive,
                                      }).SingleOrDefaultAsync();
                    return List;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelConfiguration", "GetTemplateBy_Id");
                return null;
            }
        }

        public async Task<string> GetSampleEmail()
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();

            var data = await Task.Run(() => db.Nstp_GetSampleEmailTemplate().SingleOrDefault());
            return data;
        }
        public async Task<string> GetWishTemplate()
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();

            var data =await db.WishTempaltes.FirstOrDefaultAsync();
            if(data.WishTemplate==null)
            {
                data.WishTemplate = "";
            }
            return data.WishTemplate;
        }
    }
}