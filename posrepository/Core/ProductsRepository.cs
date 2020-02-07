using mrgvn.db;
using NLog;
using posrepository.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
namespace posrepository
{
    public interface IProducts
    {

        PRODUCT Create(ProductDTO product);
        PRODUCTENTRy CreateEntry(EntryDTO dto);
        List<PRODUCT> Read(int id = 0, string barcode = "", int idcstatus = -100, decimal price = -100, decimal cost = -100, int existence = -100, bool all = false);
        List<PRODUCTENTRy> ReadEntry(int id = 0, bool all = false);
        PRODUCT Update(ProductDTO dto);
        PRODUCTENTRy UpdateEntry(EntryDTO dto);

        bool Delete(int id);
        bool DeleteEntry(int id);
    }

    public class ProductsRepository : IProducts
    {
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public bool DeleteEntry(int id)
        {
            bool flag = false;
            try
            {
                using (var context = new posContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            PRODUCTENTRy productentry = ReadEntry(id: id).FirstOrDefault();
                            if (productentry.idcstatus != (int)CSTATUS.ELIMINADO)
                            {
                                productentry.idcstatus = (int)CSTATUS.ELIMINADO;
                                //var productentrydetails = productentry.PRODUCTENTRYDETAILS.FirstOrDefault(x => x.idproductentries == id);
                                var productentrydetails = productentry.PRODUCTENTRYDETAILS.FirstOrDefault();

                                var product = context.PRODUCTS.FirstOrDefault(x => x.id == productentrydetails.idproducts);
                                var extractitems = product.existence - productentrydetails.quantity;
                                product.existence = extractitems;
                                context.Set<PRODUCTENTRy>().AddOrUpdate(productentry);
                                context.SaveChanges();
                                transaction.Commit();
                                flag = true;
                                Logger.Info(string.Format("PRODUCTENTRY DELETED {0} ", id));
                                Logger.Info(string.Format("PRODUCT MINUS{0} ", productentrydetails.quantity));
                            }
                            else
                                flag = false;
                        }
                        catch (Exception tex)
                        {
                            flag = false;
                            transaction.Rollback();
                            Logger.Error(tex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                flag = false;
            }
            return flag;
        }
        public List<PRODUCTENTRy> ReadEntry(int id = 0, bool all = false)
        {
            List<PRODUCTENTRy> listItems = new List<PRODUCTENTRy>();
            try
            {
                using (var context = new posContext())
                {
                    // filters 
                    if (all)
                        listItems = context.PRODUCTENTRIES.Include(x => x.PRODUCTENTRYDETAILS).
                                                           Include(x => x.PRODUCTENTRYDETAILS.Select(t => t.PRODUCT)).ToList();
                    else if (id > 0)
                        listItems = context.PRODUCTENTRIES.Include(x => x.PRODUCTENTRYDETAILS).
                                                           Include(x => x.PRODUCTENTRYDETAILS.Select(t => t.PRODUCT))
                                                           .Where(x => x.id == id).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            return listItems;
        }

        public PRODUCTENTRy UpdateEntry(EntryDTO dto)
        {
            PRODUCTENTRy productEntryDB = new PRODUCTENTRy();
            try
            {
                if (dto.idproductentries <= 0)
                    return null;

                using (var context = new posContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            productEntryDB = ReadEntry(id: dto.idproductentries).FirstOrDefault();
                            PRODUCTENTRYDETAIL productEntrydetailDB = productEntryDB.PRODUCTENTRYDETAILS.FirstOrDefault();
                            PRODUCT productDB = context.PRODUCTS.FirstOrDefault(x => x.id == productEntrydetailDB.idproducts);


                            if (productEntryDB.idcstatus != dto.idcstatus)
                            {
                                if (dto.idcstatus == (int)CSTATUS.ACTIVO)
                                    productDB.existence = (productDB.existence + productEntrydetailDB.quantity);
                                else
                                    productDB.existence = (productDB.existence - dto.quantity);
                            }


                            productEntrydetailDB.quantity = dto.quantity;
                            productEntrydetailDB.unitary_cost = dto.unitary_cost;
                            productEntryDB.total = dto.unitary_cost * dto.quantity;
                            productEntryDB.idcstatus = dto.idcstatus;
                            productDB.unitary_cost = dto.unitary_cost;

                            context.Entry<PRODUCTENTRy>(productEntryDB).State = EntityState.Modified;
                            context.Entry<PRODUCTENTRYDETAIL>(productEntrydetailDB).State = EntityState.Modified;
                            context.Entry<PRODUCT>(productDB).State = EntityState.Modified;
                            context.SaveChanges();

                            transaction.Commit();
                            //Logger.Info("PRODUCTENTRIES: id {0} total {1} create_date {2} idcstatus {3}", productEntryDB.id, productEntryDB.total, productEntryDB.create_date, productEntryDB.idcstatus);
                            Logger.Info(dto + "Set");
                        }
                        catch (Exception tex)
                        {
                            transaction.Rollback();
                            Logger.Error(tex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // productEntryParam.id = -1;
                Logger.Error(ex.Message);
                return null;
            }
            return productEntryDB;
        }
        public PRODUCTENTRy CreateEntry(EntryDTO dto)
        {
            PRODUCTENTRy pe = new PRODUCTENTRy();
            try
            {
                using (var context = new posContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            pe = new PRODUCTENTRy();
                            pe.total = dto.unitary_cost * dto.quantity;
                            pe.create_date = PosUtil.ConvertToTimestamp(DateTime.Now);
                            pe.idcstatus = dto.idcstatus;
                            context.Entry<PRODUCTENTRy>(pe).State = EntityState.Added;
                            context.SaveChanges();

                            PRODUCTENTRYDETAIL ped = new PRODUCTENTRYDETAIL();
                            ped.idproductentries = pe.id;
                            ped.unitary_cost = dto.unitary_cost;
                            ped.quantity = dto.quantity;
                            ped.idproducts = dto.idproducts;
                            context.Entry<PRODUCTENTRYDETAIL>(ped).State = EntityState.Added;
                            // context.SaveChanges();

                            PRODUCT product = context.PRODUCTS.FirstOrDefault(x => x.id == dto.idproducts);

                            if (pe.idcstatus == (int)CSTATUS.ACTIVO)
                                product.existence = product.existence + dto.quantity;
                            else
                                product.existence = product.existence - dto.quantity;

                            product.unitary_cost = dto.unitary_cost;

                            context.Entry<PRODUCT>(product).State = EntityState.Modified;
                            context.SaveChanges();
                            transaction.Commit();
                            pe = ReadEntry(id: pe.id).First();
                            return pe;
                        }
                        catch (Exception tex)
                        {
                            transaction.Rollback();
                            Logger.Error(tex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return null;
            }
            return pe;
        }
        public PRODUCT Create(ProductDTO dto)
        {
            PRODUCT p = new PRODUCT();

            using (var context = new posContext())
            {
                try
                {
                    // check if no exist barcode 
                    var checkExist = Read(barcode: dto.barcode);

                    if (checkExist.Count > 0)
                    {
                        Logger.Info("Unavailable barcode {0}", dto.barcode);
                        p.id = 0;
                    }
                    else
                    {
                        p.name = dto.name;
                        p.barcode = dto.barcode;
                        p.idcstatus = dto.idcstatus;
                        p.unitary_price = dto.unitary_price;
                        p.unitary_cost = dto.unitary_cost;
                        p.existence = dto.existence;
                        context.Entry(p).State = EntityState.Added;
                        context.SaveChanges();
                        Logger.Info("{0} {1} {2} {3} {4} {5} {6} ", p.id, p.name, p.barcode, p.idcstatus, p.unitary_price, p.unitary_cost, p.existence);

                    }
                }
                catch (Exception ex)
                {
                    p.id = -1;
                    Logger.Error(ex.Message);
                }
            }
            return p;
        }
        public bool Delete(int id)
        {
            try
            {
                using (var context = new posContext())
                {
                    var item = Read(id: id).FirstOrDefault();
                    item.idcstatus = (int)CSTATUS.ELIMINADO;
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                }
                Logger.Info(string.Format("Product:{0} cstatus:{1}", id, CSTATUS.ELIMINADO));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return false;
            }
        }

        public List<PRODUCT> Read(int id = 0, string barcode = "", int idcstatus = -100, decimal price = -100, decimal cost = -100, int existence = -100, bool all = false)
        {
            List<PRODUCT> listProducts = new List<PRODUCT>();
            try
            {
                using (var context = new posContext())
                {
                    // filters 
                    if (all)
                    {
                        listProducts = context.PRODUCTS.ToList();
                    }
                    else if (!string.IsNullOrEmpty(barcode))
                    {
                        listProducts = context.PRODUCTS.Where(x => x.barcode == barcode).ToList();
                    }
                    else if (id > 0)
                    {
                        listProducts = context.PRODUCTS.Where(x => x.id == id).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            return listProducts;
        }

        public PRODUCT Update(ProductDTO dto)
        {
            PRODUCT item = new PRODUCT();
            if (dto.idproducts <= 0)
                return null;
            try
            {
                using (var context = new posContext())
                {
                    item = Read(id: dto.idproducts).FirstOrDefault();
                    item.name = dto.name;
                    item.barcode = dto.barcode;
                    item.idcstatus = dto.idcstatus;
                    item.unitary_price = dto.unitary_price;
                    item.unitary_cost = dto.unitary_cost == 0 ? item.unitary_cost : dto.unitary_cost;
                    item.existence = dto.existence;
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                    Logger.Info("Product Update {0} ", item.id);
                }
            }
            catch (Exception ex)
            {
                item.id = -1;
                Logger.Error(ex.Message);
            }
            return item;
        }


    }
}
