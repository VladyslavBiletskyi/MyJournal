using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Domain.Repositories
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override bool TryUpdateInstance(Message instance)
        {
            var original = Find(instance.Id);
            if (original == null)
            {
                return false;
            }

            original.Text = instance.Text;
            original.Read = instance.Read;
            DatabaseContext.SaveChanges();
            return true;
        }
    }
}
