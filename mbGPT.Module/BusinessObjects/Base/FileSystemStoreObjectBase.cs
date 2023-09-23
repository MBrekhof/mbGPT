using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace mbGPT.Module.BusinessObjects.Base
{
    /// <summary>
    /// This class enables you to store uploaded files in a centralized file system location instead of the database. You can configure the file system store location via the static FileSystemDataModule.FileSystemStoreLocation property.
    /// </summary>
    [DefaultProperty(nameof(FileName))]
    public class FileSystemStoreObjectBase : BaseObject, IFileData, IEmptyCheckable
    {
        private Stream tempSourceStream;
        private string tempFileName = string.Empty;
        private static object syncRoot = new object();
        public string RealFileName
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName) && ID != Guid.Empty)
                    return Path.Combine(FileSystemData.FileSystemStoreLocation, string.Format("{0}-{1}", ID, FileName));
                return null;
            }
        }
        protected virtual void SaveFileToStore()
        {
            if (!string.IsNullOrEmpty(RealFileName) && TempSourceStream != null)
            {
                try
                {
                    using (Stream destination = File.Create(RealFileName))
                    { //T582918
                        FileSystemData.CopyStream(TempSourceStream, destination);
                        Size = (int)destination.Length;
                    }
                }
                catch (DirectoryNotFoundException exc)
                {
                    throw new UserFriendlyException(exc);
                }
            }
        }
        private void RemoveOldFileFromStore()
        {
            //Dennis: We need to remove the old file from the store when saving the current object.
            if (!string.IsNullOrEmpty(tempFileName) && tempFileName != RealFileName)
            {//B222892
                try
                {
                    File.Delete(tempFileName);
                    tempFileName = string.Empty;
                }
                catch (DirectoryNotFoundException exc)
                {
                    throw new UserFriendlyException(exc);
                }
            }
        }
        public override void OnSaving()
        {
            base.OnSaving();
            if (!ObjectSpace.IsObjectToDelete(this))
            {
                Guard.ArgumentNotNullOrEmpty(FileSystemData.FileSystemStoreLocation, "FileSystemStoreLocation");
                lock (syncRoot)
                {
                    if (!Directory.Exists(FileSystemData.FileSystemStoreLocation))
                        Directory.CreateDirectory(FileSystemData.FileSystemStoreLocation);
                }
                SaveFileToStore();
                RemoveOldFileFromStore();
            }
            else
            {
                Clear();
            }
        }
        #region IFileData Members
        public void Clear()
        {
            //Dennis: When clearing the file name property we need to save the name of the old file to remove it from the store in the future. You can also setup a separate service for that.
            if (string.IsNullOrEmpty(tempFileName))
                tempFileName = RealFileName;
            FileName = string.Empty;
            Size = 0;
        }
        [FieldSize(260)]
        public virtual string FileName { get; set; }

        [Browsable(false)]
        [NotMapped]
        public Stream TempSourceStream
        {
            get { return tempSourceStream; }
            set
            {
                //Michael: The original Stream might be closed after a while (on the web too - T160753)
                if (value == null)
                {
                    tempSourceStream = null;
                }
                else
                {
                    if (value.Length > int.MaxValue) throw new UserFriendlyException("File is too long");
                    tempSourceStream = new MemoryStream((int)value.Length);
                    FileSystemData.CopyStream(value, tempSourceStream);
                    tempSourceStream.Position = 0;
                }
            }
        }
        //Dennis: Fires when uploading a file.
        void IFileData.LoadFromStream(string fileName, Stream source)
        {
            //Dennis: When assigning a new file we need to save the name of the old file to remove it from the store in the future.
            if (fileName != FileName)
            {// updated, old code was: if (string.IsNullOrEmpty(tempFileName))
                tempFileName = RealFileName;
            }
            FileName = fileName;
            TempSourceStream = source;
            Size = (int)TempSourceStream.Length;
        }
        //Dennis: Fires when saving or opening a file.
        void IFileData.SaveToStream(Stream destination)
        {
            try
            {
                if (!string.IsNullOrEmpty(RealFileName))
                {
                    if (destination == null)
                        FileSystemData.OpenFileWithDefaultProgram(RealFileName);
                    else
                        FileSystemData.CopyFileToStream(RealFileName, destination);
                }
                else if (TempSourceStream != null)
                    FileSystemData.CopyStream(TempSourceStream, destination);
            }
            catch (DirectoryNotFoundException exc)
            {
                throw new UserFriendlyException(exc);
            }
            catch (FileNotFoundException exc)
            {
                throw new UserFriendlyException(exc);
            }
        }

        public virtual int Size { get; set; }
        #endregion
        #region IEmptyCheckable Members
        public bool IsEmpty
        {
            //T153149
            get { return FileDataHelper.IsFileDataEmpty(this) || !(TempSourceStream != null || File.Exists(RealFileName)); }
        }
        #endregion
    }
}