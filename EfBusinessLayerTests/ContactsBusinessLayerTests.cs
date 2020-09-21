using System.Threading;
using System.Threading.Tasks;
using EfBusinessLayer;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EfBusinessLayerTests
{
    public class ContactsBusinessLayerTests
    {
        readonly Mock<IRepository<Contact>> contactRepositoryMock = new Mock<IRepository<Contact>>();
        readonly Mock<DbSet<Contact>> contactDbSetMock = new Mock<DbSet<Contact>>();
        readonly Mock<IValidator<Contact>> contactValidatorMock = new Mock<IValidator<Contact>>(MockBehavior.Strict);
        readonly Mock<ILogger> loggerMock = new Mock<ILogger>();
        readonly ContactsBusinessLayer contactsBusinessLayer;

        public ContactsBusinessLayerTests()
        {
            contactsBusinessLayer = new ContactsBusinessLayer(
                contactRepositoryMock.Object,
                contactValidatorMock.Object,
                loggerMock.Object);
        }

        [Fact]
        public async Task Should_add_if_valid()
        {
            // arrange
            var contact = new Contact { Name = "Arthur" };

            contactRepositoryMock
                .Setup(repository => repository.Set())
                .Returns(contactDbSetMock.Object);

            contactValidatorMock
                .Setup(validator => validator.Validate(contact))
                .Returns(true);

            // act
            await contactsBusinessLayer.AddAsync(contact);

            // assert
            contactValidatorMock
                .Verify(contactValidator => contactValidator.Validate(contact),
                    Times.Once);

            contactDbSetMock
                .Verify(dbSet => dbSet.Add(contact),
                    Times.Once);

            contactRepositoryMock
                .Verify(repository => repository.SaveChangesAsync(new CancellationToken()),
                    Times.Once);

            VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Should_log_error_and_not_add_if_invalid()
        {
            // arrange
            var contact = new Contact();
            var expectedErrorMessage = $"Validation failed for contact {contact}.";

            contactValidatorMock
                .Setup(validator => validator.Validate(contact))
                .Returns(false);

            // act
            await contactsBusinessLayer.AddAsync(contact);

            // assert
            contactValidatorMock
                .Verify(contactValidator => contactValidator.Validate(contact),
                    Times.Once);

            loggerMock
                .Verify(logger => logger.LogError(expectedErrorMessage),
                    Times.Once);

            VerifyNoOtherCalls();
        }

        void VerifyNoOtherCalls()
        {
            contactValidatorMock.VerifyNoOtherCalls();
            contactRepositoryMock.VerifyNoOtherCalls();
            contactDbSetMock.VerifyNoOtherCalls();
            loggerMock.VerifyNoOtherCalls();
        }
    }
}