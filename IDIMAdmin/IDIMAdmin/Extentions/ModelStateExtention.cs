using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IDIMAdmin.Extentions
{
    public static class ModelStateExtention
    {
        public static IEnumerable<string> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage + " " + v.Exception).ToList();

        }
    }
}