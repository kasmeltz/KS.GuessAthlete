using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace KS.GuessAthlete.WPF.Utility
{
    public static class UIExtensions
    {
        /// <summary>
        /// Gets the string value of the resource with the specified key 
        /// or null if the resource can't be found.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetResourceText(this FrameworkElement element, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }

            string text = key;
            object resourceObject = element.TryFindResource(key);

            if (resourceObject != null)
            {
                text = resourceObject.ToString();
            }

            return text;
        }

        public static void ShowMessageDialogue(this FrameworkElement element,
            Exception ex, string title)
        {
            element.ShowWaitCursor(false);

            Application.Current.Dispatcher.Invoke(() =>
            {
                string baseMessage = ex.Message;
                AggregateException aggregateEx = (AggregateException)ex;
                if (aggregateEx != null)
                {
                    if (aggregateEx.InnerExceptions.Count > 0)
                    {
                        baseMessage = aggregateEx.InnerExceptions[0].Message;
                    }
                }

                string message = element.GetResourceText(baseMessage);
                if (string.IsNullOrEmpty(message))
                {
                    message = baseMessage;
                }

                MessageBox.Show(Application.Current.MainWindow,
                    message,
                    element.GetResourceText(title),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            });
        }

        public static void ShowMessageDialogue(this FrameworkElement element,
            string title, string message, MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Warning,
            Action<MessageBoxResult> onClose = null)
        {
            element.ShowWaitCursor(false);

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow,
                    element.GetResourceText(message),
                    element.GetResourceText(title),
                    buttons, image);

                if (onClose != null)
                {
                    onClose(result);
                }
            });
        }

        public static void DoOnConfirm(this FrameworkElement element,
            string title, string message, Action onOK, Action onCancel = null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow,
                    element.GetResourceText(message),
                    element.GetResourceText(title),
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.OK)
                {
                    if (onOK != null)
                    {
                        onOK();
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    if (onCancel != null)
                    {
                        onCancel();
                    }
                }
            });
        }

        public static string OpenFileDialog(this FrameworkElement element, string title,
            bool isFolder = false, bool ensureFileExists = true)
        {
            element.ShowWaitCursor(false);

            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
            dlg.Title = title;
            dlg.IsFolderPicker = isFolder;
            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.EnsureFileExists = ensureFileExists;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dlg.FileName;
            }

            return null;
        }
        
        public static void ShowWaitCursor(this FrameworkElement element, bool isWaiting)
        {
            if (isWaiting)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
            }
        }

        public static void DoBackGroundWork(this FrameworkElement element, Action a)
        {
            Thread workThread = new Thread(new ThreadStart(a));
            workThread.IsBackground = true;
            workThread.Start();
        }

        public static void DoSTAWork(this FrameworkElement element, Action a)
        {
            Thread t = new Thread(new ThreadStart(a));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        public static BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        /// <summary>
        /// Compares the values of the public properties of two items.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public static bool CompareItems<T>(this FrameworkElement element, T item1, T item2, params string[] toSkip)
        {
            Type type = typeof(T);
            PropertyInfo[] props = type.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (toSkip.Contains(prop.Name))
                {
                    continue;
                }

                object v1 = prop.GetValue(item1, null);
                object v2 = prop.GetValue(item2, null);

                if (v1 != v2)
                {
                    return false;
                }
            }

            return true;
        }
    }   
}


