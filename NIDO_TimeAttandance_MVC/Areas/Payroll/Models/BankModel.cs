using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class BankModel
    {
        public int BankId { get; set; }
        public string Bank_Name { get; set; }
        public int? Account_TypeId { get; set; }
        public int? CityId { get; set; }
        public string Branch_Code { get; set; }
        public string Address { get; set; }
        public string AccountType { get; set; }
        public string CityName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }


        public async Task<bool> SaveBank()
        {

            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var bank = new tbl_BankMaster();
                bank.Bank_Name = Bank_Name;
                bank.Branch_Code = Branch_Code;
                bank.Address = Address;
                bank.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                bank.CreatedOn = DateTime.Now;
                bank.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                bank.EditedOn = DateTime.Now;
                bank.Account_TypeId = Account_TypeId;
                bank.CityId = CityId;
                _db.tbl_BankMaster.Add(bank);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "BankModel", "Bank_SaveBank");
                return false;
            }

        }
        public async Task<IList<BankModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<BankModel> list = await (from q in _db.tbl_BankMaster
                                               orderby q.Id ascending
                                               select new BankModel
                                               {
                                                   BankId = q.Id,
                                                   Bank_Name = q.Bank_Name,
                                                   Branch_Code = q.Branch_Code,
                                               }).ToListAsync();
                return list;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "BankModel", "Bank_GetAll");

                return null;
            }
        }
        public async Task<bool> DeleteBank(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var bankModel = _db.tbl_BankMaster.Where(q => q.Id == id).FirstOrDefault();
                if (bankModel != null)
                {
                    _db.tbl_BankMaster.Remove(bankModel);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "BankModel", "Bank_Delete");

                return false;
            }
        }

        public async Task<BankModel> Get_Bank_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                BankModel am = await (from q in _db.tbl_BankMaster
                                      where q.Id == id
                                      select new BankModel
                                      {
                                          BankId = q.Id,
                                          Bank_Name = q.Bank_Name,
                                          Branch_Code = q.Branch_Code,
                                          Account_TypeId = q.Account_TypeId,
                                          Address = q.Address,
                                          AccountType = q.tbl_AccountType.Name,
                                          CityId = q.CityId,
                                          CityName = q.CityMaster.CityDesc
                                      }).SingleOrDefaultAsync();
                return am;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "BankModel", "Bank_GetBankById");

                return null;
            }
        }
        public async Task<bool> EditBank()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var bank = await _db.tbl_BankMaster.Where(x => x.Id == this.BankId).FirstOrDefaultAsync();
                if (bank != null)
                {
                    bank.Bank_Name = this.Bank_Name;
                    bank.Branch_Code = this.Branch_Code;
                    bank.Address = this.Address;
                    bank.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    bank.EditedOn = DateTime.Now;
                    bank.Account_TypeId = Account_TypeId;
                    bank.CityId = CityId;
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "BankModel", "Bank_Edit");

                return false;

            }
        }
       
    }
}