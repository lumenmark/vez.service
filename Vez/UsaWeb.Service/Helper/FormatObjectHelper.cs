namespace UsaWeb.Service.Helper
{
    public static class FormatObjectHelper
    {
        public static object RemoveNullProperties(object obj)
        {
            if (obj == null) return null;

            var properties = obj.GetType().GetProperties()
                .Where(p => p.GetValue(obj) != null) // Only include non-null properties
                .ToDictionary(p => p.Name, p => p.GetValue(obj));

            return properties;
        }
    }
}
