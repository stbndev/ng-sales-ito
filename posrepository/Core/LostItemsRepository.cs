using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using NLog;
using posrepository.DTO;
using mrgvn.db;

namespace posrepository
{
    public interface IShrinkage
    {
        INVENTORYSHRINKAGE Create(LostItemDTO dto);
        INVENTORYSHRINKAGE CreateDetails(LostItemDTO dto);
        List<INVENTORYSHRINKAGE> Read(int id = 0, string barcode = "", int idcstatus = -100, decimal price = -100, decimal cost = -100, int existence = -100, bool all = false);
        INVENTORYSHRINKAGE Update(LostItemDTO dto);
        INVENTORYSHRINKAGE UpdateEntry(LostItemDTO dto);

        bool Delete(int id);
        bool DeleteDetail(int id);
    }

    // public class ShrinkageRepository : IShrinkage
    public class ShrinkageRepository : IShrinkage
    {
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public INVENTORYSHRINKAGE Create(LostItemDTO dto)
        {
            INVENTORYSHRINKAGE li = new INVENTORYSHRINKAGE();

            using (var context = new posContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        li.idcstatus = dto.idcstatus;
                        li.create_date = PosUtil.ConvertToTimestamp(DateTime.Now);
                        li.maker = dto.maker;
                        context.Entry<INVENTORYSHRINKAGE>(li).State = EntityState.Added;
                        context.SaveChanges();

                        decimal tmptotal = 0;

                        foreach (var detail in dto.Itemsdetails)
                        {
                            PRODUCT product = context.PRODUCTS.FirstOrDefault(x => x.id == detail.idproducts);
                            INVENTORYSHRINKAGEDETAIL lids = new INVENTORYSHRINKAGEDETAIL();
                            lids.idlostitems = li.id;
                            lids.unitary_cost = product.unitary_cost;
                            lids.idproducts = detail.idproducts;
                            lids.quantity = detail.quantity;
                            context.Entry<INVENTORYSHRINKAGEDETAIL>(lids).State = EntityState.Added;
                            context.SaveChanges();
                            tmptotal = tmptotal + (lids.unitary_cost * lids.quantity);

                        }

                        li.total = tmptotal;
                        context.Entry<INVENTORYSHRINKAGE>(li).State = EntityState.Modified;
                        context.SaveChanges();
                        transaction.Commit();
                        Logger.Info("PRODUCTENTRIES PRODUCTENTRIESDETAILS PRODUCT");
                    }
                    catch (Exception tex)
                    {
                        transaction.Rollback();
                        li.id = -1;
                        Logger.Error(tex.Message);
                        Logger.Error(tex.InnerException.Message);
                    }
                }
            }
            return li;

        }



        public INVENTORYSHRINKAGEDETAIL CreateDetails(LostItemDTO dto)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            bool flag = false;
            try
            {
                using (var context = new posContext())
                {


                    INVENTORYSHRINKAGE lost = Read(id: id).First();
                    if (lost.idcstatus != (int)CSTATUS.ELIMINADO)
                    {
                        lost.idcstatus = (int)CSTATUS.ELIMINADO;
                        lost.modification_date = PosUtil.ConvertToTimestamp(DateTime.Now);
                        context.Entry(lost).State = EntityState.Modified;
                        context.SaveChanges();

                        flag = true;
                        Logger.Info(lost);
                    }
                    else
                        flag = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                flag = false;
            }
            return flag;
        }

        public bool DeleteDetail(int id)
        {
            throw new NotImplementedException();
        }

        public List<INVENTORYSHRINKAGE> Read(int id = 0, string barcode = "", int idcstatus = -100, decimal price = -100, decimal cost = -100, int existence = -100, bool all = false)
        {
            List<INVENTORYSHRINKAGE> sales = new List<INVENTORYSHRINKAGE>();

            using (var context = new posContext())
            {
                // filters 
                if (all)
                    sales = context.INVENTORYSHRINKAGEs.
                                   Include(x => x.INVENTORYSHRINKAGEDETAILS).
                                   Include(x => x.INVENTORYSHRINKAGEDETAILS.Select(sd => sd.PRODUCT)).ToList();
                else if (id >= 0)
                    sales = context.INVENTORYSHRINKAGEs.Include(x => x.INVENTORYSHRINKAGEDETAILS).
                                          Include(x => x.INVENTORYSHRINKAGEDETAILS.
                                          Select(sd => sd.PRODUCT)).Where(x => x.id == id).ToList();
            }
            return sales;
        }

        public INVENTORYSHRINKAGE Update(LostItemDTO dto)
        {
            INVENTORYSHRINKAGE lost = new INVENTORYSHRINKAGE();
            
            if (dto.id <= 0)
                return null;


            using (var context = new posContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        lost = Read(id: dto.id).FirstOrDefault();
                        lost.idcstatus = dto.idcstatus;
                        lost.modification_date = PosUtil.ConvertToTimestamp(DateTime.Now);

                        decimal tmptotal = 0;
                        List<INVENTORYSHRINKAGEDETAIL> details = lost.INVENTORYSHRINKAGEDETAILS.ToList();

                        foreach (var itemBD in details)
                        {
                            foreach (var itemCurrent in dto.Itemsdetails)
                            {
                                if (itemBD.idproducts == itemCurrent.idproducts)
                                {
                                    itemBD.quantity = itemCurrent.quantity;
                                    tmptotal = tmptotal + (itemBD.unitary_cost * itemCurrent.quantity);

                                    context.Entry<INVENTORYSHRINKAGEDETAIL>(itemBD).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }

                        lost.total = tmptotal;
                        context.Entry<INVENTORYSHRINKAGE>(lost).State = EntityState.Modified;
                        context.SaveChanges();
                        transaction.Commit();
                        Logger.Info(lost);
                    }
                    catch (Exception tex)
                    {
                        transaction.Rollback();
                        lost.id = -1;
                        Logger.Error(tex.Message);
                    }
                }
            }
            return lost;
        }

        public INVENTORYSHRINKAGEDETAIL UpdateEntry(LostItemDTO dto)
        {
            throw new NotImplementedException();
        }

        INVENTORYSHRINKAGE IShrinkage.CreateDetails(LostItemDTO dto)
        {
            throw new NotImplementedException();
        }

        INVENTORYSHRINKAGE IShrinkage.UpdateEntry(LostItemDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
