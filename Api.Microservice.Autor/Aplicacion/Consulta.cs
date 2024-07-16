using Api.Microservice.Autor.Modelo;
using Api.Microservice.Autor.Persistencia;
using AutoMapper;
using gRPC.Autor.Serve;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Microservice.Autor.Aplicacion
{
    public class Consulta
    {
        //que va a devolver la clase .. patron ETR
        public class ListaAutor: IRequest<List<AutorDto>> { }


        //recibe LitaAutor y regresa una lista de AutorLibro
        public class Manejador : IRequestHandler<ListaAutor, List<AutorDto>>
        {
            private readonly ContextoAutor _context;
            private readonly IMapper _maper;
            private readonly AutorImg.AutorImgClient _grpcClient;


            public Manejador(ContextoAutor context, IMapper mapper, AutorImg.AutorImgClient grpcClient)
            {
                this._context = context;
                this._maper = mapper;
                _grpcClient = grpcClient;
            }

            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                var autores = await _context.AutorLibros.ToListAsync();

                var autoresDto = _maper.Map<List<AutorLibro>, List<AutorDto>>(autores);


                foreach (var autor in autoresDto)
                {
                    var grpcRequest = new IdImg { Id = autor.AutorLibroId };

                    try {
                        var grpcResponse = await _grpcClient.ConsultaFiltroAsync(grpcRequest);
                        autor.Imagen = grpcResponse.Img;
                    }
                    catch (Exception ex)
                    {
                        autor.Imagen = "Imagen no disponible";
                    }
                }
                
                return autoresDto;
            }
        }
    }


}
