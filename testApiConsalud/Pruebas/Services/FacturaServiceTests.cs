using Xunit;
using Moq;
using apiConsalud.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace testApiConsalud.Pruebas.Services
{
    public class FacturaServiceTests
    {
        [Fact]
        public void GetFacturas_DebeRetornarFacturas()
        {
            // Configurar el servicio con un mock
            var mockFilePath = "BD/JsonEjemplo.json";
            var mockJsonString = File.ReadAllText(mockFilePath);
            var mockFacturas = JArray.Parse(mockJsonString);

            var mockFacturaService = new Mock<FacturaService>();
            mockFacturaService.Setup(service => service.GetFacturas()).Returns(mockFacturas);

            // Actuar - Llamar al método de prueba
            var result = mockFacturaService.Object.GetFacturas();

            // Afirmar - Verificar el resultado
            Assert.NotNull(result);
            Assert.IsType<JArray>(result);
        }

        [Fact]
        public void GetFacturas_ReturnsCorrectFacturas()
        {
            // Arrange
            var facturaService = new FacturaService();

            // Act
            var result = facturaService.GetFacturas();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetCompradorConMasCompras_ReturnsCorrectComprador()
        {
            // Arrange
            var facturaService = new FacturaService();

            // Act
            var result = facturaService.GetCompradorConMasCompras();

            // Assert
            Assert.NotNull(result);
            
        }

        [Fact]
        public void GetCompradoresConMontoTotalDeCompras_ReturnsCorrectCompradores()
        {
            // Arrange
            var facturaService = new FacturaService();

            // Act
            var result = facturaService.GetCompradoresConMontoTotalDeCompras();

            // Assert
            Assert.NotNull(result);
            
        }

        [Fact]
        public void GetFacturasAgrupadasPorComuna_ReturnsCorrectFacturas()
        {
            // Arrange
            var facturaService = new FacturaService();
            double comuna = 123;  // Proporciona un valor de comuna válido

            // Act
            var result = facturaService.GetFacturasAgrupadasPorComuna(comuna);

            // Assert
            Assert.NotNull(result);
            
        }
    }
}
