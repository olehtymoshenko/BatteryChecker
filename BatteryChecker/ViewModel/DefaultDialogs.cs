using System;
using Microsoft.Win32;
using System.Windows;

/// <summary>
/// Namespace for viewmodel component of application
/// </summary>
namespace BatteryChecker.ViewModel
{
    /// <summary>
    /// Class for using default windows dialogs
    /// </summary>
    public class DefaultDialogs
    {
        /// <summary>
        /// Represent file type (extension)
        /// </summary>
        public enum TargetFileType
        {
            PDF = 0,
            DOC_DOCX = 1
        }

        /// <summary>
        /// Filepath which using in OpenFileDialog or SaveFileDialog
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Show OpenFIleDialog
        /// </summary>
        /// <param name="targetType">required files type</param>
        /// <returns>true, if dialog closed with true (user press on OK button)</returns>
        public bool OpenFileDialog(TargetFileType targetType)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                SetUpDialog(targetType, openFileDialog);

                if (openFileDialog.ShowDialog() == true)
                {
                    FilePath = openFileDialog.FileName;
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                throw new Exception("Не удалось запустить диалог для открытия файла.\n Системное описание ошибки" + e.Message);
            }
        }

        /// <summary>
        /// Show SaveFileDialog
        /// </summary>
        /// <param name="targetType">required files type</param>
        /// <returns>true, if dialog closed with true (user press on OK button)</returns>
        public bool SaveFileDialog(TargetFileType targetType)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                SetUpDialog(targetType, saveFileDialog);

                saveFileDialog.CreatePrompt = true;
                saveFileDialog.OverwritePrompt = true;

                if (saveFileDialog.ShowDialog() == true)
                {
                    FilePath = saveFileDialog.FileName;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Не удалось запустить диалог для сохранения файла.\n Системное описание ошибки" + e.Message);
            }
        }

        /// <summary>
        /// Show message using MessageBox
        /// </summary>
        /// <param name="msg">dialog message</param>
        /// <param name="head">caption</param>
        public static void ShowMessage(string msg, string head)
        {
            MessageBox.Show(msg, head, MessageBoxButton.OK);
        }

        /// <summary>
        /// Set up FileDialog for required file type
        /// </summary>
        /// <param name="targetFileType"></param>
        /// <param name="dialog"></param>
        private void SetUpDialog(TargetFileType targetFileType, FileDialog dialog)
        {
            switch (targetFileType)
            {
                case TargetFileType.PDF:
                    {
                        dialog.Filter = @"PDF file(*.pdf)|*.pdf";
                        dialog.DefaultExt = "pdf";
                    } break;
                case TargetFileType.DOC_DOCX:
                    {
                        dialog.Filter = @"DOC file(*.doc)|*.doc|DOCX file(*.docx)|*.docx";
                        dialog.DefaultExt = "doc";
                    } break;
            }
            dialog.AddExtension = true;
            dialog.InitialDirectory = Environment.CurrentDirectory;
        }
    }
}
