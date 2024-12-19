using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Core.Encryption
{
    public class GenericDataHandler<T>
    {
        public void SaveObjectData(string filePath, T obj)
        {
            try
            {
                // Serialize the object to XML
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using (StringWriter stringWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(stringWriter, obj);
                    string xmlContenr = stringWriter.ToString();

                    // Encrypt the XML Content
                    string encryptedData = Encrypt(xmlContenr);

                    // Save the encrypted in the file
                    File.WriteAllText(filePath, encryptedData);
                }
            }catch (Exception ex)
            {
                Console.WriteLine($"Error saving encrypted dataa: {ex.Message}");
            }
            
        }

        public T ReadObjectData(string filePath)
        {
            try
            {
                string encryptedData = File.ReadAllText(filePath);

                // Decrypt the string
                string decryptedData = Decrypt(encryptedData);

                // Deserialize the XML to an object
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using (StringReader stringReader = new StringReader(decryptedData))
                {
                    return (T)xmlSerializer.Deserialize(stringReader);
                }
            }catch (Exception ex)
            {
                Console.WriteLine($"Error reading data: {ex.Message}");
                return default(T);
            }
            
        }

        private string Encrypt(string decryptedData)
        {
            //that string used in hashing by MD5 to use as a KEY for triple DES 
            string hash = "myKeyToUse@2021$";
            //Triple DES works with data in the form of bytes, not directly with text
            byte[] data = UTF8Encoding.UTF8.GetBytes(decryptedData);
            var encryptedText = string.Empty;

            try
            {
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    using (TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider())
                    {
                        tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                        tripDES.Mode = CipherMode.ECB;

                        ICryptoTransform transform = tripDES.CreateEncryptor();
                        byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                        encryptedText = Convert.ToBase64String(result);
                    }
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error happends during Encrypting : {ex.Message}");
            }
            
            return encryptedText.ToString();
        }
        private string Decrypt(string encryptedData)
        {
            string hash = "myKeyToUse@2021$";
            byte[] data = Convert.FromBase64String(encryptedData);
            var decryptedText = string.Empty;
            try
            {
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    using (TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider())
                    {
                        tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                        tripDES.Mode = CipherMode.ECB;

                        ICryptoTransform transform = tripDES.CreateDecryptor();
                        byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                        decryptedText = UTF8Encoding.UTF8.GetString(result);
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine($"Error happends Decrypting : {ex.Message}");
            }
            
            return decryptedText.ToString();

            
        }

       
    }
}
