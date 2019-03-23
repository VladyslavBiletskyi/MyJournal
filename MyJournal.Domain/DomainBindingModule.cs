using System;
using Microsoft.Extensions.DependencyInjection;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Domain
{
    public class DomainBindingModule: IBindingModule
    {
        public void ApplyBindings(IServiceCollection services)
        {
            return;
        }
    }
}