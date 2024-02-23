namespace Chirp.Core;

public interface IMessageRepository
{
    Task<IEnumerable<MessageDto>> GetMessages();
    string FormatDateTime(int timestamp);
}