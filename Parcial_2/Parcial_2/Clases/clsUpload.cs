using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Parcial_2.Clases
{
    public class clsUpload
    {
        public string Datos { get; set; }
        public string Proceso { get; set; }
        public HttpRequestMessage request { get; set; }
        private List<string> Archivos;

        private string ProcesarBD(string nombreArchivo = "")
        {
            clsPesaje pesaje = new clsPesaje();

            switch (Proceso.ToUpper())
            {
                case "PESAJE":
                    
                    return pesaje.GrabarFotoPesaje(Convert.ToInt32(Datos), Archivos);
                case "PAPELERA":
                    return pesaje.EliminarFotoPesaje(nombreArchivo);

                default:
                    return "No se ha definido el proceso en la base de datos";

            }
        }
        public async Task<HttpResponseMessage> GrabarArchivo(bool Actualizar)
        {
            if (!request.Content.IsMimeMultipartContent())
            {
                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError,
                    "No se envió un archivo valido para procesar");
            }
            string root = HttpContext.Current.Server.MapPath("~/Archivos");
            var provider = new MultipartFormDataStreamProvider(root);
            bool existe = false;
            try
            {
                // Leer el contenido de los archivos
                await request.Content.ReadAsMultipartAsync(provider);
                if (provider.FileData.Count > 0)
                {
                    Archivos = new List<string>();
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        string nombre = file.Headers.ContentDisposition.FileName;
                        if (nombre.StartsWith("\"") && nombre.EndsWith("\""))
                        {
                            nombre = nombre.Trim('"');
                        }
                        if (nombre.Contains(@"/") || nombre.Contains(@"\"))
                        {
                            nombre = Path.GetFileName(nombre);
                        }

                        if (File.Exists(Path.Combine(root, nombre)))
                        {
                            if (Actualizar)
                            {
                                //El archivo ya existe en el servidor, se elimina el original y se permite el cambio de nombre
                                File.Delete(Path.Combine(root, nombre));
                                File.Move(file.LocalFileName, Path.Combine(root, nombre));
                                existe = true;
                            }
                            else
                            {
                                // El archivo ya existe en el servidor, se elimina el archivo nuevo
                                File.Delete(Path.Combine(root, nombre));
                                existe = true;
                            }
                        }
                        else
                        {
                            //Agrego en una lista el nombre de los archivos que se cargaron 
                            Archivos.Add(nombre);
                            //Renombra el archivo temporal
                            File.Move(file.LocalFileName, Path.Combine(root, nombre));
                        }
                    }
                    if (!existe)
                    {
                        //Se genera el proceso de gestión en la base de datos
                        string RptaBD = ProcesarBD();
                        //Termina el ciclo, responde que se cargó el archivo correctamente
                        return request.CreateResponse(System.Net.HttpStatusCode.OK, "Se cargaron los archivos en el servidor, " + RptaBD);
                    }
                    else
                    {
                        return request.CreateErrorResponse(System.Net.HttpStatusCode.Conflict, "Ya existen los archivos en el servidor");
                    }
                }
                else
                {
                    return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError,
                        "No se encontraron archivos para procesar");
                }
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        public HttpResponseMessage LeerArchivo(string nombreArchivo)
        {
            string root = HttpContext.Current.Server.MapPath("~/Archivos");
            string archivo = Path.Combine(root, nombreArchivo);
            if (File.Exists(archivo))
            {
                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                var stream = new FileStream(archivo, FileMode.Open, FileAccess.Read);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = nombreArchivo;
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                return response;
            }
            else
            {
                return request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "No se encontró el archivo solicitado");
            }
        }
        public HttpResponseMessage EliminarArchivo(string nombreArchivo)
        {
            try
            {
                string Ruta = HttpContext.Current.Server.MapPath("~/Archivos");
                string archivoCompleto = Path.Combine(Ruta, nombreArchivo);
                if (File.Exists(archivoCompleto))
                {
                    File.Delete(archivoCompleto);
                    string resultado = ProcesarBD(nombreArchivo);
                    return request.CreateResponse(HttpStatusCode.OK, "Se eliminó el archivo del servidor, " + resultado);

                }
                else
                {
                    return request.CreateErrorResponse(HttpStatusCode.NotFound, "El archivo no se encuentra en el servidor");
                }
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No se pudo eliminar el archivo. " + ex.Message);
            }
        }
    }
}