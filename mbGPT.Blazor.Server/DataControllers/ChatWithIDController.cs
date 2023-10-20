using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.WebApi.Services;
using mbGPT.Module.BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mbGPT.Blazor.Server.DataControllers
{
    /// <summary>
    /// Used to create a new Chat while returning the ID of the created chat
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatWithIDController : ControllerBase
    {
        private readonly ISecurityProvider _securityProvider;
        private readonly IDataService _dataService;
        public ChatWithIDController(IDataService dataService, ISecurityProvider securityProvider)
        {
            _dataService = dataService;
            _securityProvider = securityProvider;
        }
        [HttpPost]
        public IActionResult CreateChatWitId([FromBody] Chat mychat)
        {
            var strategy = (SecurityStrategy)_securityProvider.GetSecurity();
            if (!strategy.CanCreate(typeof(Chat)))
                return Forbid("You do not have permissions to add a new chat!");
            var objectSpace = _dataService.GetObjectSpace(typeof(Chat));

            var newChat = objectSpace.CreateObject<Chat>();
            newChat.Question = mychat.Question;
            Prompt prompt = (Prompt) objectSpace.GetObjectByKey(typeof(Prompt), mychat.Prompt.PromptID);
            newChat.Prompt = prompt;
            objectSpace.CommitChanges();
            return Ok(newChat.ChatId);
        }
    }
}
