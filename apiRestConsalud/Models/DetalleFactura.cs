using System;
namespace apiRestConsalud.Models
{
    public class Detallefactura
    {
        public float CantidadProducto { get; set; }
        public Producto Producto { get; set; }
        public float TotalProducto { get; set; }
    }
}

