using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gicaf.Application.Interface.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gicaf.API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TAppService, TDTO, TId> : ControllerBase where TAppService: IAppService<TDTO>
    {
        protected TAppService _appService;

        public BaseController(TAppService appService)
        {
            _appService = appService;
        }

        // GET: api/ControllerBase
        [HttpGet]
        public virtual IEnumerable<TDTO> Get()
        {
            return _appService.GetAll();
        }

        // GET: api/ControllerBase/5
        [HttpGet("{id}", Name = "Get")]
        public virtual TDTO Get(long id)
        {
            return _appService.Get(id);
        }
        
        // PUT: api/ControllerBase/5
        [HttpPut("{id}")]
        public virtual void Put(long id, [FromBody] TDTO value)
        {
            _appService.Update(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public virtual void Delete(long id)
        {
            _appService.Remove(id);
        }
    }
}
