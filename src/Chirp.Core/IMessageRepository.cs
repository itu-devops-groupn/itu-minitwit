namespace Chirp.Core;

public interface IMessageRepository
{
    Task<IEnumerable<MessageDto>> GetMessages(int pageRange);
}