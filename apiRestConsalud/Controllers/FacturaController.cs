using Microsoft.AspNetCore.Mvc;

using apiRestConsalud.Models;
using apiConsalud.Services;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace apiRestConsalud.Controllers
{
    [ApiController]
    [Route("/")]
    public class FacturaController : ControllerBase
    {
        private readonly FacturaService _facturaService;

        public FacturaController(FacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [Authorize(AuthenticationSchemes = "ApiKeyScheme")]
        [HttpGet]
        public ActionResult<IEnumerable<Factura>> GetFacturas()
        {
            try
            {
                var facturasConTotales = _facturaService.GetFacturas();
                var facturasFormateadas = _facturaService.FormatearFactura(facturasConTotales);
                return Ok(facturasFormateadas);
            }
            catch (Exception ex)
            {
                // Log de la excepción si es necesario
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize(AuthenticationSchemes = "ApiKeyScheme")]
        [HttpGet("{rut}")]
        public ActionResult<IEnumerable<Factura>> GetFacturasPorComprador(string rut)
        {
            try
            {
                var rutComprador = rut.Substring(0, rut.Length - 2);
                var dvComprador = rut.Substring(rut.Length - 1);

                var facturas = _facturaService.GetFacturas()
                    .Where(f =>
                        ((float)f["RUTComprador"]).ToString().TrimEnd('0') == rutComprador &&
                        (string)f["DvComprador"] == dvComprador)
                    .ToList();

                return Ok(_facturaService.FormatearFactura(facturas));
            }
            catch (Exception ex)
            {
                // Log de la excepción si es necesario
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize(AuthenticationSchemes = "ApiKeyScheme")]
        [HttpGet("comprador-mas-compras")]
        public IActionResult GetCompradorConMasCompras()
        {
            try
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
            catch (Exception ex)
            {
                // Log de la excepción si es necesario
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize(AuthenticationSchemes = "ApiKeyScheme")]
        [HttpGet("compradores-con-monto-total")]
        public IActionResult GetCompradoresConMontoTotalDeCompras()
        {
            try
            {
                var compradores = _facturaService.GetCompradoresConMontoTotalDeCompras();
                return Ok(compradores.ToString());
            }
            catch (Exception ex)
            {
                // Log de la excepción si es necesario
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize(AuthenticationSchemes = "ApiKeyScheme")]
        [HttpGet("facturas-por-comuna/{comuna}")]
        public IActionResult GetFacturasAgrupadasPorComuna(double comuna)
        {
            try
            {
                var facturas = _facturaService.GetFacturasAgrupadasPorComuna(comuna);
                return Ok(facturas.ToString());
            }
            catch (Exception ex)
            {
                // Log de la excepción si es necesario
                return StatusCode(500, "Error interno del servidor");
            }
        }

    }
}
