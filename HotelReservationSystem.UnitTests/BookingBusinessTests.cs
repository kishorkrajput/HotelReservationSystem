using HotelReservationSystem.Business;
using HotelReservationSystem.Entities;
using HotelReservationSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.AutoMock;
using Xunit;

namespace HotelReservationSystem.UnitTests
{
    public class BookingBusinessTests
    {

      
        /// <summary>
        /// 1a/1b: When the requests are outside our planning period, should decline requests.
        /// </summary>
        /// <returns>Task for the Test execution.</returns>
        [Theory]
        [InlineData(-4, 2)]
        [InlineData(200, 400)]
        public async Task BookRoomAsync_WhenRequestsOutside_ShouldDeclineRequests(int startDay, int endDay)
        {
            // Arrange
            using var container = this.GetMockingContainer();

            //Act
            var actualBookingStatus = await container.Instance.BookRoomAsync(startDay, endDay);

            //Assert
            Assert.Equal(BookingResult.Decline, actualBookingStatus);

        }

        /// <summary>
        /// 2: When the requests are within planning period, should accept all request based on availability.
        /// </summary>
        /// <returns>Task for the Test execution.</returns>
        [Fact]
        public async Task BookRoomAsync_BasedOnAvailability_ShouldAcceptAllRequests()
        {
            // Arrange
            using var container = this.GetMockingContainer();
            container.Instance.HotelSize = 3;
            List<RoomEntity> rooms = new List<RoomEntity>();

            List<(int startDay, int endDay, BookingResult expectedResult)> bookingList = new List<(int, int, BookingResult)>
              {
                  (0, 5, BookingResult.Accept),
                  (7, 13, BookingResult.Accept),
                  (3, 9, BookingResult.Accept),
                  (5, 7, BookingResult.Accept),
                  (6, 6, BookingResult.Accept),
                  (0, 4, BookingResult.Accept)
              };
            container.Arrange<IBookingRepository>(bookingRepository => bookingRepository.GetRoomsAsync())
                .Returns(Task.FromResult(rooms));

            foreach (var (startDay, endDay, expectedResult) in bookingList)
            {
                //Act
                var actualBookingStatus = await container.Instance.BookRoomAsync(startDay, endDay);

                //Assert
                Assert.Equal(expectedResult, actualBookingStatus);

            }

        }

        /// <summary>
        /// 3: When the requests are within planning period, should accept/decline requests based on availability.
        /// </summary>
        /// <returns>Task for the Test execution.</returns>
        [Fact]
        public async Task BookRoomAsync_BasedOnAvailability_ShouldAcceptAndDeclineRequests()
        {
            // Arrange
            using var container = this.GetMockingContainer();
            container.Instance.HotelSize = 3;
            List<RoomEntity> rooms = new List<RoomEntity>();

            List<(int startDay, int endDay, BookingResult expectedResult)> bookingList = new List<(int, int, BookingResult)>
              {
                  (1, 3, BookingResult.Accept),
                  (2, 5, BookingResult.Accept),
                  (1, 9, BookingResult.Accept),
                  (0, 15, BookingResult.Decline)
              };
            container.Arrange<IBookingRepository>(bookingRepository => bookingRepository.GetRoomsAsync())
                .Returns(Task.FromResult(rooms));

            foreach (var (startDay, endDay, expectedResult) in bookingList)
            {
                //Act
                var actualBookingStatus = await container.Instance.BookRoomAsync(startDay, endDay);

                //Assert
                Assert.Equal(expectedResult, actualBookingStatus);

            }

        }

        /// <summary>
        /// 4: When the requests are within planning period, requests can be accepted after a decline based on availability.
        /// </summary>
        /// <returns>Task for the Test execution.</returns>
        [Fact]
        public async Task BookRoomAsync_BasedOnAvailability_RequestsCanBeAcceptedAfterDecline()
        {
            // Arrange
            using var container = this.GetMockingContainer();
            container.Instance.HotelSize = 3;
            List<RoomEntity> rooms = new List<RoomEntity>();

            List<(int startDay, int endDay, BookingResult expectedResult)> bookingList = new List<(int, int, BookingResult)>
              {
                  (1, 3, BookingResult.Accept),
                  (0, 15, BookingResult.Accept),
                  (1, 9, BookingResult.Accept),
                  (2, 5, BookingResult.Decline),
                  (4, 9, BookingResult.Accept)

              };
            container.Arrange<IBookingRepository>(bookingRepository => bookingRepository.GetRoomsAsync())
                .Returns(Task.FromResult(rooms));

            foreach (var (startDay, endDay, expectedResult) in bookingList)
            {
                //Act
                var actualBookingStatus = await container.Instance.BookRoomAsync(startDay, endDay);

                //Assert
                Assert.Equal(expectedResult, actualBookingStatus);

            }

        }

        /// <summary>
        /// 5: When the requests are within planning period, requests can be accepted or declined based on availability.
        /// </summary>
        /// <returns>Task for the Test execution.</returns>
        [Fact]
        public async Task BookRoomAsync_BasedOnAvailability_RequestsCanBeAcceptedOrDeclined()
        {
            // Arrange
            using var container = this.GetMockingContainer();
            container.Instance.HotelSize = 2;
            List<RoomEntity> rooms = new List<RoomEntity>();

            List<(int startDay, int endDay, BookingResult expectedResult)> bookingList = new List<(int, int, BookingResult)>
              {
                  (1, 3, BookingResult.Accept),
                  (0, 4, BookingResult.Accept),
                  (2, 3, BookingResult.Decline),
                  (5, 5, BookingResult.Accept),
                  (4, 10, BookingResult.Accept),
                  (10, 10, BookingResult.Accept),
                  (6, 7, BookingResult.Accept),
                  (8, 10, BookingResult.Decline),
                  (8, 9, BookingResult.Accept)

              };
            container.Arrange<IBookingRepository>(bookingRepository => bookingRepository.GetRoomsAsync())
                .Returns(Task.FromResult(rooms));

            foreach (var (startDay, endDay, expectedResult) in bookingList)
            {
                //Act
                var actualBookingStatus = await container.Instance.BookRoomAsync(startDay, endDay);

                //Assert
                Assert.Equal(expectedResult, actualBookingStatus);

            }

        }

        /// <summary>
        /// Return mocking container for <see cref="ExecuteScript"/>.
        /// </summary>
        /// <returns>ExecuteScript.</returns>
        private MockingContainer<BookingBusiness> GetMockingContainer()
        {
            var container = new MockingContainer<BookingBusiness>(new AutoMockSettings()
            {
                ConstructorArgTypes = typeof(BookingBusiness).GetConstructors()
                    .OrderByDescending(c => c.GetParameters().Count())
                    .First()
                    .GetParameters()
                    .Select(p => p.ParameterType)
                    .ToArray(),
                MockBehavior = Behavior.Strict,
            });
            return container;
        }
    }
}
