﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MySql.Data.MySqlClient;

namespace Datos.MC
{
    public class Homologacion
    {
        Entidades.TipoRetorno TipoRetorno = new Entidades.TipoRetorno();

        public string[] AgregarHomologacion(Entidades.MC.ConvalidacionHomologacion homologacion, List<Entidades.MC.ConvalidacionHomologacionEquivalenteNota> homologacionEquivalentes)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = " BEGIN; " +
                " INSERT INTO homologacion(`run`,`nombre`,`apellido`,`programaOrigen`,`programaObjetivo`,`emailSecretario`) VALUES ('" + homologacion.run + "', '" + homologacion.nombre + "', '" + homologacion.apellido + "','" + homologacion.programaOrigen + "', '" + homologacion.programaObjetivo +"','"+ homologacion.emailSecretario + "'); ";
                foreach(Entidades.MC.ConvalidacionHomologacionEquivalenteNota homologacionEquivalente in homologacionEquivalentes)
                {
                    query += " INSERT INTO homologacion_equivalenteHomologacion(`run`,`programaOrigen`,`programaObjetivo`,`codigoAsignaturaOrigen`,`codigoAsignaturaObjetivo`,`nota`) VALUES ('" + homologacionEquivalente.run+"','"+ homologacionEquivalente.programaOrigen+"','"+ homologacionEquivalente.programaObjetivo+ "','" + homologacionEquivalente.codigoAsignaturaOrigen + "','" + homologacionEquivalente.codigoAsignaturaObjetivo + "','"+ homologacionEquivalente.nota.ToString(CultureInfo.InvariantCulture) + "');";
                }
                query += " COMMIT; ";
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

        public List<Entidades.MC.RetornoHomologacion> MostrarHomologacion()
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "select h.run, h.nombre, h.apellido, h.programaOrigen, h.programaObjetivo, "+
                               "p.nombre as nombrePrograma, p.numero_decreto, "+
                               "pe.nombre as nombreProgramaExterno, pe.universidad "+
                               "from homologacion as h, programas as p, programaexterno as pe "+
                               "where h.programaObjetivo = p.codigo and h.programaOrigen = pe.codigo;";

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MC.RetornoHomologacion> homologaciones = new List<Entidades.MC.RetornoHomologacion>();

                while (reader.Read())
                {
                    Entidades.MC.RetornoHomologacion homologacion = new Entidades.MC.RetornoHomologacion();

                    homologacion.run = reader["run"].ToString();
                    homologacion.nombre = reader["nombre"].ToString();
                    homologacion.apellido = reader["apellido"].ToString();
                    homologacion.programaOrigen = new Entidades.MDP.ProgramaExterno();
                    homologacion.programaObjetivo = new Entidades.MDP.Programa();
                    homologacion.programaOrigen.Codigo = reader["programaOrigen"].ToString();
                    homologacion.programaOrigen.Universidad = reader["universidad"].ToString();
                    homologacion.programaOrigen.Nombre = reader["nombreProgramaExterno"].ToString();
                    homologacion.programaObjetivo.Codigo = reader["programaObjetivo"].ToString();
                    homologacion.programaObjetivo.Nombre = reader["nombrePrograma"].ToString();
                    homologacion.programaObjetivo.Numero_Decreto = Convert.ToInt32(reader["numero_decreto"]);


                    homologaciones.Add(homologacion);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return homologaciones;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public List<Entidades.MC.AsignaturasEquivalentesNota> MostrarHomologacion_equivalente(string programaOrigen, string programaObjetivo , string run)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();
            try
            {
                string query = "(SELECT " +
                                        "programaOrigen, programaObjetivo, codigoAsignaturaOrigen, codigoAsignaturaObjetivo, nota " +
                                        "FROM homologacion_equivalenteHomologacion WHERE(programaOrigen = '" + programaOrigen + "') and(programaObjetivo = '" + programaObjetivo + "') and (run = '" + run + "')) " +
                                "order by codigoAsignaturaOrigen, codigoAsignaturaObjetivo;";

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MC.EquivalenteNota> equivalentes = new List<Entidades.MC.EquivalenteNota>();

                List<String> codigosAsignaturas = new List<string>();

                while (reader.Read())
                {
                    Entidades.MC.EquivalenteNota equivalente = new Entidades.MC.EquivalenteNota();

                    equivalente.programaOrigen = reader["programaOrigen"].ToString();
                    equivalente.programaObjetivo = reader["programaObjetivo"].ToString();
                    equivalente.codigoAsignaturaOrigen = reader["codigoAsignaturaOrigen"].ToString();
                    equivalente.codigoAsignaturaObjetivo = reader["codigoAsignaturaObjetivo"].ToString();
                    equivalente.nota = Convert.ToSingle(reader["nota"]);

                    codigosAsignaturas.Add(reader["codigoAsignaturaOrigen"].ToString());
                    codigosAsignaturas.Add(reader["codigoAsignaturaObjetivo"].ToString());

                    equivalentes.Add(equivalente);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();

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

                List<Entidades.MC.AsignaturasEquivalentesNota> ListaAsignaturasEquivalentes = new List<Entidades.MC.AsignaturasEquivalentesNota>();
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
                                // si el siguiente no es el mismo CodigoAsignaturaOrigen O no existe
                                if (cont2 + 1 >= cantidadEquivalentes)
                                {
                                    int cantidadAsignaturasSeguidas = cont2 - cont + 1;
                                    // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                    Entidades.MC.AsignaturasEquivalentesNota asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentesNota();
                                    asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.AsignaturaNota>();
                                    asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                    
                                    //recorre hacia atras para guardar CodigosOrigen
                                    //recorre la lista se asignaturas buscando el igual para guardarlo
                                    foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                    {
                                        if (asig.Codigo == equivalentes[cont2].codigoAsignaturaOrigen)
                                        {
                                            Entidades.MDP.AsignaturaNota asignatura = new Entidades.MDP.AsignaturaNota();
                                            asignatura.Codigo = asig.Codigo;
                                            asignatura.Creditos = asig.Creditos;
                                            asignatura.Nombre = asig.Nombre;
                                            asignatura.NumeroDecreto = asig.NumeroDecreto;
                                            asignatura.Nota = equivalentes[cont2].nota;
                                            asignaturasEquivalentes.asignaturasOrigen.Add(asignatura);
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
                                        //cuando existe siguiente
                                        if (cont3 == cantidadEquivalentes)
                                        {
                                            int cantidadAsignaturasSeguidasRepetidas = ((cont3 - cont) / cantidadAsignaturasSeguidas);
                                            // se agregar las equivalencia a la lista de asigntaruas equivalentes
                                            Entidades.MC.AsignaturasEquivalentesNota asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentesNota();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.AsignaturaNota>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                                            //recorre hacia atras para guardar CodigosOrigen
                                            for (int temp = cont3 - 1; temp > cont; temp = temp - cantidadAsignaturasSeguidas)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                                    {
                                                        Entidades.MDP.AsignaturaNota asignatura = new Entidades.MDP.AsignaturaNota();
                                                        asignatura.Codigo = asig.Codigo;
                                                        asignatura.Creditos = asig.Creditos;
                                                        asignatura.Nombre = asig.Nombre;
                                                        asignatura.NumeroDecreto = asig.NumeroDecreto;
                                                        asignatura.Nota = equivalentes[temp].nota;
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asignatura);
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
                                            Entidades.MC.AsignaturasEquivalentesNota asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentesNota();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.AsignaturaNota>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();
                                            
                                            //recorre hacia atras para guardar CodigosOrigen
                                            for (int temp = cont3 - 1; temp > cont; temp = temp - cantidadAsignaturasSeguidas)
                                            {
                                                //recorre la lista se asignaturas buscando el igual para guardarlo
                                                foreach (Entidades.MDP.Asignatura asig in asignaturas)
                                                {
                                                    if (asig.Codigo == equivalentes[temp].codigoAsignaturaOrigen)
                                                    {
                                                        Entidades.MDP.AsignaturaNota asignatura = new Entidades.MDP.AsignaturaNota();
                                                        asignatura.Codigo = asig.Codigo;
                                                        asignatura.Creditos = asig.Creditos;
                                                        asignatura.Nombre = asig.Nombre;
                                                        asignatura.NumeroDecreto = asig.NumeroDecreto;
                                                        asignatura.Nota = equivalentes[temp].nota;
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asignatura);
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
                                    Entidades.MC.AsignaturasEquivalentesNota asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentesNota();
                                    asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.AsignaturaNota>();
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
                                                Entidades.MDP.AsignaturaNota asignatura = new Entidades.MDP.AsignaturaNota();
                                                asignatura.Codigo = asig.Codigo;
                                                asignatura.Creditos = asig.Creditos;
                                                asignatura.Nombre = asig.Nombre;
                                                asignatura.NumeroDecreto = asig.NumeroDecreto;
                                                asignatura.Nota = equivalentes[temp].nota;
                                                asignaturasEquivalentes.asignaturasOrigen.Add(asignatura);
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
                                            Entidades.MC.AsignaturasEquivalentesNota asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentesNota();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.AsignaturaNota>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

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
                                                        Entidades.MDP.AsignaturaNota asignatura = new Entidades.MDP.AsignaturaNota();
                                                        asignatura.Codigo = asig.Codigo;
                                                        asignatura.Creditos = asig.Creditos;
                                                        asignatura.Nombre = asig.Nombre;
                                                        asignatura.NumeroDecreto = asig.NumeroDecreto;
                                                        asignatura.Nota = equivalentes[temp].nota;
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asignatura);
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
                                            Entidades.MC.AsignaturasEquivalentesNota asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentesNota();
                                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.AsignaturaNota>();
                                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

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
                                                        Entidades.MDP.AsignaturaNota asignatura = new Entidades.MDP.AsignaturaNota();
                                                        asignatura.Codigo = asig.Codigo;
                                                        asignatura.Creditos = asig.Creditos;
                                                        asignatura.Nombre = asig.Nombre;
                                                        asignatura.NumeroDecreto = asig.NumeroDecreto;
                                                        asignatura.Nota = equivalentes[temp].nota;
                                                        asignaturasEquivalentes.asignaturasOrigen.Add(asignatura);
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
                            Entidades.MC.AsignaturasEquivalentesNota asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentesNota();
                            asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.AsignaturaNota>();
                            asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();
                            
                            int numeroAgregado = 0;
                            foreach (Entidades.MDP.Asignatura asig in asignaturas)
                            {
                                if (asig.Codigo == equivalentes[cont].codigoAsignaturaOrigen)
                                {
                                    Entidades.MDP.AsignaturaNota asignatura = new Entidades.MDP.AsignaturaNota();
                                    asignatura.Codigo = asig.Codigo;
                                    asignatura.Creditos = asig.Creditos;
                                    asignatura.Nombre = asig.Nombre;
                                    asignatura.NumeroDecreto = asig.NumeroDecreto;
                                    asignatura.Nota = equivalentes[cont].nota;
                                    asignaturasEquivalentes.asignaturasOrigen.Add(asignatura);
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

                    // si el siquiente no existe y ambos son diferentes
                    else
                    {
                        // se agregar las equivalencia a la lista de asigntaruas equivalentes
                        Entidades.MC.AsignaturasEquivalentesNota asignaturasEquivalentes = new Entidades.MC.AsignaturasEquivalentesNota();
                        asignaturasEquivalentes.asignaturasOrigen = new List<Entidades.MDP.AsignaturaNota>();
                        asignaturasEquivalentes.asignaturasObjetivo = new List<Entidades.MDP.Asignatura>();

                        int numeroAgregado = 0;
                        foreach (Entidades.MDP.Asignatura asig in asignaturas)
                        {
                            if (asig.Codigo == equivalentes[cont].codigoAsignaturaOrigen)
                            {
                                Entidades.MDP.AsignaturaNota asignatura = new Entidades.MDP.AsignaturaNota();
                                asignatura.Codigo = asig.Codigo;
                                asignatura.Creditos = asig.Creditos;
                                asignatura.Nombre = asig.Nombre;
                                asignatura.NumeroDecreto = asig.NumeroDecreto;
                                asignatura.Nota = equivalentes[cont].nota;
                                asignaturasEquivalentes.asignaturasOrigen.Add(asignatura);
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

        public string[] EliminarHomologacion(string id)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "DELETE FROM homologacion WHERE run = '" + id + "'";

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
