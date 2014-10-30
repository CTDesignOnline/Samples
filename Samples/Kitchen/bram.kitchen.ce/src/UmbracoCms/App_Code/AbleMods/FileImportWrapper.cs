namespace AbleMods
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Web;

    /// <summary>
    /// A wrapper for imported files and media
    /// </summary>
    public sealed class FileImportWrapper : HttpPostedFileBase
    {
        /// <summary>
        /// The <see cref="FileInfo"/>
        /// </summary>
        private readonly FileInfo _fileInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileImportWrapper"/> class.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        public FileImportWrapper(string filePath)
        {
            this._fileInfo = new FileInfo(filePath);
        }

        public override int ContentLength
        {
            get
            {
                return (int)this._fileInfo.Length;
            }
        }

        public override string ContentType
        {
            get
            {
                return MimeExtensionHelper.GetMimeType(this._fileInfo.Name);
            }
        }

        public override string FileName
        {
            get
            {
                return this._fileInfo.FullName;
            }
        }

        public override System.IO.Stream InputStream
        {
            get
            {
                return this._fileInfo.OpenRead();
            }
        }

        public static class MimeExtensionHelper
        {
            static object locker = new object();
            static object mimeMapping;
            static MethodInfo getMimeMappingMethodInfo;

            static MimeExtensionHelper()
            {
                Type mimeMappingType = Assembly.GetAssembly(typeof(HttpRuntime)).GetType("System.Web.MimeMapping");
                if (mimeMappingType == null)
                    throw new SystemException("Couldnt find MimeMapping type");
                ConstructorInfo constructorInfo = mimeMappingType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (constructorInfo == null)
                    throw new SystemException("Couldnt find default constructor for MimeMapping");
                mimeMapping = constructorInfo.Invoke(null);
                if (mimeMapping == null)
                    throw new SystemException("Couldnt find MimeMapping");
                getMimeMappingMethodInfo = mimeMappingType.GetMethod("GetMimeMapping", BindingFlags.Static | BindingFlags.NonPublic);
                if (getMimeMappingMethodInfo == null)
                    throw new SystemException("Couldnt find GetMimeMapping method");
                if (getMimeMappingMethodInfo.ReturnType != typeof(string))
                    throw new SystemException("GetMimeMapping method has invalid return type");
                if (getMimeMappingMethodInfo.GetParameters().Length != 1 && getMimeMappingMethodInfo.GetParameters()[0].ParameterType != typeof(string))
                    throw new SystemException("GetMimeMapping method has invalid parameters");
            }

            public static string GetMimeType(string filename)
            {
                lock (locker)
                    return (string)getMimeMappingMethodInfo.Invoke(mimeMapping, new object[] { filename });
            }
        }
    }
}