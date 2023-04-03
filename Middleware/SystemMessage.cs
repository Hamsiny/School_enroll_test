using Newtonsoft.Json;

namespace UxtrataTask.Middleware;

public class SystemMessage
{
    public string Message { get; }
    public string MessageType { get; }
    
    [JsonConstructor]
    public SystemMessage(string message, SystemMessageType type = SystemMessageType.Error) {
        Message = message;
        // MessageType = type.GetStringValue().ToLower();
    }
    
    public static SystemMessage GenericError() {
        return new SystemMessage("Unable to process request. Please try again.");
    }
}