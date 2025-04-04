using Parcial_2.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Parcial_2.Controllers
{
    [RoutePrefix("api/UploadFiles")]
    public class UploadFilesController : ApiController
    {
        [HttpPost]
        [Route("CargarArchivo")]
        public async Task<HttpResponseMessage> CargarArchivo(HttpRequestMessage request, string Datos, string Proceso)
        {
            clsUpload upload = new clsUpload();
            upload.Datos = Datos;
            upload.Proceso = Proceso;
            upload.request = request;
            return await upload.GrabarArchivo(false);
        }

        [HttpPut]
        [Route("Actualizar")]
        public async Task<HttpResponseMessage> Actualizar(HttpRequestMessage request, string Datos, string Proceso)
        {
            clsUpload upload = new clsUpload();
            upload.Datos = Datos;
            upload.Proceso = Proceso;
            upload.request = request;
            return await upload.GrabarArchivo(true);
        }

        [HttpGet]
        [Route("LeerArchivo")]
        public HttpResponseMessage LeerArchivo(string NombreArchivo)
        {
            clsUpload upload = new clsUpload();
            return upload.LeerArchivo(NombreArchivo);
        }

        [HttpDelete]
        [Route("EliminarArchivo")]
        public HttpResponseMessage EliminarArchivo(HttpRequestMessage request, string NombreArchivo, string Proceso)
        {
            clsUpload upload = new clsUpload();
            upload.Proceso = Proceso;
            upload.request = request;
            return upload.EliminarArchivo(NombreArchivo);
        }
    }
}