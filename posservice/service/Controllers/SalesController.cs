using AutoMapper;
using mrgvn.db;
using Newtonsoft.Json;
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
    public class SalesController : ApiController
    {
        private ISales ng;

        public SalesController(ISales sales)
        {
            this.ng = sales;
        }

        // PUT: api/sales/id
        public HttpResponseMessage Put(int id, [FromBody]SalesDTO value)
        {
            return executeaction(Action.UPDATE, id, value);
        }

        // GET: api/sales
        public HttpResponseMessage Get()
        {
            return executeaction(Action.READALL);
        }


        // DELETE: api/sales/id
        public HttpResponseMessage Delete(int id)
        {
            return executeaction(Action.DELETE, id: id);
        }
       
        // GET: api/sales/id
        public HttpResponseMessage Get(int id)
        {
            return executeaction(Action.READID, id: id);
        }

        // POST: api/sales
        public HttpResponseMessage Post([FromBody]SalesDTO value)
        {
            return executeaction(Action.CREATE, value: value);
        }

        private HttpResponseMessage executeaction(Action action, int id = 0, SalesDTO value = null)
        {
            ResponseModel rm = new ResponseModel();
            SALE result = new SALE();

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
                        List<SalesDTO> p = Mapper.Map<List<SalesDTO>>(results);
                        rm.result = p;
                        rm.SetResponse(true);
                        break;
                    
                    case Action.UPDATE:
                        value.idsales = id > 0 ? id : value.idsales;
                        result = ng.Update(value);
                        rm.SetResponse(true);
                        break;

                    case Action.DELETE:
                        bool flag = ng.Delete(id);
                        rm.SetResponse(flag);
                        break;
                    default:
                        break;
                }

                if (result.id > 0)
                {
                    SalesDTO p = Mapper.Map<SalesDTO>(result);
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


    }
}
