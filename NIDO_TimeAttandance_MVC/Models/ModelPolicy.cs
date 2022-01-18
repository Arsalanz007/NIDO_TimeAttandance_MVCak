using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelPolicy
    {
        public int MasterID { get; set; }
        public int DetailID { get; set; }
        public string Name { get; set; }
        public int PolicyID { get; set; }
        public int TotalDays { get; set; }
        public int Deduction { get; set; }
        public string piority { get; set; }
        public IEnumerable<LeaveSetup> Leavemodel { get; set; }
        public string[] SelectList { get; set; }
        public bool IsSandwichPolicy { get; set; }
        public bool InOutTypeOrFifoBasedAttend { get; set; }
        public int LeaveID { get; set; }
        public string LeaveName { get; set; }



        public async Task<IList<SelectListItem>> GetAllLeaves()
        {

            try
            {
                using (var _db = new PakOman_NedoEntities())
                {


                    List<SelectListItem> data = await (from ep in _db.LeaveSetups
                                                       select new SelectListItem
                                                       {
                                                           Text = ep.LeaveDsc,
                                                           Value = ep.ID.ToString(),
                                                           Selected = false
                                                       }).ToListAsync();


                    return data;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPolicy", "GetAllLeaves");
                return null;
            }


        }
        public async Task<IList<SelectListItem>> GetPolicyName()
        {

            try
            {
                using (var _db = new PakOman_NedoEntities())
                {


                    List<SelectListItem> data = await (from ep in _db.tblPolicyTypes
                                                       select new SelectListItem
                                                       {
                                                           Text = ep.PolicyName,
                                                           Value = ep.ID.ToString(),
                                                           Selected = false
                                                       }).ToListAsync();


                    return data;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPolicy", "GetAllLeaves");
                return null;
            }


        }

        public async Task<IList<ModelPolicy>> GetPolicy()
        {
            try
            {

                using (var _db = new PakOman_NedoEntities())
                {

                    IList<ModelPolicy> mp = await (from x in _db.MasterPolicies
                                                   join y in _db.tblPolicyTypes on x.PolicyID equals y.ID
                                                   orderby x.ID
                                                   select new ModelPolicy
                                                   {
                                                       MasterID = x.ID,
                                                       DetailID = y.ID,
                                                       Name = y.PolicyName,
                                                       TotalDays = x.TotalDays.Value,
                                                       Deduction = x.Deduct.Value,


                                                   }).ToListAsync();
                    return mp;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPolicy", "SavePolicy");
                return null;
            }


        }
        public async Task<bool> DeletePolicy(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var deleteSelectedList = from x in _db.DetailPolicies
                                         where x.MID == id
                                         select x;

                foreach (var detail in deleteSelectedList)
                {
                    _db.DetailPolicies.Remove(detail);
                }

                var Master = (from x in _db.MasterPolicies where x.ID == id select x).FirstOrDefault();
                _db.MasterPolicies.Remove(Master);
                await _db.SaveChangesAsync();
                return true;

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPolicy", "DeletePolicy");
                return false;
            }


        }
        public async Task<ModelPolicy> GetPolicyByID(int id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelPolicy List = await (from x in _db.MasterPolicies
                                              where x.ID == id

                                              select new ModelPolicy
                                              {
                                                  MasterID = x.ID,
                                                  PolicyID = x.PolicyID.Value,
                                                  TotalDays = x.TotalDays.Value,
                                                  Deduction = x.Deduct.Value,



                                              }).SingleOrDefaultAsync();
                    var policy = _db.SysPolicies.Take(1).FirstOrDefault();
                    List.IsSandwichPolicy = Convert.ToBoolean(policy.IsSandwichAllowed);
                    List.InOutTypeOrFifoBasedAttend = Convert.ToBoolean(policy.InOutTypeOrFifoBasedAttend);

                    var LeaveType = (from x in _db.DetailPolicies
                                     join y in _db.LeaveSetups
        on x.LeaveID equals y.ID
                                     where x.MID == id
                                     orderby x.Priority ascending
                                     select x.LeaveID).ToList();

                    List.SelectList = LeaveType.Select(i => i.ToString()).ToArray();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPolicy", "GetPolicyByID");
                return null;
            }


        }
        public async Task<bool> SavePolicy(ModelPolicy model)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    var checkRecord= _db.MasterPolicies
                    .Where(b => b.PolicyID == model.PolicyID)
                    .FirstOrDefault();

                    if (checkRecord == null)
                    {

                        var data = new MasterPolicy()
                        {
                            PolicyID = model.PolicyID,
                            TotalDays = model.TotalDays,
                            Deduct = model.Deduction,
                            isActive = true,
                            CreatedBy = HttpContext.Current.Session["UserName"].ToString(),
                            CreatedOn = DateTime.Now,
                        };
                        _db.MasterPolicies.Add(data);
                        await _db.SaveChangesAsync();
                        if (model.SelectList != null)
                        {
                            int count = 1;
                            foreach (var item in model.SelectList)
                            {
                                var DetailPolicy = new DetailPolicy
                                {

                                    MID = data.ID,
                                    LeaveID = int.Parse(item),
                                    Priority = count,
                                    CreatedBy = HttpContext.Current.Session["UserName"].ToString(),
                                    CreatedOn = DateTime.Now,

                                };
                                _db.DetailPolicies.Add(DetailPolicy);
                                await _db.SaveChangesAsync();
                                count++;
                            }
                        }

                       


                        return true;
                    }
                    return false;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPolicy", "SavePolicy");
                return false;
            }


        }
        public async Task<bool> UpdatePolicy()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();

            try
            {
                var data = await _db.MasterPolicies.Where(x => x.ID == this.MasterID).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.PolicyID = this.PolicyID;
                    data.TotalDays = this.TotalDays;
                    data.Deduct = this.Deduction;
                    data.isActive = true;
                    data.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.CreatedOn = DateTime.Now;
                    await _db.SaveChangesAsync();
                    if (this.SelectList != null)
                    {
                        var deleteSelectedList = from x in _db.DetailPolicies
                                                 where x.MID == this.MasterID
                                                 select x;

                        foreach (var detail in deleteSelectedList)
                        {
                            _db.DetailPolicies.Remove(detail);
                        }


                        await _db.SaveChangesAsync();


                        int count = 1;
                        foreach (var item in this.SelectList)
                        {
                            var DetailPolicy = new DetailPolicy
                            {
                                MID = data.ID,
                                LeaveID = int.Parse(item),
                                Priority = count,
                                CreatedBy = HttpContext.Current.Session["UserName"].ToString(),
                                CreatedOn = DateTime.Now,

                            };
                            _db.DetailPolicies.Add(DetailPolicy);
                            await _db.SaveChangesAsync();
                            count++;
                        }
                    }
                   
                }

                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCity", "EditCity");
                return false;

            }
        }
    }
}