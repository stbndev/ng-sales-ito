using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mrgvn.db;
using posrepository;
using posrepository.DTO;

namespace posUnitTest
{
    [TestClass]
    public class LostItemsRepositoryTest
    {
        //[TestMethod]
        //public void Create_()
        //{
        //    LostItemDTO lidto = new LostItemDTO();
        //    lidto.total = 0;
        //    lidto.idcstatus = (int)CSTATUS.ACTIVO;
        //    lidto.maker = "TestClass";
        //    //lidto.details = new List<LostItemDTO>();

        //    LostItemDetailsDTO lidsDTO = new LostItemDetailsDTO();
        //    lidsDTO.cost = 0;
        //    lidsDTO.quantity = 2;
        //    lidsDTO.idproducts = 2;

        //    LostItemDetailsDTO lidsDTO2 = new LostItemDetailsDTO();
        //    lidsDTO2.cost = 0;
        //    lidsDTO2.quantity = 2;
        //    lidsDTO2.idproducts = 1;


        //    lidto.details.Add(lidsDTO);
        //    lidto.details.Add(lidsDTO2);

        //    ILostItemsRepository ctrlLost = new LostItemsRepository();
        //    var result = ctrlLost.Create(lidto);
        //    Assert.IsTrue(result.id > 0, "OK success");
        //}

        //[TestMethod]
        //public void Delete_()
        //{
        //    ILostItemsRepository isr = new LostItemsRepository();
        //    var item = isr.Delete(1);
        //    Assert.IsTrue(item, "Item delete");
        //}

        [TestMethod]
        public void Update_()
        {
            
            LostItemDTO lost = new LostItemDTO();
            lost.id = 1;
            lost.idcstatus = (int)CSTATUS.ACTIVO;
            lost.maker = "TestClass.TestMethod";
            lost.Itemsdetails = new List<LostItemDetailsDTO>();

            LostItemDetailsDTO details = new LostItemDetailsDTO();
            details.quantity = 5;
            details.idproducts = 2;

            LostItemDetailsDTO details2 = new LostItemDetailsDTO();
            details2.quantity = 5;
            details2.idproducts = 1;

            lost.Itemsdetails.Add(details);
            lost.Itemsdetails.Add(details2);

            ILostItems ctrlSales = new LostItemsRepository();
            var result = ctrlSales.Update(lost);
            Assert.IsTrue(result.id > 0, "OK success");
        }

        //private long ConvertToTimestamp(DateTime value)
        //{
        //    long epoch = (value.Ticks - 621355968000000000) / 10000000;
        //    return epoch;
        //}

        //[TestMethod]
        //public void Update_()

        //{
        //PRODUCTENTRy item = new PRODUCTENTRy();
        //item.idproducts = 4;
        //item.quantity = 1000;
        //item.unitary_cost = (decimal)9.5;
        //item.id = 1;

        //IProductentries productentries = new ProductentriesRepository();
        //item = productentries.Update(item);
        //Assert.IsTrue(item.id > 0, "Item Set");
        //}

        //[TestMethod]
        //public void Delete_()
        //{
        //    IProducts products = new ProductsRepository();
        //    int idcstatus = (int)CSTATUS.INACTIVO;
        //    bool flag = products.Delete(this.id, idcstatus);
        //    Assert.IsTrue(flag, "Delete product");
        //}
    }
}
