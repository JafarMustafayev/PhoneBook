namespace PhoneBook.Service
{
    public class FileService
    {
        public static async Task<string> SaveFile(string path, IFormFile file)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileNewName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            await CopyFileAsync(Path.Combine(path, fileNewName), file);

            return fileNewName;
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private static async Task CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}