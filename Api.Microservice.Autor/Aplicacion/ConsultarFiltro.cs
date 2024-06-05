using Api.Microservice.Autor.Modelo;
using Api.Microservice.Autor.Persistencia;
using AutoMapper;
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

            public Manejador(ContextoAutor context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<AutorDto> Handle(AutorUnico request, CancellationToken cancellationToken)
            {
                var autor = await _context.AutorLibros
                    .Where(p => p.AutorLibroGuid == request.AutorGuid).FirstOrDefaultAsync();
              
                if (autor == null) {
                    throw new Exception("No se encontró el autor");
                }

                var builder = new BuilderEjec()
                    .setAutorLibroId(autor.AutorLibroId)
                    .setNombre(autor.Nombre)
                    .setApellido(autor.Apellido)
                    .setFechaNacimiento(autor.FechaNacimiento)
                    .setAutorLibroGuid(autor.AutorLibroGuid);

                var autorDto = builder.BuildDto();
                return autorDto;
            }
        }
    }
}
