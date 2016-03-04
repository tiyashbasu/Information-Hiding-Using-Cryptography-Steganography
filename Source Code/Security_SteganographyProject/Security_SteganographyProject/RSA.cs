using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Security_SteganographyProject
{
    public class RSA : IDisposable
    {
        private RSACryptoServiceProvider rsa;
        private FileStream fs;
        private BufferedStream bs;

        public RSA()
        {
            RSACryptoServiceProvider.UseMachineKeyStore = true;
        }

        public void SaveNewKeys(string pathName)
        {
            try
            {
                fs = new FileStream(Path.Combine(pathName, "PrivateSet.prvrsa"), FileMode.Create, FileAccess.Write, FileShare.None);
                bs = new BufferedStream(fs);
                rsa = new RSACryptoServiceProvider();

                string xmlStr = rsa.ToXmlString(true);
                bs.Write(Converter.GetBytes(xmlStr), 0, xmlStr.Length);
                bs.Close();
                fs.Close();
                fs = new FileStream(Path.Combine(pathName, "PublicSet.pubrsa"), FileMode.Create, FileAccess.Write, FileShare.None);
                bs = new BufferedStream(fs);
                xmlStr = rsa.ToXmlString(false);
                bs.Write(Converter.GetBytes(xmlStr), 0, xmlStr.Length);

                MessageBox.Show("New Keys Generated Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (bs != null)
                    bs.Dispose();
                if (fs != null)
                    fs.Dispose();
            }
        }

        public void SetKey(string fileName)
        {
            try
            {
                rsa = new RSACryptoServiceProvider();
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                bs = new BufferedStream(fs);
                byte[] inxml = new byte[bs.Length];
                bs.Read(inxml, 0, (int)bs.Length);
                rsa.FromXmlString(Converter.GetString(inxml));
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Can't Generate Keys", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (bs != null)
                    bs.Dispose();
                if (fs != null)
                    fs.Dispose();
            }
        }

        public byte[] Decrypt(byte[] cipherText)
        {
            try
            {
                return rsa.Decrypt(cipherText, true);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "RSA Encryption Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public byte[] Encrypt(byte[] plainText)
        {
            try
            {
                return rsa.Encrypt(plainText, true);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "RSA Encryption Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (rsa != null)
                {
                    rsa.Clear();
                    ((IDisposable)rsa).Dispose();
                }
                if (bs != null)
                    bs.Dispose();
                if (fs != null)
                    fs.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}