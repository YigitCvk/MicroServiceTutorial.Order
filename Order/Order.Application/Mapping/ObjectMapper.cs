using AutoMapper;

namespace Order.Application.Mapping
{
    public static class ObjectMapper
    {
        //istendiği takdirde init edilir
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {

            });

            return config.CreateMapper();
        });

        public static IMapper Mapper => lazy.Value;
    }
}
