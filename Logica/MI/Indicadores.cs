using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.MI
{
    public class Indicadores
    {
        public Entidades.MI.Indicadores MostrarIndicadores()
        {
            Datos.MI.Indicadores datosIndicadores = new Datos.MI.Indicadores();
            return datosIndicadores.MostrarIndicadores();
        }
    }
}
