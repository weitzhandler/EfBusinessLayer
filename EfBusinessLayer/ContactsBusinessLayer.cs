using System.Threading.Tasks;

namespace EfBusinessLayer
{
    public class ContactsBusinessLayer : IBusinessLayer<Contact>
    {
        private readonly IRepository<Contact> repository;
        private readonly IValidator<Contact> validator;
        private readonly ILogger logger;

        public ContactsBusinessLayer(IRepository<Contact> repository, IValidator<Contact> validator, ILogger logger)
        {
            this.repository = repository;
            this.validator = validator;
            this.logger = logger;
        }

        public async Task AddAsync(Contact entity)
        {
            if (!validator.Validate(entity))
            {
                this.logger.LogError($"Validation failed for contact {entity}.");
                return;
            }

            repository.Set().Add(entity);

            await repository.SaveChangesAsync();
        }
    }
}
