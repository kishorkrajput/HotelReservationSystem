using HotelReservationSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservationSystem.Repositories
{
    public interface IBookingRepository
    {
        /// <summary>
        /// Get all the rooms.
        /// </summary>
        /// <returns>List of rooms.</returns>
        Task<List<RoomEntity>> GetRoomsAsync();
    }
}
