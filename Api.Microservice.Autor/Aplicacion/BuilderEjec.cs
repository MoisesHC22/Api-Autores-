using Api.Microservice.Autor.Modelo;

namespace Api.Microservice.Autor.Aplicacion
{
    public class BuilderEjec
    {
        private int _AutorLibroId;
        private string _nombre;
        private string _apellido;
        private DateTime? _fechaNacimiento;
        public string _AutorLibroGuid;
        public string _Imagen;


        public BuilderEjec setAutorLibroId(int autorLibroId)
        {
            _AutorLibroId = autorLibroId;
            return this;
        }

        public BuilderEjec setNombre(string nombre)
        {
            _nombre = nombre;
            return this;
        }

        public BuilderEjec setApellido(string apellido)
        {
            _apellido = apellido;
            return this;
        }

        public BuilderEjec setFechaNacimiento(DateTime? fechaNacimiento)
        {
            _fechaNacimiento = fechaNacimiento;
            return this;

        }

        public BuilderEjec setAutorLibroGuid(string autorLibroGuid)
        {
            _AutorLibroGuid = autorLibroGuid;
            return this;
        }

        public BuilderEjec setImagen(string Imagen)
        { 
           _Imagen = Imagen;
            return this;
        }

        public AutorLibro Build()
        {
            return new AutorLibro
            {
                AutorLibroId = _AutorLibroId,
                Nombre = _nombre,
                Apellido = _apellido,
                FechaNacimiento = _fechaNacimiento,
                AutorLibroGuid = _AutorLibroGuid
            };
        }

        public AutorDto BuildDto()
        {
            return new AutorDto
            {
                AutorLibroId = _AutorLibroId,
                Nombre = _nombre,
                Apellido = _apellido,
                FechaNacimiento = _fechaNacimiento,
                AutorLibroGuid = _AutorLibroGuid,
                Imagen = _Imagen
            };
        }


        public List<AutorDto> BuildList(List<AutorLibro> autores) 
        {
            return autores.Select(autor => new AutorDto
            {
                AutorLibroId = autor.AutorLibroId,
                Nombre = autor.Nombre,
                Apellido = autor.Apellido,
                FechaNacimiento = autor.FechaNacimiento,
                AutorLibroGuid = autor.AutorLibroGuid
            }).ToList();
        }



    }
}
