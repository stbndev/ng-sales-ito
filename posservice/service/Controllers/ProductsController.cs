using AutoMapper;
using mrgvn.db;
using posrepository;
using posrepository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace service.Controllers
{
    public class ProductsController : ApiController
    {
        private IProducts ng;
        public ProductsController(IProducts products) { ng = products; }


        [HttpDelete]
        ///[ResponseType(typeof(UsuarioDTO))]
        [Route("api/products/entries/{id}")]
        public HttpResponseMessage DeleteEntry(int id)
        {
            return executeactionentries(Action.DELETE, id: id);
        }

        [HttpPut]
        ///[ResponseType(typeof(UsuarioDTO))]
        [Route("api/products/entries/{id}")]
        public HttpResponseMessage PutEntry(int id, [FromBody] EntryDTO dto)
        {
            return executeactionentries(Action.UPDATE, id: id, value: dto);
        }

        [HttpGet]
        ///[ResponseType(typeof(UsuarioDTO))]
        [Route("api/products/entries")]
        public HttpResponseMessage GetEntry()
        {
            return executeactionentries(Action.READALL);
        }

        [HttpGet]
        ///[ResponseType(typeof(UsuarioDTO))]
        [Route("api/products/entries/{id}")]
        public HttpResponseMessage GetEntry(int id)
        {
            return executeactionentries(Action.READID, id: id);
        }

        [HttpPost]
        ///[ResponseType(typeof(UsuarioDTO))]
        [Route("api/products/entries")]
        public HttpResponseMessage Login([FromBody] EntryDTO dto)
        {
            return executeactionentries(Action.CREATE, value: dto);
        }
        // GET: api/Product
        //public HttpResponseMessage  Get()
        public HttpResponseMessage Get()
        {
            // return Request.CreateResponse<MyOder>(HttpStatusCode.Created, order);
            return executeaction(Action.READALL);
        }

        // GET: api/Product/5
        //public async Task<HttpResponseMessage> Get(int id)
        public HttpResponseMessage Get(int id)
        {

            //var t =  Task.Run(()=> ng.Read(id: id).First());
            //t.Wait();
            //result = t.Result;
            return executeaction(Action.READID, id: id);
        }

        // POST: api/Product
        public HttpResponseMessage Post([FromBody]ProductDTO value)
        {
            return executeaction(Action.CREATE, value: value);
        }

        // PUT: api/Product/5
        public HttpResponseMessage Put(int id, [FromBody]ProductDTO value)
        {
            value.idproducts = id;
            return executeaction(Action.UPDATE, id: id, value: value);

        }

        // DELETE: api/Product/5
        public HttpResponseMessage Delete(int id)
        {
            return executeaction(Action.DELETE, id: id);
        }

        private HttpResponseMessage executeaction(Action action, int id = 0, ProductDTO value = null)
        {
            ResponseModel rm = new ResponseModel();
            PRODUCT result = new PRODUCT();

            try
            {

                switch (action)
                {
                    case Action.CREATE:
                        result = ng.Create(value);
                        break;
                    case Action.READID:
                        result = ng.Read(id: id).First();
                        break;
                    case Action.READALL:
                        var results = ng.Read(all: true);
                        // rm.result = results;
                        List<ProductDTO> p = Mapper.Map<List<ProductDTO>>(results);
                        rm.result = p;
                        rm.SetResponse(true);

                        break;
                    case Action.UPDATE:
                        value.idproducts = id > 0 ? id : value.idproducts;
                        result = ng.Update(value);
                        rm.SetResponse(true);
                        break;
                    case Action.DELETE:
                        bool flag = ng.Delete(id);
                        break;

                    default:
                        break;
                }

                if (result.id > 0)
                {
                    ProductDTO p = Mapper.Map<ProductDTO>(result);
                    rm.result = p;
                    rm.SetResponse(true);

                }
            }
            catch (Exception ex)
            {
                rm.message = ex.Message;
            }

            return Request.CreateResponse<ResponseModel>(HttpStatusCode.OK, rm);
        }

        private HttpResponseMessage executeactionentries(Action action, int id = 0, EntryDTO value = null)
        {
            ResponseModel rm = new ResponseModel();
            PRODUCTENTRy result = new PRODUCTENTRy();
            List<PRODUCTENTRy> results = new List<PRODUCTENTRy>();

            try
            {

                switch (action)
                {
                    case Action.CREATE:
                        result = ng.CreateEntry(value);
                        break;
                    case Action.READID:
                        result = ng.ReadEntry(id: id).First();

                        break;
                    case Action.READALL:
                        results = ng.ReadEntry(all: true);
                        // rm.result = results;
                        List<ProductDTO> p = Mapper.Map<List<ProductDTO>>(results);
                        rm.result = p;
                        break;
                    case Action.UPDATE:
                        value.idproducts = id > 0 ? id : value.idproducts;
                        result = ng.UpdateEntry(value);
                        break;
                    case Action.DELETE:
                        bool flag = ng.DeleteEntry(id);
                        rm.SetResponse(flag);
                        break;

                    default:
                        break;
                }

                // if (value.idproductentries > 0)
                if (result.id > 0)
                {
                    value = new EntryDTO();
                    PRODUCTENTRYDETAIL detail = result.PRODUCTENTRYDETAILS.FirstOrDefault();
                    value.idproductentries = result.id;
                    value.idcstatus = result.idcstatus;
                    value.total = result.total;
                    value.create_date = result.create_date;
                    value.idproductentrydetails = detail.id;
                    value.unitary_cost = detail.unitary_cost;
                    value.idproducts = detail.idproducts;
                    value.existence = detail.PRODUCT.existence;
                    rm.result = value;
                    rm.SetResponse(true);
                }
                else if (results.Count > 0)
                {
                    List<EntryDTO> list = new List<EntryDTO>();
                    foreach (var item in results)
                    {
                        value = new EntryDTO();
                        PRODUCTENTRYDETAIL detail = item.PRODUCTENTRYDETAILS.FirstOrDefault();
                        value.idproductentries = item.id;
                        value.idcstatus = item.idcstatus;
                        value.total = item.total;
                        value.create_date = item.create_date;
                        value.idproductentrydetails = item.id;
                        value.unitary_cost = detail.unitary_cost;
                        value.idproducts = detail.idproducts;
                        value.existence = detail.PRODUCT.existence;
                        list.Add(value);
                    }
                    rm.result = list;
                    rm.SetResponse(true);
                }
               
            }
            catch (Exception ex)
            {
                rm.message = ex.Message;
            }

            return Request.CreateResponse<ResponseModel>(HttpStatusCode.OK, rm);
        }
    }
}
