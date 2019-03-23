using Microsoft.Extensions.DependencyInjection;

namespace MyJournal.Domain.Extensibility
{
    public interface IBindingModule
    {
        void ApplyBindings(IServiceCollection services);
    }
}
