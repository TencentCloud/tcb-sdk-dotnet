using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class Storage
  {
    private Core core;

    public Storage(Core core)
    {
      this.core = core;
    }

    // 上传文件
    public async Task<UploadFileResponse> UploadFileAsync(string cloudPath, string filePath)
    {
      if (string.IsNullOrEmpty(cloudPath))
      {
        throw new CloudBaseException(CloudBaseExceptionCode.EMPTY_PARAM, "上传的云端文件路径不能为空");
      }

      if (string.IsNullOrEmpty(filePath))
      {
        throw new CloudBaseException(CloudBaseExceptionCode.EMPTY_PARAM, "上传的本地文件路径不能为空");
      }

      GetUploadUrlResponse metadataRes = await this.GetFileUploadUrlAsync(cloudPath);
      if (!string.IsNullOrEmpty(metadataRes.Code))
      {
        JObject value = new JObject();
        value["code"] = metadataRes.Code;
        value["message"] = metadataRes.Message;
        return new UploadFileResponse(value);
      }

      Dictionary<string, string> param = new Dictionary<string, string>();
      param.Add("key", cloudPath);
      param.Add("signature", metadataRes.File.Authorization);
      param.Add("x-cos-meta-fileid", metadataRes.File.CosFileId);
      param.Add("success_action_status", "201");
      param.Add("x-cos-security-token", metadataRes.File.Token);

      var res = await this.core.Request.UploadAsync(metadataRes.File.Url, filePath, param);
      if (res.IsSuccessStatusCode)
      {
        var value = new { requestId = metadataRes.RequestId, fileId = metadataRes.File.FileId };
        return new UploadFileResponse(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value)) as JObject);
      }
      else
      {
        var resStr = await res.Content.ReadAsStringAsync();
        JObject value = new JObject();
        value["code"] = CloudBaseExceptionCode.STORAGE_REQUEST_FAIL;
        value["message"] = resStr;
        return new UploadFileResponse(value);
      }
    }

    // 下载文件
    async Task DownloadFileAsync(string cloudPath, string filePath)
    {
      if (string.IsNullOrEmpty(cloudPath))
      {
        throw new CloudBaseException(CloudBaseExceptionCode.EMPTY_PARAM, "下载的云端文件路径不能为空");
      }

      if (string.IsNullOrEmpty(filePath))
      {
        throw new CloudBaseException(CloudBaseExceptionCode.EMPTY_PARAM, "下载的本地文件路径不能为空");
      }

      GetDownloadUrlResponse metadataRes = await this.GetFileDownloadUrlAsync(new List<string> { cloudPath });
      if (!string.IsNullOrEmpty(metadataRes.Code))
      {
        throw new CloudBaseException(metadataRes.Code, metadataRes.Message);
      }

      await this.core.Request.DownloadAsync(metadataRes.FileList[0].DownloadUrl, filePath);
    }

    // 删除文件
    public async Task<DeleteFilesResponse> DeleteFilesAsync(List<string> fileIdList)
    {
      if (fileIdList == null || fileIdList.Count <= 0)
      {
        throw new CloudBaseException(CloudBaseExceptionCode.EMPTY_PARAM, "删除文件列表不能为空");
      }

      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
      param.Add("fileid_list", fileIdList);

      DeleteFilesResponse res = await this.core.Request.PostAsync<DeleteFilesResponse>("storage.batchDeleteFile", param);

      return res;
    }

    public async Task<GetDownloadUrlResponse> GetFileDownloadUrlAsync(List<string> fileIdList)
    {
      if (fileIdList == null || fileIdList.Count <= 0)
      {
        throw new CloudBaseException(CloudBaseExceptionCode.EMPTY_PARAM, "下载文件列表不能为空");
      }

      List<Dictionary<string, dynamic>> files = new List<Dictionary<string, dynamic>>();
      for (int i = 0; i < fileIdList.Count; i++)
      {
        Dictionary<string, dynamic> file = new Dictionary<string, dynamic>();
        file.Add("fileid", fileIdList[i]);
        file.Add("max_age", 1800);
        files.Add(file);
      }

      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
      param.Add("file_list", files);
      GetDownloadUrlResponse res = await this.core.Request.PostAsync<GetDownloadUrlResponse>("storage.batchGetDownloadUrl", param);

      return res;
    }

    async Task<GetUploadUrlResponse> GetFileUploadUrlAsync(String cloudPath)
    {
      if (string.IsNullOrEmpty(cloudPath))
      {
        throw new CloudBaseException(CloudBaseExceptionCode.EMPTY_PARAM, "上传的云端文件路径不能为空");
      }

      Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
      param.Add("path", cloudPath);

      GetUploadUrlResponse res = await this.core.Request.PostAsync<GetUploadUrlResponse>("storage.getUploadMetadata", param);

      return res;
    }
  }
}