using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Encryption
{
    public class DatabaseHandler
    {
        private const string FileExtension = ".STD";
        private readonly GenericDataHandler<DatabaseConnectionParameters> _dataHandler
            = new GenericDataHandler<DatabaseConnectionParameters>();

        public void SaveConnectionParameters(string filePath, DatabaseConnectionParameters parameters)
        {
            string fullPath = filePath + FileExtension;
            _dataHandler.SaveObjectData(fullPath, parameters);
        }

        public DatabaseConnectionParameters LoadConnectionParameters(string filePath)
        {
            string fullPath = filePath + FileExtension;
            return _dataHandler.ReadObjectData(fullPath);
        }
    }
}
