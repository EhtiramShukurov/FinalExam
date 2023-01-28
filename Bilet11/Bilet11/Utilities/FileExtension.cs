namespace Bilet11.Utilities
{
    public static class FileExtension
    {
        public static bool CheckSize(this IFormFile? file, int kb)
            => file.Length <= kb * 1024;
        public static bool CheckType(this IFormFile? file, string type)
            => file.ContentType.Contains(type);
        public static string ChangeFileName(string oldName)
        {
            string extension  = oldName.Substring(oldName.LastIndexOf('.'));
            if (oldName.Length <32)
            {
                oldName = oldName.Substring(0, oldName.LastIndexOf("."));
            }
            else
            {
                oldName = oldName.Substring(0, 31);
            }
            return Guid.NewGuid() +oldName + extension;
        }
        public static string SaveFile(this IFormFile? file,string root,string folder)
        {
            string fileName = ChangeFileName(file.FileName);
            using (FileStream fs = new FileStream(Path.Combine(root, folder, fileName), FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return fileName;
        }
        public static void DeleteFile(this string fileName,string root,string folder)
        {
            string path = Path.Combine(root,folder,fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static string CheckValidate(this IFormFile? file,string type,int kb)
        {
            string result = "";
            if(!file.CheckType(type))
            {
                result += $" Type of file {file.FileName} should be an image!";
            }
            if (!file.CheckSize(kb))
            {
                result += $"Size of file {file.FileName} shouldn't be higher than {kb} kb!";
            }
            return result;
        }
    }
}
