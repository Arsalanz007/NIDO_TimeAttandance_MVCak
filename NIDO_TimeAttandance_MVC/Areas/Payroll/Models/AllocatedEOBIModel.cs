using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AllocatedEOBIModel
    {
        public int AllocatedEOBIId { get; set; }
        public int? EOIBId { get; set; }
        public string EOBIName { get; set; }
        public long EmpId { get; set; }
        public DateTime? EOBIDate { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeType { get; set; }

        //public DateTime? AllocatedMonth { get; set; }


        public async Task<IList<AllocatedEOBIModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<AllocatedEOBIModel> aam = await (from q in _db.tbl_AllocatedEOBI
                                                            select new AllocatedEOBIModel
                                                            {
                                                                AllocatedEOBIId = q.AllocatedEOBIId,
                                                                EOIBId = q.EOBIId,
                                                                EOBIName= q.tbl_EOBIMaster.EOBIName,
                                                                EmpId = q.EmpMaster.EmpId,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                Department = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                                Designation = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                                EmployeeType = q.EmpMaster.EmployeeType.EmployeeTypeDsc,
                                                                Company = q.EmpMaster.CompanyMaster.CompanyDesc,
                                                                EOBIDate = q.EobiDate
                                                                
                                                                //AllocatedMonth = q.AllowanceMonth

                                                            }).ToListAsync();
                
                return aam;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedEOBI", "AllocatedEOBI_GetAll");
                return null;
            }
        }
        public async Task<bool> DeleteAllocation(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var allocation = _db.tbl_AllocatedEOBI.Where(x => x.AllocatedEOBIId == id).FirstOrDefault();
                if (allocation != null)
                {
                    _db.tbl_AllocatedEOBI.Remove(allocation);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedEOBI", "DeleteEOBIAllocation");
                return false;
            }
        }
        public async Task<bool> AllocateEOBI(string empIds, int iEOBIId,DateTime eobiDate)
        {
            try
            {
                string[] sEmployeeIds = empIds.Split(new char[] { ',' });
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                foreach (string empid in sEmployeeIds)
                {
                    if (empid.Trim() == string.Empty)
                        continue;
                    //  DateTime dtCurrent = dtFrom;
                //    var taa = new tbl_AllocatedEOBI();

                    long lEmpid = Convert.ToInt64(empid.Trim());
                   // while (dtCurrent <= dtTill)
                 //   {
                       var  eobiModel = _db.tbl_AllocatedEOBI.Where(x => x.EmpId == lEmpid ).FirstOrDefault();
                        if(eobiModel == null)
                        eobiModel = new tbl_AllocatedEOBI();

                    eobiModel.EOBIId = iEOBIId;
                    eobiModel.EmpId = lEmpid;// Convert.ToInt64(empid.Trim());
                    eobiModel.EobiDate = eobiDate;
                                           //taa.AllowanceMonth = dtCurrent;

                    if (eobiModel.AllocatedEOBIId == 0)
                    {

                    _db.tbl_AllocatedEOBI.Add(eobiModel);
                        //          dtCurrent = dtCurrent.AddMonths(1);
                        //         continue;
                    }

                       // dtCurrent = dtCurrent.AddMonths(1);

                  //  }


                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedEOBI", "AllocatedEOBI_Allocate");
                return false; ;

            }
        }
    }
}