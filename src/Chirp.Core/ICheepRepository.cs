namespace Chirp.Core;

public interface ICheepRepository
{
    Task<IEnumerable<CheepDto>> GetCheeps(int pageIndex, int pageRange);
    Task<IEnumerable<CheepDto>> GetCheepsFromAuthor(string authorName, int pageIndex, int pageRange);
    Task<IEnumerable<CheepDto>> GetPersonalCheeps(string authorName, int pageIndex, int pageRange);
    Task CreateCheep(CreateCheepDto cheep);
    Task DeleteCheep(Guid cheepId);
}