using Microsoft.AspNetCore.Mvc;

namespace MyJournal.WebApi.Attributes
{
    public class UpdateActivityAttribute : TypeFilterAttribute
    {
        public UpdateActivityAttribute() : base(typeof(UpdateActivityFilter))
        {
        }
    }
}