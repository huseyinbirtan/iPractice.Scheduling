using iPractice.Scheduling.Domain.Exceptions;
using iPractice.Scheduling.Domain.ScheduleAggregate;
using iPractice.Scheduling.Domain.ValueObjects;

namespace iPractice.Scheduling.Tests.ScheduleAggregateTests
{
    public class ScheduleTests
    {
        private Availability availability_1 = new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(new DateTime(2024, 1, 30, 9, 0, 0), new DateTime(2024, 1, 30, 17, 0, 0)))
                                    .AddAppointment(new Appointment(Guid.NewGuid(), Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 13, 0, 0), new DateTime(2024, 1, 30, 13, 30, 0))))
                                    .AddAppointment(new Appointment(Guid.NewGuid(), Guid.NewGuid(), 1, new TimeSlotValueObject(new DateTime(2024, 1, 30, 15, 45, 0), new DateTime(2024, 1, 30, 16, 15, 0))));

        private Availability availability_2 = new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(new DateTime(2024, 2, 1, 9, 0, 0), new DateTime(2024, 2, 1, 17, 0, 0)));

        [Fact]
        public void CreateAvailability_ShouldSucceed()
        {
            //Arrange
            var schedule = new Schedule(Guid.NewGuid(), 1);
            schedule.CreateAvailability(availability_1).CreateAvailability(availability_2);

            //Act 
            var availabilityStartTime = new DateTime(2024, 2, 2, 11, 0, 0);
            var availabilityEndTime = new DateTime(2024, 2, 2, 17, 0, 0);

            schedule.CreateAvailability(new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(availabilityStartTime, availabilityEndTime)));

            //Assert
            Assert.Equal(3, schedule.Availabilities.Count());
        }

        [Fact]
        public void CreateAvailability_ShouldThrowException_WhenCreatingOverlappingAvailabilities()
        {
            //Arrange
            var schedule = new Schedule(Guid.NewGuid(), 1);
            schedule.CreateAvailability(availability_1).CreateAvailability(availability_2);

            //Act 
            var availabilityStartTime = new DateTime(2024, 2, 1, 11, 0, 0);
            var availabilityEndTime = new DateTime(2024, 2, 1, 17, 0, 0);

            //Act & Assert
            Assert.Throws<OverlappingAvailablityException>(() =>
            {
                schedule.CreateAvailability(new Availability(Guid.NewGuid(), Guid.NewGuid(), 1, new AvailabilityTimeSlotValueObject(availabilityStartTime, availabilityEndTime)));
            });

            //Assert
            Assert.Equal(2, schedule.Availabilities.Count());
        }

        [Fact]
        public void GetAvailableTimeSlots_ShouldMatchTheCount()
        {
            //Arrange
            var schedule = new Schedule(Guid.NewGuid(), 1);
            schedule.CreateAvailability(availability_1)
                    .CreateAvailability(availability_2);

            //Assert
            Assert.Equal(29, schedule.GetAvailableTimeSlots().Count());
        }
    }
}