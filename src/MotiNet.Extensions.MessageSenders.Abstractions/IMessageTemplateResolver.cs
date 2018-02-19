namespace MotiNet.MessageSenders
{
    public interface IMessageTemplateResolver<out TMarker> : IMessageTemplateResolver { }

    public interface IMessageTemplateResolver
    {
        string ResolveTemplate(string location, string templateName);
    }
}
