using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;

namespace BatteryChecker.ViewModel
{
    class DefaultDialogs
    {
        public enum TargetFileType
        {
            PDF = 0,
            DOC_DOCX = 1
        }

        public string FilePath { get; set; }

        public bool OpenFileDialog(TargetFileType targetType)
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

        public bool SaveFileDialog(TargetFileType targetType)
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

        public void ShowMessage(string msg)
        {
            MessageBox.Show(msg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

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
