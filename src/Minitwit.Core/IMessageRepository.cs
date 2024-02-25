namespace Chirp.Core;

public interface IMessageRepository
{
    Task<IEnumerable<MessageDto>> GetMessages(int pageRange);

    Task<IEnumerable<MessageDto>> GetMessagesFromUser(string username, int pageRange);
    Task<IEnumerable<MessageDto>> GetPersonalMessages(int userId, int pageRange);
    Task CreateMessage(string text, int id);
}