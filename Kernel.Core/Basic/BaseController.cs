using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.Basic
{
    [Route("api/v{version:apiVersion}/{area}/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
