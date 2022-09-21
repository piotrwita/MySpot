using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Configurations;

//konfiguracja mapowania obiektu rezerwacji (obiect value z i do bazy)
internal sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        //jezeli wlasciwosc w klasie nazywa sie Id jest ona domyslnie traktowana jak identyfikator z poziomi EF
        builder.HasKey(x => x.Id);

        //mapowanie
        //konwersja reservationId na guid i odwrotnie
        //dla typu reservationId
        builder.Property(x => x.Id)
            //chce miec konwersje na guida
            //czyli do bazy danych chce zapisac guida
            //gdy czytam z bazy po przecinku konwercja guid na ReservationId
            .HasConversion(x => x.Value, x => new ReservationId(x));

        builder.Property(x => x.ParkingSpotId)
            .HasConversion(x => x.Value, x => new ParkingSpotId(x));

        builder.Property(x => x.Capacity)
            .IsRequired()
            .HasConversion(x => x.Value, x => new Capacity(x));

        builder.Property(x => x.Date)
            .HasConversion(x => x.Value, x => new Date(x));

        //konfiguracja dyskryminatora
        //dodanie kolumny type
        //wartosc dla konkretnych typow 
        //dzieki temu w bazie moge trzymac jedna tabele z dodatkowa kolumna z typem danych
        builder
            .HasDiscriminator<string>("Type")
            .HasValue<CleaningReservation>(nameof(CleaningReservation))
            .HasValue<VehicleReservation>(nameof(VehicleReservation));
    }
}
