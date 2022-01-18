using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelGrade
    {
        public int GradeId { get; set; }
        public string GradeDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public string[] GradeDscArray { get; set; }
        public async Task< IList<ModelGrade>> GetGrade()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelGrade> List = await(from q in _db.GradeMasters
                                              orderby q.GradeId ascending
                                              select new ModelGrade
                                              {
                                                  GradeId = q.GradeId,
                                                  GradeDesc = q.GradeDesc,
                                              }
                                            ).ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelGrade", "GetGrade");
                return null; ;
            }


        }
        public async Task< bool> DeleteGrade(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data =await _db.GradeMasters.Where(x => x.GradeId == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.GradeMasters.Remove(data);
                   await _db.SaveChangesAsync();

                }
                return true;


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelGrade", "DeleteGrade");
                return false;

            }
        }
        public async Task< bool> EditGrade()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data =await _db.GradeMasters.Where(x => x.GradeId == this.GradeId).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    data.GradeDesc = this.GradeDesc;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now;
                   await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelGrade", "EditGrade");
                return false;

            }
        }
        public async Task< bool> Grade_Save()
        {
            try
            {
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                GradeMaster ObjGrade = new GradeMaster();
                for (int i = 0; i < GradeDscArray.Length; i++)
                {

                    ObjGrade.GradeDesc = GradeDscArray[i];
                    ObjGrade.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjGrade.CreatedOn = DateTime.Now;
                    ObjGrade.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjGrade.EditedOn = DateTime.Now;
                    _Db.GradeMasters.Add(ObjGrade);
                  await _Db.SaveChangesAsync();
                }
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelGrade", "Grade_Save");
                return false;
            }

        }
        public async Task< ModelGrade> Get_Grade_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelGrade List =await (from q in _db.GradeMasters
                                       where q.GradeId == Id
                                       select new ModelGrade
                                       {
                                           GradeId = q.GradeId,
                                           GradeDesc = q.GradeDesc,
                                       }
                                            ).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelGrade", "Get_Grade_By_Id");
                return null;
            }

        }

    }
}