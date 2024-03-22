using Common.Dto;
using Common.TextGeneration;
using Crypt;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ApiTest
{
    public class UnitTest1
    {
        string url = "https://localhost:5001/";
        string GetKeyAddress = "GetKey";
        string GetTextAddress = "GetText";
        string VerificateMessageByServiceAddress = "VerificateMessageByService";

        ITextGenerator _textGenerator;
        ITextGenerator TextGenerator => _textGenerator ?? (_textGenerator = new TextGenerator(null));
        ICryptMaster CryptMaster =>  new RSAMaster();

        HttpClient Client => new HttpClient() { BaseAddress = new Uri(url) };


        [Fact]
        public void Script1Success()//�������� 1 �������
        {
            var originalMessage = TextGenerator.GenerateText();
            var keys = CryptMaster.GetKeys();
            var signedMessage = CryptMaster.SignContent(originalMessage, keys.Value);
            var messageWithCertificate = new MessageWithCertificate()
            {
                OriginalMessage = originalMessage,
                PublicKey = keys.Key.ToEncodedString(),
                SignedMessage = signedMessage
            };
            var text = JsonConvert.SerializeObject(messageWithCertificate);
            var stringContent = new StringContent(text, Encoding.UTF8, "application/json");
            using(var client=Client)
            {
                var result = Convert.ToBoolean(client.PostAsync(VerificateMessageByServiceAddress,stringContent).Result.Content.ReadAsStringAsync().Result);
                Assert.True(result);
            }
        }

        [Fact]
        public void Script1WithWrongOriginalMessage()// �������� 1, 
        {
            var originalMessage = TextGenerator.GenerateText();
            var keys = CryptMaster.GetKeys();
            var signedMessage = CryptMaster.SignContent(originalMessage, keys.Value);
            var messageWithCertificate = new MessageWithCertificate()
            {
                OriginalMessage = TextGenerator.GenerateText(),//������� ��������� �� ������
                PublicKey = keys.Key.ToEncodedString(),
                SignedMessage = signedMessage
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(messageWithCertificate), Encoding.UTF8, "application/json");
            using (var client = Client)
            {
                var result = Convert.ToBoolean(client.PostAsync(VerificateMessageByServiceAddress, stringContent).Result.Content.ReadAsStringAsync().Result);
                Assert.False(result);
            }
        }

        [Fact]
        public void Script2Success()//�������� 2 �������
        {
            using (var client = Client)
            {
                var keyString = (client.GetStringAsync(GetKeyAddress).Result);
                var keyModel = new KeyDTO() { Key = keyString };
                var stringContent=new StringContent(JsonConvert.SerializeObject(keyModel), Encoding.UTF8, "application/json");
                var message = JsonConvert.DeserializeObject<MessageWithCertificate>(client.PostAsync(GetTextAddress,stringContent).Result.Content.ReadAsStringAsync().Result);
                var result = CryptMaster.VerifyData(message.OriginalMessage, message.SignedMessage, message.PublicKey.ToPublicKey());
                Assert.True(result);
            }
        }

        [Fact]
        public void Script2WrongOriginalTextByLocal()//�������� 2 ������� ������
        {
            using (var client = Client)
            {
                var keyString = (client.GetStringAsync(GetKeyAddress).Result);
                var keyModel = new KeyDTO() { Key = keyString };
                var stringContent = new StringContent(JsonConvert.SerializeObject(keyModel), Encoding.UTF8, "application/json");
                var message = JsonConvert.DeserializeObject<MessageWithCertificate>(client.PostAsync(GetTextAddress,stringContent).Result.Content.ReadAsStringAsync().Result);
                var result = CryptMaster.VerifyData(TextGenerator.GenerateText(),//������� ������
                    message.SignedMessage,
                    message.PublicKey.ToPublicKey());
                Assert.False(result);
            }
        }

        [Fact]
        public void Script2WrongOriginalTextByService()//�������� 2 ������� ������
        {
            using (var client = Client)
            {
                var keyString = (client.GetStringAsync(GetKeyAddress).Result);
                var keyModel = new KeyDTO() { Key = keyString };
                var stringContent = new StringContent(JsonConvert.SerializeObject(keyModel), Encoding.UTF8, "application/json");
                var oldmessage = JsonConvert.DeserializeObject<MessageWithCertificate>(client.PostAsync(GetTextAddress,stringContent).Result.Content.ReadAsStringAsync().Result);

                var secondKeyString = (client.GetStringAsync(GetKeyAddress).Result);
                keyModel = new KeyDTO() { Key = secondKeyString };
                stringContent = new StringContent(JsonConvert.SerializeObject(keyModel), Encoding.UTF8, "application/json");
                var message = JsonConvert.DeserializeObject<MessageWithCertificate>(client.PostAsync(GetTextAddress,stringContent).Result.Content.ReadAsStringAsync().Result);
                var result = CryptMaster.VerifyData(message.OriginalMessage,//����� �����
                    oldmessage.SignedMessage,//������ ����������� �����
                    oldmessage.PublicKey.ToPublicKey()); //������ ��������� ����
                Assert.False(result);
            }
        }
    }
}