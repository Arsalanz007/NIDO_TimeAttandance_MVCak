using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class SalaryIncrementModel
    {
        public int Id { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public double? Increment_Amount { get; set; }
        public double? BasicSalary { get; set; }
        public DateTime? Increment_Date { get; set; }

        public string Increment_DateString { get; set; }

        public int DesignationId { get; set; }
        public string Designation { get; set; }

        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public string Increment_By { get; set; }
       



        public async Task<IList<SalaryIncrementModel>> GetSalaryHistory(long empId)
        {
            var db = new PakOman_NedoEntities();

            IList<SalaryIncrementModel> model = await db.tbl_SalaryIncrement_Posting.Where(c => c.EmpId == empId).Select(c => new SalaryIncrementModel
            {
                Id = c.Id,
                EmpId = c.EmpId.Value,
                EmpCode = c.EmpMaster.EmpCode,
                EmpName = c.EmpMaster.EmpName,
                Increment_Amount = c.Increment_Amount,
                Designation = c.DesignationMaster.DesignationDesc,
                DesignationId = c.DesignationId.Value,
                Department = c.DepartmentMaster.DepartmentDesc,
                DepartmentId = c.DepartmentId.Value,
                BasicSalary = c.BasicSalary,
                Increment_Date = c.Increment_Date,
                Increment_By = c.CreatedBy

            }).ToListAsync();

            foreach (var item in model)
            {
                item.Increment_DateString = item.Increment_Date.Value.ToShortDateString();
            }

            return model;
        }

        public async Task<double> GetBasicSalary(long empId)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                var emp = await _db.EmpMasters.Where(c => c.EmpId == empId).FirstOrDefaultAsync();
                return  emp.BasicSallary ?? 0;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "SalaryIncrementModel", "SalaryIncrement_GetBasicSalary");
                return 0;
            }

        }

        public async Task<SalaryIncrementModel> GetEmployeeInfo(long empId)
        {
            var db = new PakOman_NedoEntities();

            var model = await db.EmpMasters.Where(c => c.EmpId == empId).Select(c => new SalaryIncrementModel
            {
               
                EmpId = c.EmpId,
                EmpCode = c.EmpCode,
                EmpName = c.EmpName,
                Designation = c.DesignationMaster.DesignationDesc,
                Department = c.DepartmentMaster.DepartmentDesc,
                DepartmentId = c.DepartmentId.Value,
                BasicSalary = c.BasicSallary,

            }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<IList<SalaryIncrementModel>> GetSalaryIncrementReport(long[] employeeIds, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                IList<SalaryIncrementModel> model = await _db.tbl_SalaryIncrement_Posting.Where(c => employeeIds.Contains(c.EmpId.Value)
               
                && (c.Increment_Date.Value >= DateFrom.Date && c.Increment_Date.Value <= DateTo.Date))
                    .Select(c => new SalaryIncrementModel
                    {
                        Id = c.Id,
                        EmpId = c.EmpId.Value,
                        EmpCode = c.EmpMaster.EmpCode,
                        EmpName = c.EmpMaster.EmpName,
                        Increment_Amount = c.Increment_Amount,
                        Designation = c.DesignationMaster.DesignationDesc,
                        DesignationId = c.DesignationId.Value,
                        Department = c.DepartmentMaster.DepartmentDesc,
                        DepartmentId = c.DepartmentId.Value,
                        BasicSalary = c.BasicSalary,
                        Increment_Date = c.Increment_Date,
                        Increment_By = c.CreatedBy,

                    }).ToListAsync();


                
                return model;



            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "SalaryIncrementModel", "SalaryIncrement_GetBasicSalary");
                return null;
            }

        }
        public async Task<bool> SaveIncrement()
        {

            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                var incrementList = await _db.tbl_SalaryIncrement_Posting.Where(c => c.EmpId == EmpId).ToListAsync();
                incrementList.Exists(x => x.BasicSalary.Equals(1));
                if(incrementList.Count > 0)
                    incrementList.ForEach(c => c.IsActive = false);

                var emp = await _db.EmpMasters.Where(c => c.EmpId == EmpId).FirstOrDefaultAsync();
                var model = new tbl_SalaryIncrement_Posting();
                if(Id > 0)
                {
                    model =await _db.tbl_SalaryIncrement_Posting.Where(c => c.Id == Id).FirstOrDefaultAsync();
                    model.Increment_Amount = Increment_Amount;
                   
                }
                else
                {

                    model.Increment_Amount =  Increment_Amount;
                    
                    model.BasicSalary = incrementList.Count>0? incrementList.Where(x => x.Increment_Date.Value == (incrementList.Select(y => y.Increment_Date.Value).Max())).Select(x=>x.BasicSalary.Value).SingleOrDefault() + incrementList[incrementList.Count-1].Increment_Amount : emp.BasicSallary;
                }
                model.EmpId = EmpId;
                model.DesignationId = DesignationId;
                model.DepartmentId = DepartmentId;
             
               
                model.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                model.CreatedOn = DateTime.Now;
                model.Increment_Date = Increment_Date;
                model.IsActive = true;


                emp.DesignationID = DesignationId;
                emp.DepartmentId = DepartmentId;
               // emp.BasicSallary = model.BasicSalary + model.Increment_Amount;


                if (Id == 0)
                    _db.tbl_SalaryIncrement_Posting.Add(model);

                await _db.SaveChangesAsync();

        
                
             



                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "SalaryIncrementModel", "SalaryIncrement_SaveIncrement");
                return false;
            }

        }


        public bool Increment_Delete(int id) {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var deleteitem = _db.tbl_SalaryIncrement_Posting.SingleOrDefault(q => q.Id == id);
                _db.tbl_SalaryIncrement_Posting.Remove(deleteitem);
                _db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "SalaryIncrementModel", "Increment_Delete");
                return false;
            }

        }
    }
}