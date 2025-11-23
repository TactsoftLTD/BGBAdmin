namespace IDIMAdmin.Extentions
{
	public static class StringExtention
    {
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}