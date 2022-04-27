using Microsoft.AspNetCore.Http;
using System.IO; 
using System; 

namespace E_Commerce.MvcWebUI.Managing
{
    public class ImageClass
    { 
        public string UploadImage(IFormFile file, string uploadsFolder)
        {

            var uniqueFileName = NewImageName(Path.GetExtension(file.FileName));
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            } 

            string[] fullpath = filePath.Split("\\");   
            return fullpath[fullpath.Length - 3] + "/" + fullpath[fullpath.Length-2]+"/"+fullpath[fullpath.Length-1];
        }
        
        public string UpdateImage(string oldUrl, IFormFile file, string url)
        {
            //Bir önceki fotoğrafı sil
            DeleteImage(oldUrl);

            //Yeni fotoğrafı yükle
           return UploadImage(file, url); 
        }
        public bool DeleteImage(string url)
        {
            var filePath = url.Replace("/","\\");
            FileInfo fileDelete = new FileInfo(filePath);
            if (fileDelete.Exists) 
            {
                fileDelete.Delete();
                return true;
            }
            return false;   
        } 
        private string NewImageName(string fileName)
        {
            return Guid.NewGuid().ToString().Replace("-", "") + fileName; 
        }
    }
}
