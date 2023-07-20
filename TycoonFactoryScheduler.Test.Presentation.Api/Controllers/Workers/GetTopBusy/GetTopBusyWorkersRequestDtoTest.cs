using TycoonFactoryScheduler.Infrastructure.StaticAdapters;
using TycoonFactoryScheduler.Infrastructure.Validation;
using TycoonFactoryScheduler.Presentation.Api.Controllers.Workers.GetTopBusy;

namespace TycoonFactoryScheduler.Test.Presentation.Api.Controllers.Workers.GetTopBusy
{
    [TestFixture]
    [Category(Category.Unit)]
    public class GetTopBusyWorkersRequestDtoTest
    {

        [Test]
        public void Constructor_With_No_Values_Creates_Valid_Object()
        {
            var dto = new GetTopBusyWorkersRequestDto();
            Assert.That(dto.IsValid());
        }

        [Test]
        public void Constructor_With_No_Values_Creates_WithParams_DateTimeNow_DateTimeNow_10()
        {
            var start = new DateTime(2023, 1, 1);
            var end = new DateTime(2023, 1, 8);
            DateTimeProvider.SetNow(() => start);

            var dto = new GetTopBusyWorkersRequestDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.Start, Is.EqualTo(start));
                Assert.That(dto.End, Is.EqualTo(end));
                Assert.That(dto.Size, Is.EqualTo(10));
            });
        }

        [Test]
        public void Constructor_With_Size_LowerThanOne_Creates_NotValid_Object()
        {
            var dto = new GetTopBusyWorkersRequestDto
            {
                Size = 0
            };

            Assert.That(dto.IsNotValid());
        }

        [Test]
        public void Constructor_With_Size_LowerThanOne_Then_Validate_Returns_ErrorMessage_Containing_GreaterThanZero()
        {
            var dto = new GetTopBusyWorkersRequestDto
            {
                Size = 0
            };
            var errors = dto.GetValidationErrorsMessages();

            Assert.That(errors.Any(error => error?.Contains("greater than zero") ?? false));
        }

        [Test]
        public void GetTopBusy_WithParameter_EndDate_Before_StartDate_Returns_ErrorMessage_Containing_After()
        {
            var dto = new GetTopBusyWorkersRequestDto
            {
                Start = new DateTime(2023, 1, 2),
                End = new DateTime(2023, 1, 1),
            };
            var errors = dto.GetValidationErrorsMessages();

            Assert.That(errors.Any(error => error?.Contains("after") ?? false));
        }
    }
}
