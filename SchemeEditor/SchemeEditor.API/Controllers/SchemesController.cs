using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchemeEditor.Abstraction.Application.Models;
using SchemeEditor.Abstraction.Application.Services;
using SchemeEditor.Application.Infrastructure;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.API.Controllers
{
    [Authorize]
    public class SchemesController : BaseController
    {
        private readonly ISchemeService _schemeService;

        public SchemesController(ISchemeService schemeService, IExecutionContext<User> executionContext, UserManager<User> userManager)
	        : base(executionContext, userManager)
        {
            _schemeService = schemeService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Authorize(Roles = "ADMINISTRATOR,MODERATOR,USER,NOT ACTIVE")]
        public async Task<ActionResult<List<SchemeView>>> Get()
        {
            var schemes = await _schemeService.List(this.ExecutionContext.User.Id);
            return Ok(schemes);
        }

        [HttpGet("{schemeId}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "ADMINISTRATOR,MODERATOR,USER,NOT ACTIVE")]
        public async Task<ActionResult<SchemeView>> Get(long schemeId)
        {
            var schemeView = await _schemeService.GetAsync(schemeId);
            return Ok(schemeView);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Authorize(Roles = "ADMINISTRATOR,MODERATOR,USER,NOT ACTIVE")]
        public async Task<ActionResult<SchemeView>> Post(SchemeView model)
        {
            return await _schemeService.CreateAsync(this.ExecutionContext.User.Id, model);
        }

        //[HttpPost("draft")]
        //[ProducesResponseType(200)]
        //[Authorize(Roles = "ADMINISTRATOR,MODERATOR,USER,NOT ACTIVE")]
        //public async Task<ActionResult<SchemeView>> PostDraft()
        //{
        //    return await _schemeService.CreateDraftScheme(this.ExecutionContext.User.Id);
        //}

        [HttpPut]
        [ProducesResponseType(200)]
        [Authorize(Roles = "ADMINISTRATOR,MODERATOR,USER")]
        public async Task<ActionResult<SchemeView>> Put(SchemeView model)
        {
            var schemeView = await _schemeService.UpdateAsync(this.ExecutionContext.User.Id, model);
            return Ok(schemeView);
        }

        [HttpPut("restore")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "ADMINISTRATOR,MODERATOR,USER")]
        public async Task<ActionResult<SchemeView>> Restore(long schemeId)
        {
            var schemeView = await _schemeService.RestoreAsync(this.ExecutionContext.User.Id, schemeId);
            return Ok(schemeView);
        }

        [HttpDelete("{schemeId}")]
        [Authorize(Roles = "ADMINISTRATOR,MODERATOR,USER")]
        public async Task<IActionResult> Delete(long schemeId)
        {
            var scheme = await _schemeService.DeleteAsync(this.ExecutionContext.User.Id, schemeId);
            return Ok(scheme);
        }
        
        [HttpDelete("{schemeId}/permanent")]
        [Authorize(Roles = "ADMINISTRATOR,MODERATOR,USER")]
        public async Task<IActionResult> DeletePermanent(long schemeId)
        {
            await _schemeService.DeletePermanentAsync(schemeId);
            return Ok();
        }
    }
}
