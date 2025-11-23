namespace IDIMAdmin.Extentions.Data
{
	public static class JsonMessages
    {
        public static JsonMessage Message(string message, bool success = false, JsonMessageType type = JsonMessageType.Success, object data = null)
        {
            return new JsonMessage { Message = message, Success = success, Type = type, Data = data };
        }

        public static JsonMessage Success(string message, JsonMessageType type = JsonMessageType.Success, object data = null)
        {
            return Message(message, true, type, data);
        }

        public static JsonMessage Failed(string message, JsonMessageType type = JsonMessageType.Success, object data = null)
        {
            return Message(message, false, type, data);
        }
    }
}
