namespace Stalkr.In
{
    public interface IChecksumStalkr
    {
        string GetSha256Digest(string content);
    }
}