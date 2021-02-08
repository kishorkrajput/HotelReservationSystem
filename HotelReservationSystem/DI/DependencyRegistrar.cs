using HotelReservationSystem.Business;
using HotelReservationSystem.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservationSystem.DI
{
    /// <summary>
    /// Contains methods to registers all the dependencies required for the application to work.
    /// </summary>
    public static class DependencyRegistrar
    {
        /// <summary>
        /// Registers dependencies to <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">Instance of <see cref="IServiceCollection"/>.</param>
        /// <returns>An instance of IOC container that has dependencies registered.</returns>
        public static IServiceCollection RegisterDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IBookingBusiness, BookingBusiness>();
            serviceCollection.AddTransient<IBookingRepository, BookingRepository>();
            return serviceCollection;
        }
    }
}
