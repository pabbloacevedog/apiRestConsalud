using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using apiRestConsalud.Models;

namespace apiConsalud.Services
{
    public class FacturaService
    {
        private readonly string _filePath = "BD/JsonEjemplo.json";
        private readonly JArray _facturas;

        public FacturaService()
        {
            var jsonString = File.ReadAllText(_filePath);
            _facturas = JArray.Parse(jsonString);

            // Asignar el resultado de CalcularTotales de vuelta a _facturas
            _facturas = CalcularTotales(_facturas);
        }

        public JArray GetFacturas()
        {
            return _facturas;
        }

        // Devolver un nuevo arreglo modificado en lugar de modificar el original
        static JArray CalcularTotales(JArray jsonArray)
        {
            foreach (var factura in jsonArray)
            {
                double totalFactura = factura["DetalleFactura"]
                    .Sum(detalle => (double)detalle["TotalProducto"]);

                factura["TotalFactura"] = totalFactura;
            }

            return jsonArray;
        }

        public JArray GetFacturasPorComprador(double rut)
        {
            return (JArray)_facturas["facturas"].Where(f => (double)f["RUTComprador"] == rut);
        }

        public JObject GetCompradorConMasCompras()
        {
            var compradores = new Dictionary<string, double>();
            var facturas = _facturas;

            foreach (var factura in facturas)
            {
                var rut = (string)factura["RUTComprador"] + (string)factura["DvComprador"];
                var total = (double)factura["TotalFactura"];

                if (compradores.ContainsKey(rut))
                {
                    compradores[rut] += total;
                }
                else
                {
                    compradores.Add(rut, total);
                }
            }

            // Encontrar el máximo total de compras
            var maxCompras = compradores.Values.Max();

            // Filtrar los compradores que tienen el máximo total de compras
            var compradoresConMaxCompras = compradores
                .Where(c => c.Value == maxCompras)
                .Select(c => new JObject
                {
            { "RUTComprador", c.Key },
            { "TotalCompras", c.Value }
                })
                .ToList();

            // Manejar el caso en que haya empates
            if (compradoresConMaxCompras.Count == 1)
            {
                // Solo hay un comprador con el máximo total de compras
                return compradoresConMaxCompras.First();
            }
            else
            {
                // Hay varios compradores con el mismo máximo total de compras
                return new JObject
                {
                    { "Empate", true },
                    { "Compradores", new JArray(compradoresConMaxCompras) }
                };
            }
        }



        public JArray GetCompradoresConMontoTotalDeCompras()
        {
            var compradores = new Dictionary<string, double>();
            var facturas = _facturas;

            foreach (var factura in facturas)
            {
                var rut = (string)factura["RUTComprador"] + (string)factura["DvComprador"];
                var total = (double)factura["TotalFactura"];

                if (compradores.ContainsKey(rut))
                {
                    compradores[rut] += total;
                }
                else
                {
                    compradores.Add(rut, total);
                }
            }

            var compradoresConMontoTotal = new JArray();

            foreach (var comprador in compradores)
            {
                var obj = new JObject
            {
                { "RUTComprador", comprador.Key },
                { "TotalCompras", comprador.Value }
            };

                compradoresConMontoTotal.Add(obj);
            }

            return compradoresConMontoTotal;
        }

        public JArray GetFacturasAgrupadasPorComuna(double comuna)
        {
            var facturas = _facturas;

            // Filtrar las facturas por la comuna específica
            var facturasDeComuna = facturas.Where(f => (double)f["ComunaComprador"] == comuna).ToList();

            // Agrupar las facturas por comuna
            var facturasAgrupadas = facturas.GroupBy(f => (double)f["ComunaComprador"])
                                            .ToDictionary(group => group.Key, group => group.ToList());

            // Retornar la lista de facturas agrupadas
            return new JArray(facturasAgrupadas[comuna]);
        }
        internal IEnumerable<Factura> FormatearFactura(IEnumerable<JToken> facturas)
        {
            foreach (var factura in facturas)
            {
                yield return new Factura
                {
                    NumeroDocumento = (float)factura["NumeroDocumento"],
                    RUTVendedor = (float)factura["RUTVendedor"],
                    DvVendedor = (string)factura["DvVendedor"],
                    RUTComprador = (float)factura["RUTComprador"],
                    DvComprador = (string)factura["DvComprador"],
                    DireccionComprador = (string)factura["DireccionComprador"],
                    ComunaComprador = (float)factura["ComunaComprador"],
                    RegionComprador = (float)factura["RegionComprador"],
                    TotalFactura = (float)factura["TotalFactura"],
                    Detallefactura = ((JArray)factura["DetalleFactura"]).Select(detalle => new Detallefactura
                    {
                        CantidadProducto = (float)detalle["CantidadProducto"],
                        Producto = new Producto
                        {
                            Descripcion = (string)detalle["Producto"]["Descripcion"],
                            Valor = (float)detalle["Producto"]["Valor"]
                        },
                        TotalProducto = (float)detalle["TotalProducto"]
                    }).ToArray()
                };
            }
        }
    }
}

