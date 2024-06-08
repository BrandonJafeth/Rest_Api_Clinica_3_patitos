using API_PruebaEF.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Appointments;
using Services.Extensions;
using static Services.Extensions.DtoMapping;

namespace Testing
{
    public class UnitTest1
    {
       
            private readonly Mock<ISvAppointment> _mockSvAppointment;
        private readonly AppoitmentController _controller;
        private readonly Mock<IAuthorizationService> _mockAuthorizationService = new Mock<IAuthorizationService>();

       
           

            public UnitTest1()
        {
            _mockSvAppointment = new Mock<ISvAppointment>();
            _controller = new AppoitmentController(_mockSvAppointment.Object, _mockAuthorizationService.Object);

            _mockAuthorizationService = new Mock<IAuthorizationService>();
        }

        [Fact]
        public async Task Get_ReturnsAllAppointments()
        {
            // Arrange
            var appointments = new List<DtoAppointment>
            {
            new DtoAppointment { Id_Appointment  = 1, Name_type = "Test Appointment 1" },
            new DtoAppointment { Id_Appointment  = 2, Name_type = "Test Appointment 2" }
        };
            _mockSvAppointment.Setup(svc => svc.GetAllAppointments()).ReturnsAsync(appointments);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.Equal(appointments, result);
        }

        [Fact]
        public async Task Get_WithId_ReturnsAppointment()
        {
            // Arrange
            var appointment = new DtoMapping.DtoAppointment { Id_Appointment = 1, Name_type = "Test Appointment" };
            _mockSvAppointment.Setup(svc => svc.GetAppointmentById(1)).ReturnsAsync(appointment);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.Equal(appointment, result);
        }






        [Fact]
        public async Task GetAppointmentDates_ReturnsDates()
        {
            // Arrange
            var dates = new List<DateTime>()
    {
        DateTime.Today,
        DateTime.Today.AddDays(1)
    };
            _mockSvAppointment.Setup(svc => svc.GetAppointmentDates())
                .ReturnsAsync(dates);

            // Act
            var result = await _controller.GetAppointmentDates();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dates, result);
        }
        [Fact]
        public async Task GetAppointmentsForToday_ReturnsAppointments_ForAdminUser()
        {
            // Arrange
            var appointments = new List<DtoMapping.DtoAppointment>()
            {



    new DtoMapping.DtoAppointment
    {
        Id_Appointment = 1,
        Date = DateTime.Now,
        Id_ClinicBranch = 2,
        Id_Appoitment_Type = 3
    },
    new DtoMapping.DtoAppointment
    {
        Id_Appointment = 2,
        Date = DateTime.Now.AddHours(1),
        Id_ClinicBranch = 4,
        Id_Appoitment_Type = 5
    },



        };
            _mockSvAppointment.Setup(svc => svc.GetAppointmentsForToday())
                .ReturnsAsync(appointments);

            // Act
            var result = await _controller.GetAppointmentsForToday();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(appointments, result);
        }

        [Fact]

        public async Task CancelAppointment_ThrowsException_ReturnsBadRequest()
        {
            // Arrange 
            var id = 1;

            _mockSvAppointment.Setup(svc => svc.CancelAppointment(id))
                .ThrowsAsync(new Exception("Error canceling appointment"));
            // Act
            var result = await _controller.CancelAppointment(id);

            // Assert 


            Assert.Equal(typeof(BadRequestObjectResult), result.GetType());


            var badRequestResult = (BadRequestObjectResult)result;
            Assert.NotNull(badRequestResult.Value);


        }
        [Fact]
        public async Task CancelAppointment_ReturnsTrue_ForAuthorizedUser()
        {
            // Arrange
            var id = 1;

            _mockSvAppointment.Setup(svc => svc.CancelAppointment(id))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _controller.CancelAppointment(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }



        [Fact]
        public async Task GetAppointmentsByUserId_ReturnsAppointments_ForAuthorizedUser()
        {
            // Arrange
            var userId = 1;
            var appointments = new List<DtoAppointment>()
        {
            new DtoAppointment { Id_Appointment = 1 },
            new DtoAppointment { Id_Appointment = 2 }
        };
            _mockSvAppointment.Setup(svc => svc.GetAppointmentsByUserId(userId))
                .ReturnsAsync(appointments);

            // Act
            var result = await _controller.GetAppointmentsByUserId(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(appointments, result);
        }


        [Fact]
        public async Task GetAppointmentsByUserId_ThrowsUnauthorized_ForUnauthorizedUser()
        {
            // Arrange
            var userId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _controller.GetAppointmentsByUserId(userId));
        }



        [Fact]
        public async Task GetAppointmentsForTodayThrowsUnauthorizedForUnauthorizedUser()
        {
            // Arrange

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _controller.GetAppointmentsForToday()); // Expect Unauthorized exception
        }

        [Fact]
        public async Task Delete_AuthorizedUser_ReturnsOk()
        {
            // Arrange
            var id = 1;


            _mockSvAppointment.Setup(svc => svc.DeleteAppointment(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }


       

           
        }


    }



