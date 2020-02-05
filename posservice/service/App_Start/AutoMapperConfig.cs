using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using posrepository.DTO;
using mrgvn.db;

namespace service
{
    public enum Action : int
    {
        CREATE = 1,
        READALL = 2,
        READID = 3,
        UPDATE = 4,
        DELETE = 5,
    }
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                //config.CreateMap<ProductDTO, PRODUCT>().
                //ForMember(y => y.id, x => x.MapFrom(src => src.idproducts)).
                //ReverseMap();

                config.CreateMap<PRODUCT, ProductDTO>().
                ForMember(y => y.idproducts, x => x.MapFrom(src => src.id)).
                ReverseMap();

                config.CreateMap<SALE, SalesDTO>().
                ForMember(x => x.Saledetails, src => src.MapFrom(x => x.SALEDETAILS)).
                ForMember(x1 => x1.idsales, src1 => src1.MapFrom(x1 => x1.id)).
                ReverseMap();

                config.CreateMap<LOSTITEM, LostItemDTO>().
                ForMember(x => x.Itemsdetails, src => src.MapFrom(x => x.LOSTITEMDETAILS)).
                ReverseMap();


                config.ValidateInlineMaps = false;
            });

            // var config = new MapperConfiguration(cfg => cfg.CreateMap<PRODUCT, ProductDTO>());

        }
    }
}