using Xunit;
using Moq; // Asegúrate de tener instalado el paquete NuGet Moq para poder usarlo
using Microsoft.AspNetCore.Mvc;
using apiRestConsalud.Controllers;
using apiConsalud.Services;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace testApiConsalud.Pruebas.Controllers
{
    public class FacturaControllerTests
    {
        [Fact]
        public void GetFacturas_DebeRetornarFacturasFormateadas()
        {
            // Configurar el servicio con un mock
            var mockFacturaService = new Mock<FacturaService>();
            mockFacturaService.Setup(service => service.GetFacturas()).Returns(new JArray()); 

            // Crear una instancia del controlador con el servicio mock
            var controller = new FacturaController(mockFacturaService.Object);

            // Act
            var result = controller.GetFacturas();

            // Verificar el resultado
            Assert.IsType<OkObjectResult>(result.Result); // Verificar si el resultado es un Ok
            var okResult = (OkObjectResult)result.Result;
            Assert.IsType<JArray>(okResult.Value); // Verificar si el valor es un JArray
        }

        [Fact]
        public void GetFacturas_ReturnsOkResult()
        {
            // Arrange
            var facturaServiceMock = new Mock<FacturaService>();
            facturaServiceMock.Setup(service => service.GetFacturas()).Returns(new JArray()); 

            var controller = new FacturaController(facturaServiceMock.Object);

            // Act
            var result = controller.GetFacturas();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); // Verifica que el valor no sea nulo
        }

        [Fact]
        public void GetFacturasPorComprador_ReturnsOkResult()
        {
            // Arrange
            var facturaServiceMock = new Mock<FacturaService>();
            facturaServiceMock.Setup(service => service.GetFacturasPorComprador(It.IsAny<double>())).Returns(new JArray());

            var controller = new FacturaController(facturaServiceMock.Object);

            // Act
            var result = controller.GetFacturasPorComprador("12345678"); // Proporciona un RUT válido

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void GetCompradorConMasCompras_ReturnsOkResult()
        {
            // Arrange
            var facturaServiceMock = new Mock<FacturaService>();
            facturaServiceMock.Setup(service => service.GetCompradorConMasCompras()).Returns(new JObject());

            var controller = new FacturaController(facturaServiceMock.Object);

            // Act
            var result = controller.GetCompradorConMasCompras();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void GetCompradoresConMontoTotalDeCompras_ReturnsOkResult()
        {
            // Arrange
            var facturaServiceMock = new Mock<FacturaService>();
            facturaServiceMock.Setup(service => service.GetCompradoresConMontoTotalDeCompras()).Returns(new JArray());

            var controller = new FacturaController(facturaServiceMock.Object);

            // Act
            var result = controller.GetCompradoresConMontoTotalDeCompras();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void GetFacturasAgrupadasPorComuna_ReturnsOkResult()
        {
            // Arrange
            var facturaServiceMock = new Mock<FacturaService>();
            facturaServiceMock.Setup(service => service.GetFacturasAgrupadasPorComuna(It.IsAny<double>())).Returns(new JArray());

            var controller = new FacturaController(facturaServiceMock.Object);

            // Act
            var result = controller.GetFacturasAgrupadasPorComuna(1); // Proporciona un valor de comuna válido

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}
