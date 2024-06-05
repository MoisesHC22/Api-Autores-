using Api.Microservice.Autor.Modelo;
using Api.Microservice.Autor.Persistencia;
using AutoMapper;
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


            public Manejador(ContextoAutor context, IMapper mapper)
            {
                this._context = context;
                this._maper = mapper;
            }

            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                var autores = await _context.AutorLibros.ToListAsync();
                var builder = new BuilderEjec();

                var autoresDto = builder.BuildList(autores);
                return autoresDto;
            }
        }
    }


}
