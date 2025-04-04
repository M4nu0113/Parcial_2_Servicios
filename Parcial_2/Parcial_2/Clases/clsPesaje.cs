using Parcial_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parcial_2.Clases
{
    public class clsPesaje
    {
        private DBExamenEntities dbExamen = new DBExamenEntities();
        public Pesaje pesaje { get; set; }

        public string GuardarPesaje(string placa, string marca, int numEjes)
        {
            try
            {
                var ultimoPesaje = dbExamen.Pesajes.OrderByDescending(p => p.id).FirstOrDefault();

                if (ultimoPesaje != null)
                {
                    pesaje.id = ultimoPesaje.id + 1;
                }
                else
                {
                    pesaje.id = 1;
                }

                if (ConsultarPlaca(placa) == false)
                {
                    Camion camion = new Camion
                    {
                        Placa = placa.ToUpper(),
                        Marca = marca,
                        NumeroEjes = numEjes
                    };
                    dbExamen.Camions.Add(camion); // agrega el camion a la base de datos
                    dbExamen.SaveChanges(); // guarda los cambios en la base de datos
                }

                pesaje.PlacaCamion = placa.ToUpper(); // convierte la placa a mayusculas y la asigna al pesaje
                dbExamen.Pesajes.Add(pesaje); // agrega el pesaje a la base de datos
                dbExamen.SaveChanges(); // guarda los cambios en la base de datos

                return "Pesaje guardado correctamente";

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public bool ConsultarPlaca(string placa)
        {
            try
            {
                placa = placa.ToUpper(); // convierte la placa a mayusculas

                Camion camion = dbExamen.Camions.Find(placa); // busca el camion por la placa

                if (camion != null)
                {
                    return true; // la placa existe
                }
                else
                {
                    return false; // la placa no existe
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IQueryable ConsultarPesajexCamion(string placa)
        {
            return from C in dbExamen.Set<Camion>()
                   join P in dbExamen.Set<Pesaje>()
                   on C.Placa equals P.PlacaCamion
                   join F in dbExamen.Set<FotoPesaje>()
                   on P.id equals F.idPesaje
                   where C.Placa == placa
                   select new
                   {
                       placa = C.Placa,
                       marca = C.Marca,
                       numeroEjes = C.NumeroEjes,
                       fechaPesaje = P.FechaPesaje,
                       peso = P.Peso,
                       foto = F.ImagenVehiculo
                   };
        }

        public string GrabarFotoPesaje(int idPesaje, List<string> Fotos)
        {
            try
            {
                foreach (string nombreFoto in Fotos)
                {
                    FotoPesaje foto = new FotoPesaje();
                    foto.idPesaje = idPesaje; // asigna el id del pesaje a la foto
                    foto.ImagenVehiculo = nombreFoto;
                    dbExamen.FotoPesajes.Add(foto); // agrega la foto a la base de datos
                    dbExamen.SaveChanges(); 
                }
                return "Foto guardada correctamente";

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

        }

        public string EliminarFotoPesaje(string nombreFoto)
        {
            try
            {
                FotoPesaje foto = dbExamen.FotoPesajes.FirstOrDefault(f => f.ImagenVehiculo == nombreFoto);
                if (foto == null)
                {
                    return "La imagen no existe en la base de datos";
                }
                else
                {
                    dbExamen.FotoPesajes.Remove(foto);
                    dbExamen.SaveChanges();
                    return "Imagen eliminada correctamente de la base de datos";
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar imagen: " + ex.Message;
            }
        }
    }
}