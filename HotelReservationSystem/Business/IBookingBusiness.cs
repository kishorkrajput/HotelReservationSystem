using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservationSystem.Business
{
    public interface IBookingBusiness
    {
        /// <summary>
        /// Gets or sets the hotel size.
        /// </summary>
        int HotelSize { get; set; }

        /// <summary>
        /// Book the room based on availability and  return the response.
        /// </summary>
        /// <param name="startDay">Booking start day.</param>
        /// <param name="endDay">Booking end day.</param>
        /// <returns>Booking response.</returns>
        Task<BookingResult> BookRoomAsync(int startDay, int endDay);
    }
}
