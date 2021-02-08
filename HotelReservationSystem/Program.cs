using HotelReservationSystem.Business;
using HotelReservationSystem.DI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace HotelReservationSystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            try
            {
                var bookingBusiness = services.GetRequiredService<IBookingBusiness>();
                Console.WriteLine("Please enter the Hotel Size");
                var hotelSize = Console.ReadLine();
                bookingBusiness.HotelSize = int.Parse(hotelSize);
                while (true)
                {
                    Console.WriteLine("\nPlease enter Start and End date i.e 1, 3");
                    var bookingData = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(bookingData))
                    {
                        var bookingDays = bookingData.Split(",");

                        if (bookingDays.Length == 2 && int.TryParse(bookingDays[0].Trim(), out int startDay) && int.TryParse(bookingDays[1].Trim(), out int endDay))
                        {
                            var bookingStatus = await bookingBusiness.BookRoomAsync(startDay, endDay);

                            Console.WriteLine($"Booking Status: {bookingStatus}.");

                            Console.WriteLine("Do you want to book more room (Y/N)");
                            var choice = Console.ReadKey();
                            if (choice.Key != ConsoleKey.Y)
                            {
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid booking entry, please enter valid Start and End date i.e 1, 3");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred. {ex.Message}");
            }

        }

        /// <summary>
        /// Creates an instance of <c>IHostBuilder</c>.
        /// </summary>
        /// <param name="args">Arguments to create an instance of <c>IHostBuilder</c>.</param>
        /// <returns>Instance of <c>IHostBuilder</c>.</returns>
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((services) =>
                {
                    services.RegisterDependencies();
                });
        }
    }
}
