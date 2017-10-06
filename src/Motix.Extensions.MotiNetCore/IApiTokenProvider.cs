namespace MotiNet
{
    public interface IApiTokenProvider
    {
        void GenerateToken(string secretKey, string command, string data, double timeOut, out long timeStamp, out string token);

        bool ValidateToken(string secretKey, string command, string data, long timeStamp, string token);
    }
}
