using MyJournal.Domain.Entities;

namespace MyJournal.Domain.Extensibility.Repositories
{
    public interface ILessonSkipRepository : IRepositoryBase<LessonSkip>, IBatchInsertRepository<LessonSkip>
    {
    }
}
