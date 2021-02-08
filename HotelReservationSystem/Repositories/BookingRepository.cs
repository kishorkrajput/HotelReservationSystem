using HotelReservationSystem.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservationSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        readonly List<RoomEntity> rooms = new List<RoomEntity>();

        /// <inheritdoc/>
        public Task<List<RoomEntity>> GetRoomsAsync()
        {
            // for Simplicity, using in-memory for storing the room data but in actual there will be a DB call hence made this method async.
            return Task.FromResult(rooms);   
        }
    }
}
