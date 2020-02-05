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
    public interface ILostItems
    {
        LOSTITEM Create(LostItemDTO dto);
        LOSTITEMDETAIL CreateDetails(LostItemDTO dto);
        List<LOSTITEM> Read(int id = 0, string barcode = "", int idcstatus = -100, decimal price = -100, decimal cost = -100, int existence = -100, bool all = false);
        LOSTITEM Update(LostItemDTO dto);
        LOSTITEMDETAIL UpdateEntry(LostItemDTO dto);

        bool Delete(int id);
        bool DeleteDetail(int id);
    }
    public class LostItemsRepository : ILostItems
    {
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public LOSTITEM Create(LostItemDTO dto)
        {
            LOSTITEM li = new LOSTITEM();

            using (var context = new posContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        li.idcstatus = dto.idcstatus;
                        li.create_date = PosUtil.ConvertToTimestamp(DateTime.Now);
                        li.maker = dto.maker;
                        context.Entry<LOSTITEM>(li).State = EntityState.Added;
                        context.SaveChanges();

                        decimal tmptotal = 0;

                        foreach (var detail in dto.Itemsdetails)
                        {
                            PRODUCT product = context.PRODUCTS.FirstOrDefault(x => x.id == detail.idproducts);
                            LOSTITEMDETAIL lids = new LOSTITEMDETAIL();
                            lids.idlostitems = li.id;
                            lids.unitary_cost = product.unitary_cost;
                            lids.idproducts = detail.idproducts;
                            lids.quantity = detail.quantity;
                            context.Entry<LOSTITEMDETAIL>(lids).State = EntityState.Added;
                            context.SaveChanges();
                            tmptotal = tmptotal + (lids.unitary_cost * lids.quantity);

                        }

                        li.total = tmptotal;
                        context.Entry<LOSTITEM>(li).State = EntityState.Modified;
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



        public LOSTITEMDETAIL CreateDetails(LostItemDTO dto)
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


                    LOSTITEM lost = Read(id: id).First();
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

        public List<LOSTITEM> Read(int id = 0, string barcode = "", int idcstatus = -100, decimal price = -100, decimal cost = -100, int existence = -100, bool all = false)
        {
            List<LOSTITEM> sales = new List<LOSTITEM>();

            using (var context = new posContext())
            {
                // filters 
                if (all)
                    sales = context.LOSTITEMS.
                                   Include(x => x.LOSTITEMDETAILS).
                                   Include(x => x.LOSTITEMDETAILS.Select(sd => sd.PRODUCT)).ToList();
                else if (id >= 0)
                    sales = context.LOSTITEMS.Include(x => x.LOSTITEMDETAILS).
                                          Include(x => x.LOSTITEMDETAILS.Select(sd => sd.PRODUCT)).Where(x => x.id == id).ToList();
            }
            return sales;
        }

        public LOSTITEM Update(LostItemDTO dto)
        {
            LOSTITEM lost = new LOSTITEM();
            
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
                        List<LOSTITEMDETAIL> details = lost.LOSTITEMDETAILS.ToList();

                        foreach (var itemBD in details)
                        {
                            foreach (var itemCurrent in dto.Itemsdetails)
                            {
                                if (itemBD.idproducts == itemCurrent.idproducts)
                                {
                                    itemBD.quantity = itemCurrent.quantity;
                                    tmptotal = tmptotal + (itemBD.unitary_cost * itemCurrent.quantity);

                                    context.Entry<LOSTITEMDETAIL>(itemBD).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }

                        lost.total = tmptotal;
                        context.Entry<LOSTITEM>(lost).State = EntityState.Modified;
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

        public LOSTITEMDETAIL UpdateEntry(LostItemDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
