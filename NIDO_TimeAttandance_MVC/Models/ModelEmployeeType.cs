using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelEmployeeType
    {
        public long ID { get; set; }
        public string EmployeeTypeDsc { get; set; }
        public string [] EmployeeTypeDscArray { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public string EditedBy { get; set; }

        public async Task< IList<ModelEmployeeType>> GetEmployeeType()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelEmployeeType> List = await (from q in _db.EmployeeTypes
                                                    select new ModelEmployeeType
                                                    {
                                                        ID = q.ID,
                                                        EmployeeTypeDsc = q.EmployeeTypeDsc,

                                                    }
                                            ).ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmployeeType", "GetEmployeeType");
                return null; ;
            }


        }

        public async Task< bool> DeleteEmployeeType(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data =await _db.EmployeeTypes.Where(x => x.ID == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.EmployeeTypes.Remove(data);
                  await  _db.SaveChangesAsync();

                }
                return true;


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmployeeType", "DeleteEmployeeType");
                return false;

            }
        }

        public async Task<bool> EditEmployeeType()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data =await _db.EmployeeTypes.Where(x => x.ID == this.ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    data.EmployeeTypeDsc = this.EmployeeTypeDsc;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now;
                   await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmployeeType", "EditEmployeeType");
                return false;

            }
        }
        public async Task< bool> EmployeeType_Save()
        {
            try
            {
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                EmployeeType ObjET = new EmployeeType();
                for (int i = 0; i < EmployeeTypeDscArray.Length; i++)
                {
                    ObjET.EmployeeTypeDsc = EmployeeTypeDscArray[i];
                    ObjET.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjET.CreatedDate = DateTime.Now;
                    ObjET.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjET.EditedOn = DateTime.Now;
                    _Db.EmployeeTypes.Add(ObjET);
                   await _Db.SaveChangesAsync();
                }
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmployeeType", "EmployeeType_Save");
                return false;
            }

        }
        public async Task< ModelEmployeeType> Get_EmployeeType_By_Id(long Id)
        {

            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelEmployeeType List = await (from q in _db.EmployeeTypes
                                             where q.ID == Id
                                             select new ModelEmployeeType
                                             {
                                                 ID = q.ID,
                                                 EmployeeTypeDsc = q.EmployeeTypeDsc,
                                             }
                                            ).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmployeeType", "Get_EmployeeType_By_Id");
                return null;
            }

        }

    }
}