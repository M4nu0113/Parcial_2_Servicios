using Parcial_2.Clases;
using Parcial_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Parcial_2.Controllers
{
    [RoutePrefix("api/Pesaje")]
    public class PesajeController : ApiController
    {
        [HttpPost]
        [Route("GuardarPesaje")]
        public string GuardarPesaje([FromBody] Pesaje pesaje, string placa, string marca, int numEjes)
        {
            try
            {
                clsPesaje clsPesaje = new clsPesaje();
                clsPesaje.pesaje = pesaje;
                return clsPesaje.GuardarPesaje(placa, marca, numEjes);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [HttpGet]
        [Route("ConsultarPesajexCamion")]
        public IQueryable ConsultarPesajexCamnion(string placa)
        {
            try
            {
                clsPesaje clsPesaje = new clsPesaje();
                return clsPesaje.ConsultarPesajexCamion(placa);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

    }
}