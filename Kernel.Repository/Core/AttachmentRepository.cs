﻿using Dapper;
using Kernel.Dapper.Repository;
using Kernel.IService.Repository.Core;
using Kernel.Model.Core;
using Kernel.Model.Core.Attachment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kernel.Repository.Core
{
    public class AttachmentRepository : BaseRepository<SysAttachments>, IAttachmentRepository
    {
        public override string DBName => DapperConst.QYPT_ORACLE;

        public async Task<SysAttachments> GetAttachment_V1_0(string attachID)
        {
            try
            {
                using (var conn = Connection)
                {
                    return await conn.GetAsync<SysAttachments>(attachID);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async void AddAttachment_V1_0(SysAttachments attachment)
        {
            try
            {
                using (var conn = Connection)
                {
                    await conn.InsertAsync<string, SysAttachments>(attachment);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<SysAttachmentsOutParams>> GetAttachmentList_V1_0(string bizID)
        {
            try
            {
                using (var conn = Connection)
                {
                    return await conn.GetListAsync<SysAttachmentsOutParams>("WHERE ATTACH_BIZ_ID = :ATTACH_BIZ_ID", new { ATTACH_BIZ_ID = bizID });
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<SysAttachmentsOutParams>> GetAttachmentList_V1_0(string[] attachIDs)
        {
            try
            {
                using (var conn = Connection)
                {
                    return await conn.GetListAsync<SysAttachmentsOutParams>("WHERE ATTACH_ID IN :ATTACH_ID", new { ATTACH_ID = attachIDs });
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<int> DeleteAttachment_V1_0(string[] attachIDs)
        {
            try
            {
                using (var conn = Connection)
                {
                    return await conn.DeleteListAsync<SysAttachments>("WHERE ATTACH_ID IN :ATTACH_ID", new { ATTACH_ID = attachIDs });
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> DeleteAttachment_V1_0(string bizID)
        {
            try
            {
                using (var conn = Connection)
                {
                    return await conn.DeleteListAsync<SysAttachments>("WHERE ATTACH_BIZ_ID = :ATTACH_BIZ_ID", new { ATTACH_BIZ_ID = bizID });
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
