/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020-04-24       初始版本

******************************************************************/
using Kernel.Core.AOP;
using Kernel.MediatR.Core.Attachment.V1_0;
using Kernel.Model.Core.Attachment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Areas.Core.Controllers
{
    [ApiVersion("1.0")]
    //[Authorize]
    public class FileController : CoreBaseController
    {
        private IMediator _mediator;

        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 打包下载文件
        /// </summary>
        [HttpGet]
        [Route("zip/{bizID}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> DownloadZip_V1_0(string bizID)
        {
            SysAttachmentsInParams data = new SysAttachmentsInParams
            {
                AttachBizId = bizID
            };
            var result = await _mediator.Send(new DownloadZipCommand(data));
            return Ok(result);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        [HttpGet]
        [Route("download/{attachID}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> DownloadFile_V1_0(string attachID)
        {
            SysAttachmentsInParams data = new SysAttachmentsInParams
            {
                AttachId = attachID
            };
            var result = await _mediator.Send(new DownFileCommand(data));

            if (result == null)
                throw new KernelException("文件不存在！");
            else
                return File(result.Data.Content, result.Data.ContentType, result.Data.FileName);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        [HttpGet]
        [Route("files/{bizID}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetFiles_V1_0(string bizID)
        {
            SysAttachmentsInParams data = new SysAttachmentsInParams
            {
                AttachBizId = bizID
            };
            var result = await _mediator.Send(new GetFilesCommand(data));
            return Ok(result);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        [HttpGet]
        [Route("file/{attachID}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetFile_V1_0(string attachID)
        {
            SysAttachmentsInParams data = new SysAttachmentsInParams
            {
                AttachId = attachID
            };
            var result = await _mediator.Send(new GetFileCommand(data));
            return Ok(result);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        [HttpPost]
        [Route("file"), MapToApiVersion("1.0")]
        public async Task<IActionResult> UploadFiles_V1_0(string bizID, IFormFile formFile)
        {
            SysAttachmentsInParams data = new SysAttachmentsInParams
            {
                Files = Request.Form.Files,
                AttachBizId = bizID
            };
            var result = await _mediator.Send(new UploadFilesCommand(data));
            return Ok(result);

        }

        /// <summary>
        /// 删除文件，按业务ID删除
        /// </summary>
        [HttpDelete]
        [Route("file/{bizID}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteFilesByBizID_V1_0(string bizID)
        {
            SysAttachmentsInParams data = new SysAttachmentsInParams
            {
                AttachBizId = bizID
            };
            var result = await _mediator.Send(new DeleteFilesByBizIDCommand(data));
            return Ok(result);
        }

        /// <summary>
        /// 删除文件，按文件ID删除（多个ID以逗号分隔）
        /// </summary>
        [HttpDelete]
        [Route("file"), MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteFilesByAttachIDs_V1_0(string attachIDs)
        {
            SysAttachmentsInParams data = new SysAttachmentsInParams
            {
                AttachId = attachIDs
            };
            var result = await _mediator.Send(new DeleteFilesCommand(data));
            return Ok(result);
        }

    }
}
