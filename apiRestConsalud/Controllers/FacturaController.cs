using Microsoft.AspNetCore.Mvc;

using apiRestConsalud.Models;
using apiConsalud.Services;
using Newtonsoft.Json.Linq;

namespace apiRestConsalud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly FacturaService _facturaService;

        public FacturaController(FacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [HttpGet]
        public IEnumerable<Factura> GetFacturas()
        {
            var facturasConTotales = _facturaService.GetFacturas();
            return _facturaService.FormatearFactura(facturasConTotales);
        }

        [HttpGet("{rut}")]
        public IEnumerable<Factura> GetFacturasPorComprador(string rut)
        {
            // Obtén el RUTComprador y el DvComprador de la URL
            var rutComprador = rut.Substring(0, rut.Length - 2); // Elimina el último carácter (DvComprador)
            var dvComprador = rut.Substring(rut.Length - 1); // Obtiene el último carácter (DvComprador)
            Console.WriteLine(rutComprador + '-' + dvComprador);

            // Filtra las facturas por el RUTComprador y el DvComprador
            var facturas = _facturaService.GetFacturas()
                .Where(f =>
                    ((float)f["RUTComprador"]).ToString().TrimEnd('0') == rutComprador &&
                    (string)f["DvComprador"] == dvComprador)
                .ToList();

            return _facturaService.FormatearFactura(facturas);
        }


        [HttpGet("comprador-mas-compras")]
        public IActionResult GetCompradorConMasCompras()
        {
            var comprador = _facturaService.GetCompradorConMasCompras();

            if (comprador != null)
            {
                return Ok(comprador.ToString());
            }
            else
            {
                return NotFound(); 
            }
        }

        [HttpGet("compradores-con-monto-total")]
        public IActionResult GetCompradoresConMontoTotalDeCompras()
        {
            var compradores = _facturaService.GetCompradoresConMontoTotalDeCompras();
            return Ok(compradores.ToString());
        }

        [HttpGet("facturas-por-comuna/{comuna}")]
        public IActionResult GetFacturasAgrupadasPorComuna(double comuna)
        {
            var facturas = _facturaService.GetFacturasAgrupadasPorComuna(comuna);
            return Ok(facturas.ToString());
        }
    }
}
