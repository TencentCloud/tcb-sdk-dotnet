using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CloudBase
{
  public class DeleteMetadata
  {
    public readonly string FileId;
    public readonly string Code;

    public DeleteMetadata(JObject data)
    {
      this.FileId = (string) data["fileid"];
      this.Code = (string) data["code"];
    }
  }

  public class DeleteFilesResponse : Response
  {
    public readonly List<DeleteMetadata> FileList = new List<DeleteMetadata>();

    public DeleteFilesResponse(JObject res) : base(res)
    {
      if (string.IsNullOrEmpty(base.Code))
      {
        JArray fileList = res["data"]["delete_list"] as JArray;
        for (int i = 0; i < fileList.Count; i++)
        {
          JObject fileObj = fileList[i] as JObject;
          DeleteMetadata meta = new DeleteMetadata(fileObj);
          this.FileList.Add(meta);
        }
      }
    }
  }

  public class DownloadMetadata
  {
    public readonly string FileId;
    public readonly string DownloadUrl;

    public DownloadMetadata(JObject data)
    {
      this.FileId = (string) data["fileid"];
      this.DownloadUrl = (string) data["download_url"];
    }
  }

  public class GetDownloadUrlResponse : Response
  {
    public readonly List<DownloadMetadata> FileList = new List<DownloadMetadata>();

    public GetDownloadUrlResponse(JObject res) : base(res)
    {
      if (string.IsNullOrEmpty(base.Code))
      {
        JArray fileList = res["data"]["download_list"] as JArray;
        for (int i = 0; i < fileList.Count; i++)
        {
          JObject fileObj = fileList[i] as JObject;
          DownloadMetadata meta = new DownloadMetadata(fileObj);
          this.FileList.Add(meta);
        }
      }
    }
  }

  // public class DownloadFileResponse : Response {
  //   public DownloadFileResponse() : base(res) {

  //   }
  // }

  class UploadMetadata
  {
    public readonly string Url;
    public readonly string Token;
    public readonly string Authorization;
    public readonly string FileId;
    public readonly string CosFileId;

    public UploadMetadata(JObject data)
    {
      this.Url = (string) data["url"];
      this.Token = (string) data["token"];
      this.Authorization = (string) data["authorization"];
      this.FileId = (string) data["fileId"];
      this.CosFileId = (string) data["cosFileId"];
    }
  }

  class GetUploadUrlResponse : Response
  {
    public readonly UploadMetadata File;

    public GetUploadUrlResponse(JObject res) : base(res)
    {
      if (string.IsNullOrEmpty(base.Code))
      {
        JObject fileObj = res["data"] as JObject;
        this.File = new UploadMetadata(fileObj);
      }
    }
  }

  public class UploadFileResponse : Response
  {
    public readonly string FileID;

    public UploadFileResponse(JObject res) : base(res)
    {
      if (string.IsNullOrEmpty(base.Code))
      {
        this.FileID = (string) res["fileId"];
      }
    }
  }

}