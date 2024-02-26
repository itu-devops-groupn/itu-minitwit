namespace Minitwit.Infrastructure;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic;

using Minitwit.Core;

public class MessageRepository : IMessageRepository
{
    private readonly MinitwitContext _context;

    public MessageRepository(MinitwitContext context)
    {
        _context = context;
    }

    private static string FormatDateTime(int timestamp)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(timestamp)
            .ToLocalTime()
            .ToString("yyyy-MM-dd @ HH:mm");
    }

    public async Task CreateMessage(string text, int id)
    {
        var newMessage = new Message
        {
            Author_id = id,
            Text = text,
            Pub_date = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds
        };

        await _context.Messages.AddAsync(newMessage);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MessageDto>> GetMessages(int pageRange)
    {
        var messages = await _context.Messages
            .Join(_context.Users,
                message => message.Author_id,
                user => user.User_id,
                (message, user) => new { Message = message, User = user })
            .OrderByDescending(cont => cont.Message.Pub_date)
            .Take(pageRange)
            .Where(cont => cont.Message.Flagged == 0)
            .ToListAsync();

        return messages.Select(cont => new MessageDto(cont.Message.Text, cont.User.Username, FormatDateTime(cont.Message.Pub_date)));
    }

    public async Task<IEnumerable<MessageDto>> GetMessagesFromUser(string username, int pageRange)
    {
        var messages = await _context.Messages
            .Join(_context.Users,
                message => message.Author_id,
                user => user.User_id,
                (message, user) => new { Message = message, User = user })
            .OrderByDescending(cont => cont.Message.Pub_date)
            .Take(pageRange)
            .Where(cont => cont.Message.Flagged == 0 && cont.User.Username == username)
            .ToListAsync();

        return messages.Select(cont => new MessageDto(cont.Message.Text, cont.User.Username, FormatDateTime(cont.Message.Pub_date)));
    }

    public async Task<IEnumerable<MessageDto>> GetPersonalMessages(int userId, int pageRange)
    {
        var followedUsers = _context.Followers
            .Where(f => f.Who_id == userId)
            .Select(f => f.Whom_id);
        
        var messages = await _context.Messages
            .Join(_context.Users,
                message => message.Author_id,
                user => user.User_id,
                (message, user) => new { Message = message, User = user })
            .OrderByDescending(cont => cont.Message.Pub_date)
            .Take(pageRange)
            .Where(cont => cont.Message.Flagged == 0 && (cont.Message.Author_id == userId || followedUsers.Contains(cont.Message.Author_id)))
            .ToListAsync();

        return messages.Select(cont => new MessageDto(cont.Message.Text, cont.User.Username, FormatDateTime(cont.Message.Pub_date)));
    }
}