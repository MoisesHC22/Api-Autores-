using Api.Microservice.Autor.Modelo;
using Api.Microservice.Autor.Persistencia;
using FluentValidation;
using gRPC.Autor.Serve;
using MediatR;

namespace Api.Microservice.Autor.Aplicacion
{
    public class Nuevo
    {

        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }
            public string Imagen { get; set; }
        }

        //clase para validar la clase ejecuta a traves de apifluent validator
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(p => p.Nombre).NotEmpty();
                RuleFor(p => p.Apellido).NotEmpty();
                RuleFor(p => p.Imagen).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly ContextoAutor _context;
            private readonly ILogger<Manejador> _logger;
            private readonly AutorImg.AutorImgClient _grpcClient;


            public Manejador(ContextoAutor context, ILogger<Manejador> logger, AutorImg.AutorImgClient grpcClient)
            {
                _context = context;
                _logger = logger;
                _grpcClient = grpcClient;
            }


            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //se crea la instacia de autor-libro ligada al contexto

                var builder = new BuilderEjec()
                    .setNombre(request.Nombre)
                    .setApellido(request.Apellido)
                    .setFechaNacimiento(request.FechaNacimiento)
                    .setAutorLibroGuid(Guid.NewGuid().ToString())
                    .Build();

                //agregamos el objeto del tipo autor-libro
                _context.AutorLibros.Add(builder);

                //insertamos el valor de insercion
                var respuesta = await _context.SaveChangesAsync();

                if (respuesta > 0)
                {
                    var grpcRequest = new ImgRequest
                    {
                        Id = builder.AutorLibroId.GetHashCode(),
                        Img = request.Imagen
                    };

                    var grpcResponse = await _grpcClient.GuardarImgAsync(grpcRequest);

                    if (grpcResponse.Mensaje != "La imagen se guardo exitosamente") 
                    {
                        throw new Exception("No se pudo guardar la imagen");
                    }

                    _logger.LogError("No se pudo insertar el autor del libro en la base de datos");
                    throw new Exception("No se pudo insertar el autor del libro");
                }
               return Unit.Value;
            }


        }


    }
}
