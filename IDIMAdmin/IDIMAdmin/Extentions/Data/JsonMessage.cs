namespace IDIMAdmin.Extentions.Data
{
    public class JsonMessage
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public JsonMessageType Type { get; set; } = JsonMessageType.Success;
        public object Data { get; set; } = new object();
    }
}
