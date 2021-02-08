using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservationSystem.Entities
{
    public class RoomEntity
    {
        public int[] days = new int[365];
        public int bookedDays = 0;
        public int lastBookedDay;

        public bool IsRoomFullyBooked()
        {
            return bookedDays == 365;
        }
    }
}
