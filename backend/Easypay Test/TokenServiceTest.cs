using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Easypay_App.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easypay_Test
{
    public class TokenServiceTest
    {
        ITokenService _tokenService;
        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Tokens:JWT"] = "THIS_IS_A_32_BYTE_SECRET_KEY_123456"
                }).Build();

            _tokenService = new TokenService(config);
        }
        [Test]
        public void TokenTest()
        {
            var user = new LoginResponseDTO
            {
                UserName = "test",
                Role = "Tester"
            };

            var result = _tokenService.GenerateToken(user);

            Assert.That(result, Is.Not.Null);
        }

        [TearDown]
        public void TearDown() 
        { 
        }
    }
}
