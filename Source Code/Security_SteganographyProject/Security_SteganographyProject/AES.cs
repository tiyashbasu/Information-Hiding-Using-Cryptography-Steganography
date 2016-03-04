using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Security_SteganographyProject
{
    public class AES : IDisposable
    {
        private AesManaged aes;

        public byte[] Key
        {
            get
            {
                return aes.Key;
            }
        }

        public byte[] InitVector
        {
            get
            {
                return aes.IV;
            }
        }

        public byte[] Encrypt(byte[] msg)
        {
            try
            {
                aes = new AesManaged();
                aes.GenerateKey();
                aes.GenerateIV();
                ICryptoTransform encrypt = aes.CreateEncryptor();
                return encrypt.TransformFinalBlock(msg, 0, msg.Length);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "AES Encryption Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public byte[] Decrypt(byte[] cipherMsg, byte[] key, byte[] initVector)
        {
            try
            {
                aes = new AesManaged();
                ICryptoTransform decrypt = aes.CreateDecryptor(key, initVector);
                return decrypt.TransformFinalBlock(cipherMsg, 0, cipherMsg.Length);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "AES Decryption Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (aes != null)
                {
                    aes.Clear();
                    ((IDisposable)aes).Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}