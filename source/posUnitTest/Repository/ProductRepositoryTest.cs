using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mrgvn.db;
using posrepository;
using posrepository.DTO;
namespace posUnitTest
{
    [TestClass]
    public class ProductRepositoryTest
    {

        //[TestMethod]
        //public void ProductCreate_()
        //{
        //    ProductDTO dto = new ProductDTO();
        //    dto.name = "HALLS EXTRA STRONG";
        //    dto.barcode = "7622210427106";
        //    dto.idcstatus = (int)CSTATUS.ACTIVO;
        //    dto.price = (decimal)7;
        //    dto.unitary_cost= (decimal)4.58;
        //    dto.existence = 12;
        //    IProducts products = new ProductsRepository();
        //    var item = products.Create(dto);
        //    //this.id = item.id;
        //    Assert.IsTrue(item.id > 0, "Item add");
        //}

        //[TestMethod]
        //public void ProductEntryCreate_()
        //{
        //    ProductDTO dto = new ProductDTO();
        //    dto.idcstatus = (int)CSTATUS.ACTIVO;
        //    dto.unitary_cost = (decimal)9.99;
        //    // dto.unitary_price = (decimal)13;
        //    dto.quantity = 1;
        //    dto.idproducts = 1;

        //    IProducts products = new ProductsRepository();
        //    var item = products.CreateEntry(dto);

        //    Assert.IsTrue(item.id > 0, "Item add");
        //}

        //[TestMethod]
        //public void ProductUpdate_()
        //{
        //    ProductDTO dto = new ProductDTO();
        //    dto.idproducts = 1 ;  
        //    dto.name = "SOL Tubi-Papa ORIGINAL";
        //    dto.barcode = "7503020501623";
        //    dto.idcstatus = (int)(CSTATUS.INACTIVO);
        //    dto.unitary_price = (decimal)14;
        //    dto.existence = 1;

        //    IProducts ctrlProduct = new ProductsRepository();
        //    var item = ctrlProduct.Update(dto);
        //    Assert.IsTrue(item.id > 0, "Item update");
        //}

        //[TestMethod]
        //public void ProductEntryUpdate_()
        //{
        //    ProductDTO dto = new ProductDTO();
        //    dto.idproductentries = 13;
        //    dto.unitary_cost = (decimal)9.89;
        //    dto.quantity = 10;
        //    dto.idproducts = 1;
        //    dto.idcstatus = (int)CSTATUS.ACTIVO;
        //    IProducts ctrlProduct = new ProductsRepository();
        //    var item = ctrlProduct.UpdateEntry(dto);
        //    Assert.IsTrue(item.id > 0, "Item update");
        //}

        //[TestMethod]
        //public void Delete_()
        //{
        //    IProducts products = new ProductsRepository();
        //    int idcstatus = (int)CSTATUS.ELIMINADO;
        //    bool flag = products.Delete(1, idcstatus);
        //    Assert.IsTrue(flag, "Delete product");
        //}

        //[TestMethod]
        //public void DeleteEntry_()
        //{
        //    IProducts products = new ProductsRepository();
        //    int idcstatus = (int)CSTATUS.ELIMINADO;
        //    bool flag = products.DeleteEntry(13);
        //    Assert.IsTrue(flag, "Delete product");
        //}

    }
}
