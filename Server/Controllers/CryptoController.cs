using Common.Dto;
using Common.InstanceManagement;
using Common.TextGeneration;
using Crypt;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    public class CryptoController : ControllerBase
    {

        private readonly IInstanceManager _instanceManager;
        private readonly ICryptMaster _cryptMaster;
        private readonly ILogger<CryptoController> _logger;

        public CryptoController(ILogger<CryptoController> logger,ICryptMaster cryptMaster,IInstanceManager instanceManager)
        {
            _logger = logger;
            _cryptMaster = cryptMaster;
            _instanceManager = instanceManager;
        }

        [HttpPost]
        [Route("VerificateMessageByService")]
        public async  Task<IActionResult> VerificateMessage(MessageWithCertificate body)//����������� ����������� ��������� ���������� ������, ������ true ���� ��� ������
        {
            return StatusCode(StatusCodes.Status200OK, _cryptMaster.VerifyData(body.OriginalMessage, body.SignedMessage, body.PublicKey.ToPublicKey()));
        }

        [HttpGet]
        [Route("GetKey")]
        public async Task<IActionResult> GetKey()//��������� ����� ��� ������������, ������ ��������� ���� � string
        {
            try
            {

                var id = _instanceManager.Add();
                return StatusCode(StatusCodes.Status200OK, id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "���������� �����");
            }
        }

        [HttpPost]
        [Route("GetText")]
        public async Task<IActionResult> GetText(KeyDTO key)//��������� ������ ��� ������������, ������ ����������� �����
        {
            var result = _instanceManager[key.Key];
            if(result is null)
                _logger.LogError($"����� �� ������ {_instanceManager.Count}");
            return StatusCode(result is null? StatusCodes.Status204NoContent:StatusCodes.Status200OK, result);
        }
    }
}