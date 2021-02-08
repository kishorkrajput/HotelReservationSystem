
using HotelReservationSystem.Entities;
using HotelReservationSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace HotelReservationSystem.Business
{
    public class BookingBusiness : IBookingBusiness
    {
        private readonly IBookingRepository bookingRepository;

        public int HotelSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingBusiness"/> class.
        /// <summary>
        public BookingBusiness(IBookingRepository bookingRepository)
        {
            this.bookingRepository  = bookingRepository;

        }

        /// <inheritdoc/>
        public async Task<BookingResult> BookRoomAsync(int startDay, int endDay)
        {
            if (!IsBookingDaysValid(startDay, endDay))
            {
                return BookingResult.Decline;
            }

            var rooms = await this.bookingRepository.GetRoomsAsync();
            var availableRoom = this.GetAvailableRoom(startDay, endDay, rooms);

            if (availableRoom != null)
            {
                // Update the room booking
                if (availableRoom != null)
                {
                    for (int i = startDay; i <= endDay; i++)
                    {
                        availableRoom.days[i] = 1;
                        availableRoom.bookedDays++;
                    }
                    availableRoom.lastBookedDay = endDay;
                }
                return BookingResult.Accept;
            }
            return BookingResult.Decline;
        }

        /// <summary>
        /// Returns the available room based on booking period.
        /// </summary>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <param name="rooms"></param>
        /// <returns></returns>
        private RoomEntity GetAvailableRoom(int startDay, int endDay, List<RoomEntity> rooms)
        {
            RoomEntity availableRoom = null;
            foreach (var room in rooms.OrderBy(r => endDay - r.lastBookedDay))
            {
                if (room.IsRoomFullyBooked())
                    continue;

                var found = true;

                for ( int i = startDay; i <= endDay; i++)
                {
                    if (room.days[i] == 1)
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    availableRoom = room;
                    break; 
                }          
            }

            if (availableRoom == null)
            {
                if (this.CreateRoom(rooms))
                {
                    availableRoom =  rooms[rooms.Count - 1];
                }
            }
            
            return availableRoom;

        }

        /// <summary>
        /// Add new room.
        /// </summary>
        /// <param name="rooms">List of rooms.</param>
        /// <returns>Returns true if allowed to add new room else false.</returns>
        private bool CreateRoom(List<RoomEntity> rooms)
        {
            if (rooms.Count < HotelSize)
            {
                var newRoom = new RoomEntity();
                rooms.Add(newRoom);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates the booking period.
        /// </summary>
        /// <param name="startDay">Booking start day.</param>
        /// <param name="endDay">Booking end day.</param>
        /// <returns>Returns True if booking days are within plan period.</returns>
        private bool IsBookingDaysValid(int startDay, int endDay)
        {
            var isValid = true;
            if (startDay < 0 || endDay > 365)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
