using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mrgvn.db;
using posrepository;
using posrepository.DTO;

namespace posUnitTest
{
    [TestClass]
    public class SaleTest
    {
        //[TestMethod]
        //public void Create_()
        //{
        //    SalesDTO sDTO = new SalesDTO();
        //    sDTO.total = 0;
        //    sDTO.idcstatus = (int)CSTATUS.ACTIVO;
        //    sDTO.maker = "TestClass";
        //    sDTO.details = new List<SalesDetailsDTO>();

        //    SalesDetailsDTO sdDTO = new SalesDetailsDTO();
        //    sdDTO.unitary_price = 0;
        //    sdDTO.quantity = 1;
        //    sdDTO.idproducts = 1;

        //    SalesDetailsDTO sdDTO2 = new SalesDetailsDTO();
        //    sdDTO2.unitary_price = 0;
        //    sdDTO2.quantity = 1;
        //    sdDTO2.idproducts = 2;


        //    sDTO.details.Add(sdDTO);
        //    sDTO.details.Add(sdDTO2);

        //    ISales ctrlSales = new SalesRepository();
        //    var result = ctrlSales.Create(sDTO);
        //    Assert.IsTrue(result.id > 0, "OK success");
        //}

        //[TestMethod]
        //public void Delete_()
        //{
        //    ISales isr = new SalesRepository();
        //    var item = isr.Delete(5);
        //    Assert.IsTrue(item, "Item delete");
        //}

        //[TestMethod]
        //public void Update_()
        //{
        //    SalesDTO salesDTO = new SalesDTO();
        //    salesDTO.idsales = 5;
        //    salesDTO.idcstatus = (int)CSTATUS.ACTIVO;
        //    salesDTO.maker = "TestClass.TestMethod";
        //    salesDTO.details = new List<SalesDetailsDTO>();

        //    SalesDetailsDTO sdDTO = new SalesDetailsDTO();
        //    sdDTO.quantity = 3;
        //    sdDTO.idproducts = 2;

        //    SalesDetailsDTO sdDTO2 = new SalesDetailsDTO();
        //    sdDTO2.quantity = 4;
        //    sdDTO2.idproducts = 1;

        //    salesDTO.details.Add(sdDTO);
        //    salesDTO.details.Add(sdDTO2);

        //    ISales ctrlSales = new SalesRepository();
        //    var result = ctrlSales.Update(salesDTO);
        //    Assert.IsTrue(result.id > 0, "OK success");
        //}

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
