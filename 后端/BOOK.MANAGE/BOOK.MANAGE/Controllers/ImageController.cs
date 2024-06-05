using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BOOK.MANAGE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;

        public ImageController(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        [HttpPost("imgupload")]
        public ActionResult<object> imgupload(IFormFile file, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    // 生成新的文件名，使用当前时间戳
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";

                    // 获取应用程序的根目录
                    string contentRootPath = webHostEnvironment.ContentRootPath;

                    // 拼接文件保存路径，假设有一个名为temp_img的文件夹在项目根目录
                    string filePath = Path.Combine(contentRootPath, "TempImage", fileName);

                    // 创建目标文件夹
                    string targetDirectory = Path.Combine(contentRootPath, "TempImage");
                    Directory.CreateDirectory(targetDirectory);

                    // 保存文件到指定路径
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // 构建图片的URL地址
                    string imageUrl = $"https://localhost:7065/temp_img/{fileName}";

                    // 返回成功的响应，包含图片的URL
                    return new { code = 20000, success = true, message = "文件上传成功", data = new { imageUrl, URL = fileName } };
                }
                else
                {
                    // 文件为空时的处理
                    return new { code = 400, success = false, message = "文件为空" };
                }
            }
            catch (Exception ex)
            {
                // 异常处理
                return new { code = 400, success = false, message = ex.Message };
            }
        }

        //调用图片：https://localhost:7052/api/Tool/20231226000938.jpg
        [HttpGet("{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            if (imageName == "null") imageName = "noway.gif";
            // 获取应用程序的根目录
            string rootPath = _hostEnvironment.ContentRootPath;

            // 构建相对路径
            string relativePath = Path.Combine("TempImage", imageName);

            // 构建完整的文件路径
            string imagePath = Path.Combine(rootPath, relativePath);

            // 检查文件是否存在
            if (System.IO.File.Exists(imagePath))
            {
                // 读取文件内容
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);

                // 返回文件流
                return File(imageBytes, "image/jpeg"); // 根据实际情况设置正确的 Content-Type
            }
            else
            {
                return NotFound(); // 文件不存在时返回404
            }
        }
    }
}
