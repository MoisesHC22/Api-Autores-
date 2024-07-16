using Api.Microservice.Autor.Modelo;
using Api.Microservice.Autor.Persistencia;
using AutoMapper;
using gRPC.Autor.Serve;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Api.Microservice.Autor.Aplicacion
{
    public class ConsultarFiltro
    {
        public class AutorUnico : IRequest<AutorDto> { 
            public string AutorGuid { get; set; }
        }
        //recibe    / devuelve
        public class Manejador : IRequestHandler<AutorUnico, AutorDto>
        { 
        private readonly ContextoAutor _context;
            private readonly IMapper _mapper;
            private readonly AutorImg.AutorImgClient _grpcClient;

            public Manejador(ContextoAutor context, IMapper mapper, AutorImg.AutorImgClient grpcClient)
            {
                _context = context;
                _mapper = mapper;
                _grpcClient = grpcClient;
            }

            public async Task<AutorDto> Handle(AutorUnico request, CancellationToken cancellationToken)
            {
                var autor = await _context.AutorLibros
                    .Where(p => p.AutorLibroGuid == request.AutorGuid).FirstOrDefaultAsync();
              
                if (autor == null) {
                    throw new Exception("No se encontró el autor");
                }

                var grpcRequest = new IdImg { Id = autor.AutorLibroId };
                var grpcResponse = await _grpcClient.ConsultaFiltroAsync(grpcRequest);

                var builder = new BuilderEjec()
                    .setAutorLibroId(autor.AutorLibroId)
                    .setNombre(autor.Nombre)
                    .setApellido(autor.Apellido)
                    .setFechaNacimiento(autor.FechaNacimiento)
                    .setAutorLibroGuid(autor.AutorLibroGuid)
                    .setImagen(grpcResponse.Img);
               

                var autorDto = builder.BuildDto();
                return autorDto;
            }
        }
    }
}
