using Api.Microservice.Autor.Aplicacion;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Microservice.Autor.Controllers
{
    //EL MICROSERVICIO NO QUEDA EXPUESRO
        //estandar[]
    [Route("[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        //patron de diseño mediatr
        private readonly IMediator _mediator;
        /// <summary>
        /// mandamos llamar media tr y hacemos la inyeccion
        /// </summary>
        /// <param name="mediator"></param>
        public AutorController(IMediator mediator)
        {
            _mediator = mediator;
        }


        //clase. clase / la variable la toma como data
        //lo manda a una escritura d datos
        //si sALE BIEN REGRESO UN unit
        //patron abajo        
        [HttpPost, Route("Crear")]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        { 
            return await _mediator.Send(data);
        }






        //trae todos los elementos d la lista
        //quien hace la diferencia, es el patron q viene implicito
        [HttpGet, Route("GetAutores")]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            return await _mediator.Send(new Consulta.ListaAutor());
        }







        [HttpGet, Route("GetAutorLibro")]

        public async Task<ActionResult<AutorDto>> GetAutorLibro(string id)
        {
            return await _mediator.Send(new ConsultarFiltro.AutorUnico { AutorGuid = id });
        }

    }
}
