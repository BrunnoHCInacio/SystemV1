using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;

namespace SystemV1.RegistersAPI.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        public ActionResult OkResult(object result = null)
        {
            if (result != null)
            {
                return Ok(new
                {
                    result
                });
            }
            return Ok();
        }
    }
}