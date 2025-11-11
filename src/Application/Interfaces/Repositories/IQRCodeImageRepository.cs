namespace Application.Interfaces.Repositories
{
    public interface IQRCodeImageRepository
    {
        byte[] GenerateQRCodeImage(string content);
    }
}
