using Newtonsoft.Json;

namespace VacationRental.Api.Application.Extensions
{
    public static class ObjectExtensions
    {
        public static T DeepCopy<T>(this T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}