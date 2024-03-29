using iPractice.Scheduling.Domain.Exceptions;
using iPractice.Scheduling.Domain.ScheduleAggregate;
using iPractice.Scheduling.Domain.ValueObjects;

namespace iPractice.Scheduling.Tests.ScheduleAggregateTests
{
    public class AvailablityTests
    {
        private static IEnumerable<object[]> availabilities = new List<object[]>() {
            new object[] { //availability between 9:00-17:00, no appointment
                new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(new DateTime(2024, 1, 30, 9, 0, 0), new DateTime(2024, 1, 30, 17, 0, 0))),
                16
            },
            new object[] { //availability between 9:00-17:00, appointments 13:00-13:30
                new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(new DateTime(2024, 1, 30, 9, 0, 0), new DateTime(2024, 1, 30, 17, 0, 0)))
                        .AddAppointment(new Appointment(Guid.NewGuid(),Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 13, 0, 0), new DateTime(2024, 1, 30, 13, 30, 0)))),
                15
            },
            new object[] { //availability between 9:00-17:00, appointments 13:00-13:30 and 15:45-16:15
                new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(new DateTime(2024, 1, 30, 9, 0, 0), new DateTime(2024, 1, 30, 17, 0, 0)))
                        .AddAppointment(new Appointment(Guid.NewGuid(),Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 13, 0, 0), new DateTime(2024, 1, 30, 13, 30, 0))))
                        .AddAppointment(new Appointment(Guid.NewGuid(),Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 15, 45, 0), new DateTime(2024, 1, 30, 16, 15, 0)))),
                13
            }
        };

        public static IEnumerable<object[]> GetAvailabilities()
        {
            foreach (var availability in availabilities)
            {
                yield return availability;
            }
        }

        [Theory]
        [MemberData(nameof(GetAvailabilities))]
        public void GetAvailableTimeSlots(Availability availability, int count)
        {
            Assert.Equal(count, availability.GetAvailableTimeSlots().Count());
        }

        [Fact]
        public void UpdateAvailablityTimeSlot_ShouldSucceed()
        {
            //Arrange
            var availability = new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(new DateTime(2024, 1, 30, 9, 0, 0), new DateTime(2024, 1, 30, 17, 0, 0)));
            availability.AddAppointment(new Appointment(Guid.NewGuid(), Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 13, 0, 0), new DateTime(2024, 1, 30, 13, 30, 0))));

            //Act 
            var updatedStartTime = new DateTime(2024, 1, 30, 11, 0, 0);
            var updatedEndTime = new DateTime(2024, 1, 30, 17, 0, 0);
            availability.UpdateAvailablityTimeSlot(new AvailabilityTimeSlotValueObject(updatedStartTime, updatedEndTime));

            //Assert
            Assert.Equal(availability.AvailabilityTimeSlot.StartTime, updatedStartTime);
            Assert.Equal(availability.AvailabilityTimeSlot.EndTime, updatedEndTime);
            Assert.Equal(1, availability.Appointments.Count());
        }

        [Fact]
        public void UpdateAvailablityTimeSlot_ShouldThrowException_WhenUpdatedAvailabilityDoesNotContainExistingAppointment()
        {
            //Arrange
            var availability = new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(new DateTime(2024, 1, 30, 9, 0, 0), new DateTime(2024, 1, 30, 17, 0, 0)));
            availability.AddAppointment(new Appointment(Guid.NewGuid(), Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 13, 0, 0), new DateTime(2024, 1, 30, 13, 30, 0))));

            //Act 
            var updatedStartTime = new DateTime(2024, 1, 30, 14, 0, 0);
            var updatedEndTime = new DateTime(2024, 1, 30, 17, 0, 0);
            //Act & Assert
            Assert.Throws<TimeSlotOutOfAvailablityException>(() =>
            {
                availability.UpdateAvailablityTimeSlot(new AvailabilityTimeSlotValueObject(updatedStartTime, updatedEndTime));
            });

            //Assert
            Assert.Equal(1, availability.Appointments.Count());
        }

        [Fact]
        public void AddAppointment_ShouldSucceed()
        {
            //Arrange
            var availability = new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(new DateTime(2024, 1, 30, 9, 0, 0), new DateTime(2024, 1, 30, 17, 0, 0)));

            availability.AddAppointment(new Appointment(Guid.NewGuid(), Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 13, 0, 0), new DateTime(2024, 1, 30, 13, 30, 0))));

            //Act 
            availability.AddAppointment(new Appointment(Guid.NewGuid(), Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 15, 0, 0), new DateTime(2024, 1, 30, 15, 45, 0))));

            //Assert
            Assert.Equal(2, availability.Appointments.Count());
        }


        [Fact]
        public void AddAppointment_ShouldThrowException_WhenAddingOverlappingAppointment()
        {
            //Arrange
            var availability = new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(new DateTime(2024, 1, 30, 9, 0, 0), new DateTime(2024, 1, 30, 17, 0, 0)));

            availability.AddAppointment(new Appointment(Guid.NewGuid(), Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 13, 0, 0), new DateTime(2024, 1, 30, 13, 30, 0))));


            //Act & Assert
            Assert.Throws<OverlappingAppointmentException>(() =>
            {
                availability.AddAppointment(new Appointment(Guid.NewGuid(), Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 13, 0, 0), new DateTime(2024, 1, 30, 13, 45, 0))));
            });
        }

        [Fact]
        public void AddAppointment_ShouldThrowException_WhenAddingAppointmentOutOfAvailability()
        {
            //Arrange
            var availability = new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(new DateTime(2024, 1, 30, 9, 0, 0), new DateTime(2024, 1, 30, 17, 0, 0)));

            //Act & Assert
            Assert.Throws<TimeSlotOutOfAvailablityException>(() =>
            {
                availability.AddAppointment(new Appointment(Guid.NewGuid(), Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 17, 0, 0), new DateTime(2024, 1, 30, 17, 45, 0))));
            });
        }
    }
}