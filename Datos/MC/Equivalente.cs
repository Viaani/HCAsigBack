using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Datos.MC
{
    public class Equivalente
    {
        Entidades.TipoRetorno TipoRetorno = new Entidades.TipoRetorno();

        public string[] AgregarEquivalenteConvalidacion(Entidades.MC.Equivalentes equivalentes)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "BEGIN;";
                foreach(Entidades.MC.Equivalente equivalente in equivalentes.ListaEquivalente)
                {
                    query += "INSERT INTO equivalente(`programaOrigen`,`programaObjetivo`,`codigoAsignaturaOrigen`,`codigoAsignaturaObjetivo`) VALUES('"+equivalente.programaOrigen+"', '"+equivalente.programaObjetivo+"', '"+equivalente.codigoAsignaturaOrigen+"', '"+equivalente.codigoAsignaturaObjetivo+"'); ";
                }
                query +="COMMIT;";
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();

                conexion.CommitTransaction();
                return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
            }

            catch (Exception e)
            {
                if (e.ToString().Contains("Duplicate"))
                {
                    return new string[] { "duplicado", this.TipoRetorno.error.ToString() };
                }
                conexion.RollBack();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }

        }

        public string[] AgregarEquivalenteHomologacion(Entidades.MC.Equivalentes equivalentes)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "BEGIN;";
                foreach (Entidades.MC.Equivalente equivalente in equivalentes.ListaEquivalente)
                {
                    query += "INSERT INTO equivalenteHomologacion(`programaOrigen`,`programaObjetivo`,`codigoAsignaturaOrigen`,`codigoAsignaturaObjetivo`) VALUES('" + equivalente.programaOrigen + "', '" + equivalente.programaObjetivo + "', '" + equivalente.codigoAsignaturaOrigen + "', '" + equivalente.codigoAsignaturaObjetivo + "'); ";
                }
                query += "COMMIT;";
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();

                conexion.CommitTransaction();
                return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
            }

            catch (Exception e)
            {
                if (e.ToString().Contains("Duplicate"))
                {
                    return new string[] { "duplicado", this.TipoRetorno.error.ToString() };
                }
                conexion.RollBack();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }

        }

        public List<Entidades.MC.AsignaturasEquivalentes> BuscarEquivalenteConvalidacion(string programaOrigen, string programaObjetivo)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "(SELECT " +
                                        "programaOrigen, programaObjetivo, codigoAsignaturaOrigen, codigoAsignaturaObjetivo " +
                                        "FROM   equivalente WHERE(programaOrigen = '" + programaOrigen + "') and(programaObjetivo = '" + programaObjetivo + "')) " +
                               "order by codigoAsignaturaOrigen, codigoAsignaturaObjetivo;";

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MC.Equivalente> equivalentes = new List<Entidades.MC.Equivalente>();

                List<String> codigosAsignaturas = new List<string>();

                while (reader.Read())
                {
                    Entidades.MC.Equivalente equivalente = new Entidades.MC.Equivalente();

                    equivalente.programaOrigen = reader["programaOrigen"].ToString();
                    equivalente.programaObjetivo = reader["programaObjetivo"].ToString();
                    equivalente.codigoAsignaturaOrigen = reader["codigoAsignaturaOrigen"].ToString();
                    equivalente.codigoAsignaturaObjetivo = reader["codigoAsignaturaObjetivo"].ToString();

                    codigosAsignaturas.Add(reader["codigoAsignaturaOrigen"].ToString());
                    codigosAsignaturas.Add(reader["codigoAsignaturaObjetivo"].ToString());
                    equivalentes.Add(equivalente);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();

                if (codigosAsignaturas.Count == 0)
                {
                    List<Entidades.MC.AsignaturasEquivalentes> vacio = new List<Entidades.MC.AsignaturasEquivalentes>();
                    return vacio;
                }

                conexion.AbrirConexion();
                conexion.BeginTransaction();

                query = "SELECT * FROM asignaturas WHERE ";
                int cantidadCodigosAsignaturas = codigosAsignaturas.Count - 1;
                int contador = 0;
                foreach (String codigo in codigosAsignaturas)
                {
                    if (contador == cantidadCodigosAsignaturas)
                    {
                        query += " codigo = '" + codigo + "' ;";
                    }
                    else
                    {
                        query += " codigo = '" + codigo + "' or ";
                    }
                    contador++;
                }

                commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                reader = commandDatabase.ExecuteReader();
                List<Entidades.MDP.Asignatura> asignaturas = new List<Entidades.MDP.Asignatura>();


                while (reader.Read())
                {
                    Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                    asignatura.Codigo = reader["codigo"].ToString();
                    asignatura.Nombre = reader["nombre"].ToString();
                    asignatura.Creditos = Convert.ToInt32(reader["creditos"]);

                    asignaturas.Add(asignatura);
                }

                List<Entidades.MC.AsignaturasEquivalentes> ListaAsignaturasEquivalentes = new List<Entidades.MC.AsignaturasEquivalentes>();
                int cantidadEquivalentes = equivalentes.Count;
                // recorre el arreglo equivalentes
                for(int cont = 0; cont < cantidadEquivalentes; cont++)
                {
                    // Hay 3 casos
                    // si el siguiente existe
                    if(cont+1 < cantidadEquivalentes) {
                        // si el siguiente CodigoAsignaturaOrigen es igual
                        if(equivalentes[cont].codigoAsignaturaOrigen == equivalentes[cont + 1].codigoAsignaturaOrigen)
                        {
                            //revisa si hay mas de un CodigoAsignaturaOrigen Seguido
                            for (int cont2 = cont; cont2 < cantidadEquivalentes; cont2++)
                            {
                                // si el siguiente no es el mismo CodigoAsignaturaOrigen O no existe
                                if (cont2 + 1 >= cantidadEquivalentes)
                                {
                                    int cantidadAsignaturasSeguidas = cont2 - cont + 1;
                                    // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                    Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                    asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                    asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                    //recorre hacia atras para guardar CodigosOrigen
                                    //recorre la lista se asignaturas buscando el igual para guardarlo
                                    foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                    {
                                        if (asig.Codigo == equivalentes[cont2].codigoAsignaturaOrigen)
                                        {
                                            asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                            break;
                                        }
                                    }
                                    //recorre hacia atras para guardar CodigosObjetivo
                                    for (int temp = cont2; temp > cont2 - cantidadAsignaturasSeguidas; temp--)
                                    {
                                        //recorre la lista se asignaturas buscando el igual para guardarlo
                                        foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                        {
                                            if (asig.Codigo == equivalentes[temp].codigoAsignaturaObjetivo)
                                            {
                                                asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                                break;
                                            }
                                        }
                                    }
                                    ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                    cont += (cantidadAsignaturasSeguidas) - 1;
                                    break;
                                }
                                if (equivalentes[cont2].codigoAsignaturaOrigen != equivalentes[cont2 + 1].codigoAsignaturaOrigen)
                                {
                                    int cantidadAsignaturasSeguidas = cont2 - cont + 1;
                                    // revisa cuantas veces se repite el patron de codigos
                                    for (int cont3 = cont2+1; cont3 <= cantidadEquivalentes; cont3 += cantidadAsignaturasSeguidas)
                                    {   
                                        //cuando el siguiente no existe
                                        if (cont3 == cantidadEquivalentes)
                                        {
                                            int cantidadAsignaturasSeguidasRepetidas = ((cont3-cont) / cantidadAsignaturasSeguidas);
                                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                            Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                            Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                                            //recorre hacia atras para guardar CodigosOrigen
                                            for (int temp = cont3 - 1; temp > cont; temp = temp - cantidadAsignaturasSeguidas)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                                    {
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                                        break;
                                                    }
                                                }

                                            }
                                            //recorre hacia atras para guardar CodigosObjetivo
                                            for (int temp = cont3 - 1; temp > cont3 - 1 - cantidadAsignaturasSeguidas; temp--)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaObjetivo)
                                                    {
                                                        asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                                        break;
                                                    }
                                                }
                                            }
                                            ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                            cont += (cantidadAsignaturasSeguidas * cantidadAsignaturasSeguidasRepetidas) - 1;
                                            break;
                                        }
                                        //cuando no se repite
                                        if(equivalentes[cont3].codigoAsignaturaObjetivo != equivalentes[cont3 - cantidadAsignaturasSeguidas].codigoAsignaturaObjetivo)
                                        {
                                            int cantidadAsignaturasSeguidasRepetidas = ((cont3-cont)/cantidadAsignaturasSeguidas);
                                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                            Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                            Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                                            //recorre hacia atras para guardar CodigosOrigen
                                            for (int temp = cont3-1; temp > cont ; temp =  temp - cantidadAsignaturasSeguidas)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                                    {
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                                        break;
                                                    }
                                                }

                                            }
                                            //recorre hacia atras para guardar CodigosObjetivo
                                            for (int temp = cont3 - 1; temp > cont3 - 1 - cantidadAsignaturasSeguidas; temp--)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaObjetivo)
                                                    {
                                                        asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                                        break;
                                                    }
                                                }
                                            }
                                            ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                            cont += (cantidadAsignaturasSeguidas * cantidadAsignaturasSeguidasRepetidas) - 1;
                                            break;
                                        }
                                    
                                    }
                                    break;
                                }
                            }
                        }
                    
                        // si el siguiente CodigoAsignaturaObjetivo es igual
                        else if(equivalentes[cont].codigoAsignaturaObjetivo == equivalentes[cont + 1].codigoAsignaturaObjetivo)
                        {
                            //revisa si hay mas de un CodigoAsignaturaObjetivo Seguido
                            for (int cont2 = cont; cont2 < cantidadEquivalentes; cont2++)
                            {
                                // si el siguiente no es el mismo CodigoAsignaturaObjetivo O no existe
                                if (cont2 + 1 >= cantidadEquivalentes)
                                {
                                    int cantidadAsignaturasSeguidas = cont2 - cont + 1;
                                    // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                    Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                    asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                    asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                    //recorre la lista se asignaturas buscando el igual para guardarlo
                                    foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                    {
                                        if (asig.Codigo == equivalentes[cont2].codigoAsignaturaObjetivo)
                                        {
                                            asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                            break;
                                        }
                                    }

                                    //recorre hacia atras para guardar CodigosOrigen
                                    for (int temp = cont2; temp > cont2 - cantidadAsignaturasSeguidas; temp--)
                                    {
                                        //recorre la lista se asignaturas buscando el igual para guardarlo
                                        foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                        {
                                            if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                            {
                                                asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                                break;
                                            }
                                        }
                                    }
                                    ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                    cont += (cantidadAsignaturasSeguidas) - 1;
                                    break;
                                }

                                if (equivalentes[cont2].codigoAsignaturaObjetivo != equivalentes[cont2 + 1].codigoAsignaturaObjetivo)
                                {
                                    int cantidadAsignaturasSeguidas = cont2 - cont + 1;
                                    // revisa cuantas vese se repite el patron de codigos
                                    for (int cont3 = cont2 + 1; cont3 <= cantidadEquivalentes; cont3 += cantidadAsignaturasSeguidas)
                                    {
                                        //cuando no existe siguiente
                                        if (cont3 == cantidadEquivalentes)
                                        {
                                            int cantidadAsignaturasSeguidasRepetidas = ((cont3 - cont) / cantidadAsignaturasSeguidas);
                                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                            Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                            Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                                            //recorre hacia atras para guardar CodigosObjetivos
                                            for (int temp = cont3 - 1; temp > cont; temp = temp - cantidadAsignaturasSeguidas)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaObjetivo)
                                                    {
                                                        asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                                        break;
                                                    }
                                                }

                                            }
                                            //recorre hacia atras para guardar CodigosOrigen
                                            for (int temp = cont3 - 1; temp > cont3 - 1 - cantidadAsignaturasSeguidas; temp--)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                                    {
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                                        break;
                                                    }
                                                }
                                            }
                                            ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                            cont += (cantidadAsignaturasSeguidas * cantidadAsignaturasSeguidasRepetidas) - 1;
                                            break;
                                        }
                                        //cuando no se repite
                                        if (equivalentes[cont3].codigoAsignaturaOrigen != equivalentes[cont3 - cantidadAsignaturasSeguidas].codigoAsignaturaOrigen)
                                        {
                                            int cantidadAsignaturasSeguidasRepetidas = ((cont3-cont) / cantidadAsignaturasSeguidas);
                                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                            Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                            Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                                            //recorre hacia atras para guardar CodigosObjetivos
                                            for (int temp = cont3 - 1; temp > cont; temp = temp - cantidadAsignaturasSeguidas)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaObjetivo)
                                                    {
                                                        asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                                        break;
                                                    }
                                                }

                                            }
                                            //recorre hacia atras para guardar CodigosOrigen
                                            for (int temp = cont3 - 1; temp > cont3 - 1 - cantidadAsignaturasSeguidas; temp--)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                                    {
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                                        break;
                                                    }
                                                }
                                            }
                                            ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                            cont += (cantidadAsignaturasSeguidas * cantidadAsignaturasSeguidasRepetidas) - 1;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        // si el siquiente existe y ambos son diferentes
                        else
                        {
                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                            Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                            Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();
                            int numeroAgregado = 0;
                            foreach (Entidades.MDP.Asignatura asig in asignaturas)
                            {
                                if (asig.Codigo == equivalentes[cont].codigoAsignaturaOrigen)
                                {
                                    asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                    numeroAgregado++;
                                }
                                if (asig.Codigo == equivalentes[cont].codigoAsignaturaObjetivo)
                                {
                                    asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                    numeroAgregado++;
                                }
                                if (numeroAgregado == 2)
                                {
                                    break;
                                }
                            }
                            ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                        }
                    }

                    // si el siquiente si ambos son diferentes
                    else
                    {
                        // se agregar las equivalencia a la lista de asigntaruas equivalentes
                        Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                        asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                        asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                        Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();
                        int numeroAgregado = 0;
                        foreach(Entidades.MDP.Asignatura asig in asignaturas)
                        {
                            if(asig.Codigo == equivalentes[cont].codigoAsignaturaOrigen)
                            {
                                asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                numeroAgregado++;
                            }
                            if (asig.Codigo == equivalentes[cont].codigoAsignaturaObjetivo)
                            {
                                asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                numeroAgregado++;
                            }
                            if(numeroAgregado == 2)
                            {
                                break;
                            }
                        }
                        ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                    }
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return ListaAsignaturasEquivalentes;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public List<Entidades.MC.AsignaturasEquivalentes> BuscarEquivalenteHomologacion(string programaOrigen, string programaObjetivo)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "(SELECT " +
                                        "programaOrigen, programaObjetivo, codigoAsignaturaOrigen, codigoAsignaturaObjetivo " +
                                        "FROM   equivalenteHomologacion WHERE(programaOrigen = '" + programaOrigen + "') and(programaObjetivo = '" + programaObjetivo + "')) " +
                               "order by codigoAsignaturaOrigen, codigoAsignaturaObjetivo;";

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MC.Equivalente> equivalentes = new List<Entidades.MC.Equivalente>();

                List<String> codigosAsignaturas = new List<string>();

                while (reader.Read())
                {
                    Entidades.MC.Equivalente equivalente = new Entidades.MC.Equivalente();

                    equivalente.programaOrigen = reader["programaOrigen"].ToString();
                    equivalente.programaObjetivo = reader["programaObjetivo"].ToString();
                    equivalente.codigoAsignaturaOrigen = reader["codigoAsignaturaOrigen"].ToString();
                    equivalente.codigoAsignaturaObjetivo = reader["codigoAsignaturaObjetivo"].ToString();

                    codigosAsignaturas.Add(reader["codigoAsignaturaOrigen"].ToString());
                    codigosAsignaturas.Add(reader["codigoAsignaturaObjetivo"].ToString());
                    equivalentes.Add(equivalente);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();

                if(codigosAsignaturas.Count == 0)
                {
                    List<Entidades.MC.AsignaturasEquivalentes> vacio = new List<Entidades.MC.AsignaturasEquivalentes>();
                    return vacio;
                }

                conexion.AbrirConexion();
                conexion.BeginTransaction();

                query = "SELECT * FROM asignaturas WHERE ";
                int cantidadCodigosAsignaturas = codigosAsignaturas.Count - 1;
                int contador = 0;
                foreach (String codigo in codigosAsignaturas)
                {
                    if (contador == cantidadCodigosAsignaturas)
                    {
                        query += " codigo = '" + codigo + "' ;";
                    }
                    else
                    {
                        query += " codigo = '" + codigo + "' or ";
                    }
                    contador++;
                }

                commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                reader = commandDatabase.ExecuteReader();
                List<Entidades.MDP.Asignatura> asignaturas = new List<Entidades.MDP.Asignatura>();


                while (reader.Read())
                {
                    Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                    asignatura.Codigo = reader["codigo"].ToString();
                    asignatura.Nombre = reader["nombre"].ToString();
                    asignatura.Creditos = Convert.ToInt32(reader["creditos"]);

                    asignaturas.Add(asignatura);
                }

                List<Entidades.MC.AsignaturasEquivalentes> ListaAsignaturasEquivalentes = new List<Entidades.MC.AsignaturasEquivalentes>();
                int cantidadEquivalentes = equivalentes.Count;
                // recorre el arreglo equivalentes
                for (int cont = 0; cont < cantidadEquivalentes; cont++)
                {
                    // Hay 3 casos
                    // si el siguiente existe
                    if (cont + 1 < cantidadEquivalentes)
                    {
                        // si el siguiente CodigoAsignaturaOrigen es igual
                        if (equivalentes[cont].codigoAsignaturaOrigen == equivalentes[cont + 1].codigoAsignaturaOrigen)
                        {
                            //revisa si hay mas de un CodigoAsignaturaOrigen Seguido
                            for (int cont2 = cont; cont2 < cantidadEquivalentes; cont2++)
                            {
                                // si el siguiente no es el mismo CodigoAsignaturaOrigen o no existe
                                if (cont2 + 1 >= cantidadEquivalentes)
                                {
                                    int cantidadAsignaturasSeguidas = cont2 - cont + 1;
                                    // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                    Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                    asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                    asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                    Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                                    //recorre hacia atras para guardar CodigosOrigen
                                    //recorre la lista se asignaturas buscando el igual para guardarlo
                                    foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                    {
                                        if (asig.Codigo == equivalentes[cont2].codigoAsignaturaOrigen)
                                        {
                                            asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                            break;
                                        }
                                    }
                                    //recorre hacia atras para guardar CodigosObjetivo
                                    for (int temp = cont2; temp > cont2 - cantidadAsignaturasSeguidas; temp--)
                                    {
                                        //recorre la lista se asignaturas buscando el igual para guardarlo
                                        foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                        {
                                            if (asig.Codigo == equivalentes[temp].codigoAsignaturaObjetivo)
                                            {
                                                asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                                break;
                                            }
                                        }
                                    }
                                    ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                    cont += (cantidadAsignaturasSeguidas) - 1;
                                    break;
                                }
                                if (equivalentes[cont2].codigoAsignaturaOrigen != equivalentes[cont2 + 1].codigoAsignaturaOrigen)
                                {
                                    int cantidadAsignaturasSeguidas = cont2 - cont + 1;
                                    // revisa cuantas vese se repite el patron de codigos
                                    for (int cont3 = cont2 + 1; cont3 <= cantidadEquivalentes; cont3 += cantidadAsignaturasSeguidas)
                                    {
                                        //cuando no existe siguiente
                                        if (cont3 == cantidadEquivalentes)
                                        {
                                            int cantidadAsignaturasSeguidasRepetidas = ((cont3 - cont) / cantidadAsignaturasSeguidas);
                                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                            Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                            Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                                            //recorre hacia atras para guardar CodigosOrigen
                                            for (int temp = cont3 - 1; temp > cont; temp = temp - cantidadAsignaturasSeguidas)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                                    {
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                                        break;
                                                    }
                                                }

                                            }
                                            //recorre hacia atras para guardar CodigosObjetivo
                                            for (int temp = cont3 - 1; temp > cont3 - 1 - cantidadAsignaturasSeguidas; temp--)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaObjetivo)
                                                    {
                                                        asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                                        break;
                                                    }
                                                }
                                            }
                                            ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                            cont += (cantidadAsignaturasSeguidas * cantidadAsignaturasSeguidasRepetidas) - 1;
                                            break;
                                        }
                                        //cuando no se repite
                                        if (equivalentes[cont3].codigoAsignaturaObjetivo != equivalentes[cont3 - cantidadAsignaturasSeguidas].codigoAsignaturaObjetivo)
                                        {
                                            int cantidadAsignaturasSeguidasRepetidas = ((cont3-cont) / cantidadAsignaturasSeguidas);
                                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                            Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                            Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                                            //recorre hacia atras para guardar CodigosOrigen
                                            for (int temp = cont3 - 1; temp > cont; temp = temp - cantidadAsignaturasSeguidas)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                                    {
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                                        break;
                                                    }
                                                }

                                            }
                                            //recorre hacia atras para guardar CodigosObjetivo
                                            for (int temp = cont3 - 1; temp > cont3 - 1 - cantidadAsignaturasSeguidas; temp--)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaObjetivo)
                                                    {
                                                        asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                                        break;
                                                    }
                                                }
                                            }
                                            ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                            cont += (cantidadAsignaturasSeguidas * cantidadAsignaturasSeguidasRepetidas) - 1;
                                            break;
                                        }

                                    }
                                    break;
                                }
                            }
                        }

                        // si el siguiente CodigoAsignaturaObjetivo es igual
                        else if (equivalentes[cont].codigoAsignaturaObjetivo == equivalentes[cont + 1].codigoAsignaturaObjetivo)
                        {
                            //revisa si hay mas de un CodigoAsignaturaObjetivo Seguido
                            for (int cont2 = cont; cont2 < cantidadEquivalentes; cont2++)
                            {
                                // si el siguiente no es el mismo CodigoAsignaturaObjetivo O no existe
                                if (cont2 + 1 >= cantidadEquivalentes)
                                {
                                    int cantidadAsignaturasSeguidas = cont2 - cont + 1;
                                    // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                    Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                    asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                    asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                    Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();
                                    
                                    //recorre la lista se asignaturas buscando el igual para guardarlo
                                    foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                    {
                                        if (asig.Codigo == equivalentes[cont2].codigoAsignaturaObjetivo)
                                        {
                                            asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                            break;
                                        }
                                    }

                                            
                                    //recorre hacia atras para guardar CodigosOrigen
                                    for (int temp = cont2; temp > cont2 - cantidadAsignaturasSeguidas; temp--)
                                    {
                                        //recorre la lista se asignaturas buscando el igual para guardarlo
                                        foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                        {
                                            if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                            {
                                                asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                                break;
                                            }
                                        }
                                    }
                                    ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                    cont += (cantidadAsignaturasSeguidas) - 1;
                                    break;
                                    
                                }

                                if (equivalentes[cont2].codigoAsignaturaObjetivo != equivalentes[cont2 + 1].codigoAsignaturaObjetivo)
                                {
                                    int cantidadAsignaturasSeguidas = cont2 - cont + 1;
                                    // revisa cuantas vese se repite el patron de codigos
                                    for (int cont3 = cont2 + 1; cont3 <= cantidadEquivalentes; cont3 += cantidadAsignaturasSeguidas)
                                    {
                                        //cuando no existe siguiente
                                        if (cont3 == cantidadEquivalentes)
                                        {
                                            int cantidadAsignaturasSeguidasRepetidas = ((cont3 - cont) / cantidadAsignaturasSeguidas);
                                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                            Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                            Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                                            //recorre hacia atras para guardar CodigosObjetivos
                                            for (int temp = cont3 - 1; temp > cont; temp = temp - cantidadAsignaturasSeguidas)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaObjetivo)
                                                    {
                                                        asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                                        break;
                                                    }
                                                }

                                            }
                                            //recorre hacia atras para guardar CodigosOrigen
                                            for (int temp = cont3 - 1; temp > cont3 - 1 - cantidadAsignaturasSeguidas; temp--)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                                    {
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                                        break;
                                                    }
                                                }
                                            }
                                            ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                            cont += (cantidadAsignaturasSeguidas * cantidadAsignaturasSeguidasRepetidas) - 1;
                                            break;
                                        }
                                        //cuando no se repite
                                        if (equivalentes[cont3].codigoAsignaturaOrigen != equivalentes[cont3 - cantidadAsignaturasSeguidas].codigoAsignaturaOrigen)
                                        {
                                            int cantidadAsignaturasSeguidasRepetidas = ((cont3-cont) / cantidadAsignaturasSeguidas);
                                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                            Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                            Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                                            //recorre hacia atras para guardar CodigosObjetivos
                                            for (int temp = cont3 - 1; temp > cont; temp = temp - cantidadAsignaturasSeguidas)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaObjetivo)
                                                    {
                                                        asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                                        break;
                                                    }
                                                }

                                            }
                                            //recorre hacia atras para guardar CodigosOrigen
                                            for (int temp = cont3 - 1; temp > cont3 - 1 - cantidadAsignaturasSeguidas; temp--)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                                    {
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                                        break;
                                                    }
                                                }
                                            }
                                            ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                                            cont += (cantidadAsignaturasSeguidas * cantidadAsignaturasSeguidasRepetidas) - 1;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        // si el siquiente si ambos son diferentes
                        else
                        {
                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                            Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                            Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();
                            int numeroAgregado = 0;
                            foreach (Entidades.MDP.Asignatura asig in asignaturas)
                            {
                                if (asig.Codigo == equivalentes[cont].codigoAsignaturaOrigen)
                                {
                                    asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                    numeroAgregado++;
                                }
                                if (asig.Codigo == equivalentes[cont].codigoAsignaturaObjetivo)
                                {
                                    asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                    numeroAgregado++;
                                }
                                if (numeroAgregado == 2)
                                {
                                    break;
                                }
                            }
                            ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                        }
                    }

                    // si el siquiente si ambos son diferentes
                    else
                    {
                        // se agregar las equivalencia a la lista de asigntaruas equivalentes
                        Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentes();
                        asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.Asignatura>();
                        asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                        Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();
                        int numeroAgregado = 0;
                        foreach (Entidades.MDP.Asignatura asig in asignaturas)
                        {
                            if (asig.Codigo == equivalentes[cont].codigoAsignaturaOrigen)
                            {
                                asignaturasEquivalentes.asignaturasOrigen.Add(asig);
                                numeroAgregado++;
                            }
                            if (asig.Codigo == equivalentes[cont].codigoAsignaturaObjetivo)
                            {
                                asignaturasEquivalentes.asignaturasObjetivo.Add(asig);
                                numeroAgregado++;
                            }
                            if (numeroAgregado == 2)
                            {
                                break;
                            }
                        }
                        ListaAsignaturasEquivalentes.Add(asignaturasEquivalentes);
                    }
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return ListaAsignaturasEquivalentes;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public string[] EliminarEquivalenteConvalidacion(List<Entidades.MC.Equivalente> listaEquivalente)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "DELETE FROM equivalente WHERE ";
                int cantidadEquivalentes = listaEquivalente.Count - 1;
                int contador = 0;
                foreach (Entidades.MC.Equivalente equivalente in listaEquivalente)
                {
                    if( contador == cantidadEquivalentes)
                    {
                        query += " ( programaOrigen = '" + equivalente.programaOrigen +
                                "' and programaObjetivo = '" + equivalente.programaObjetivo +
                                "' and codigoAsignaturaOrigen = '" + equivalente.codigoAsignaturaOrigen +
                                "' and codigoAsignaturaObjetivo = '" + equivalente.codigoAsignaturaObjetivo + "');";
                    }
                    else
                    {
                        query +=" ( programaOrigen = '" + equivalente.programaOrigen+ 
                                "' and programaObjetivo = '" + equivalente.programaObjetivo+
                                "' and codigoAsignaturaOrigen = '" + equivalente.codigoAsignaturaOrigen+
                                "' and codigoAsignaturaObjetivo = '" + equivalente.codigoAsignaturaObjetivo+"') or ";
                    }
                    contador++;
                }

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                commandDatabase.ExecuteReader();

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }

        }

        public string[] EliminarEquivalenteHomologacion(List<Entidades.MC.Equivalente> listaEquivalente)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "DELETE FROM equivalenteHomologacion WHERE ";
                int cantidadEquivalentes = listaEquivalente.Count - 1;
                int contador = 0;
                foreach (Entidades.MC.Equivalente equivalente in listaEquivalente)
                {
                    if (contador == cantidadEquivalentes)
                    {
                        query += " ( programaOrigen = '" + equivalente.programaOrigen +
                                "' and programaObjetivo = '" + equivalente.programaObjetivo +
                                "' and codigoAsignaturaOrigen = '" + equivalente.codigoAsignaturaOrigen +
                                "' and codigoAsignaturaObjetivo = '" + equivalente.codigoAsignaturaObjetivo + "');";
                    }
                    else
                    {
                        query += " ( programaOrigen = '" + equivalente.programaOrigen +
                                "' and programaObjetivo = '" + equivalente.programaObjetivo +
                                "' and codigoAsignaturaOrigen = '" + equivalente.codigoAsignaturaOrigen +
                                "' and codigoAsignaturaObjetivo = '" + equivalente.codigoAsignaturaObjetivo + "') or ";
                    }
                    contador++;
                }

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                commandDatabase.ExecuteReader();

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }

        }
    }
}
