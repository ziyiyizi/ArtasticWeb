using Aliyun.OSS;
using System;
using System.IO;

namespace OSA
{
    public class ObjectStoreAccess
    {
        private static readonly String endpoint = OssConfig.endpoint;

        private static readonly String accessKeyId = OssConfig.accessKeyId;

        private static readonly String accessKeySecret = OssConfig.accessKeySecret;

        private static readonly String bucketName = OssConfig.bucketName;

        private readonly String baseFileDir = "PicArtastic/";

        private static readonly CannedAccessControlList acl = CannedAccessControlList.PublicRead;

        private String fileDir = "PicArtastic/";

        private static readonly String website = "http://pic-artastic.oss-cn-shanghai.aliyuncs.com/";

        private readonly OssClient ossClient;

        public ObjectStoreAccess()
        {
            ossClient = new OssClient(endpoint, accessKeyId, accessKeySecret);
        }

        public void ChangeDir(string str)
        {
            fileDir = baseFileDir + str;
        }

        public string UploadFile(string url)
        {
            string ret = null;
            try
            {
                var fs = File.Open(url, FileMode.Open);
                if (fs.Length > 5 * 1024 * 1024)
                {
                    throw new Exception("the max size of the file is 5MB...");
                }
                string name = Path.GetFileName(url);
                ret = UploadFile2(fs, name);
            }
            catch (Exception e)
            {

            }
            return ret;
        }

        public string UploadFile(Stream fs, string url)
        {
            if (fs.Length > 5 * 1024 * 1024)
            {
                throw new Exception("the max size of the file is 5MB...");
            }
            string name = Guid.NewGuid().ToString("N") + Path.GetExtension(url).ToLower();
            UploadFile2(fs, name);
            return name;
        }

        /**
         * 上传到OSS服务器  如果同名文件会覆盖服务器上的
         *
         * @param instream 文件流
         * @param fileName 文件名称 包括后缀名
         * @return 出错返回"" ,唯一MD5数字签名
         */
        public string UploadFile2(Stream instream, string fileName)
        {
            string ret = null;
            try
            {
                //创建上传Object的Metadata
                ObjectMetadata objectMetadata = new ObjectMetadata
                {
                    ContentLength = instream.Length,
                    CacheControl = "no-cache",
                    ContentType = GetcontentType(fileName.Substring(fileName.LastIndexOf("."))),
                    ContentDisposition = "inline;filename=" + fileName
                };
                objectMetadata.AddHeader("Pragma", "no-cache");
                //上传文件
                PutObjectResult putResult = ossClient.PutObject(bucketName, fileDir + fileName, instream, objectMetadata);
                ossClient.SetObjectAcl(bucketName, fileDir + fileName, acl);
                ret = putResult.ETag;
            }
            catch (IOException e)
            {
            }
            finally
            {
                try
                {
                    if (instream != null)
                    {
                        instream.Close();
                    }
                }
                catch (IOException e)
                {
                }
            }
            return ret;
        }

        /**
         * Description: 判断OSS服务文件上传时文件的contentType
         *
         * @param FilenameExtension 文件后缀
         * @return String
         */
        public static String GetcontentType(String FilenameExtension)
        {
            if (FilenameExtension.Equals(".bmp", StringComparison.OrdinalIgnoreCase))
            {
                return "image/bmp";
            }
            if (FilenameExtension.Equals(".gif", StringComparison.OrdinalIgnoreCase))
            {
                return "image/gif";
            }
            if (FilenameExtension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    FilenameExtension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    FilenameExtension.Equals(".png", StringComparison.OrdinalIgnoreCase))
            {
                return "image/jpeg";
            }
            if (FilenameExtension.Equals(".html", StringComparison.OrdinalIgnoreCase))
            {
                return "text/html";
            }
            if (FilenameExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
            {
                return "text/plain";
            }
            if (FilenameExtension.Equals(".vsd", StringComparison.OrdinalIgnoreCase))
            {
                return "application/vnd.visio";
            }
            if (FilenameExtension.Equals(".pptx", StringComparison.OrdinalIgnoreCase) ||
                    FilenameExtension.Equals(".ppt", StringComparison.OrdinalIgnoreCase))
            {
                return "application/vnd.ms-powerpoint";
            }
            if (FilenameExtension.Equals(".docx", StringComparison.OrdinalIgnoreCase) ||
                    FilenameExtension.Equals(".doc", StringComparison.OrdinalIgnoreCase))
            {
                return "application/msword";
            }
            if (FilenameExtension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
            {
                return "text/xml";
            }
            return "image/jpeg";
        }


        /**
         * 获得图片路径
         *
         * @param fileUrl
         * @return
         */
        public String GetImgUrl(String fileUrl, bool secret = false)
        {
            if (!string.IsNullOrEmpty(fileUrl))
            {
                if (secret)
                {
                    String[] strings = fileUrl.Split("/");
                    return GetUrl(fileDir + strings[strings.Length - 1]);
                }
                else
                {
                    return website + fileDir + fileUrl;
                }
                
            }
            return null;
        }

        public String GetUrl(String key)
        {
            // 生成URL
            Uri url = null;
            try
            {
                
                url = ossClient.GeneratePresignedUri(bucketName, key);
            }
            catch (Exception e)
            {
            }

            return url?.ToString();
        }
    }
}
