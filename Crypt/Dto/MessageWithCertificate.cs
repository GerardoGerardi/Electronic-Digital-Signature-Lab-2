namespace Common.Dto
{
    public class MessageWithCertificate//DTO для верификации
    {
        public string OriginalMessage { get; set; }
        public string SignedMessage { get; set; }
        public string PublicKey { get; set; }
    }
}
