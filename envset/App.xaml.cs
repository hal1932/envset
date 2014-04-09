using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using Livet;

namespace envset
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DispatcherHelper.UIDispatcher = Dispatcher;
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            if (!StartCommandLine(e.Args))
            {
                new Views.MainWindow().Show();
            }
        }

        //集約エラーハンドラ
        //private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        //{
        //    //TODO:ロギング処理など
        //    MessageBox.Show(
        //        "不明なエラーが発生しました。アプリケーションを終了します。",
        //        "エラー",
        //        MessageBoxButton.OK,
        //        MessageBoxImage.Error);
        //
        //    Environment.Exit(1);
        //}

        private bool StartCommandLine(string[] args)
        {
            var config = "";
            for (var i = 0; i < args.Length; ++i)
            {
                switch (args[i].Trim())
                {
                    case "--config":
                        config = args[++i].Trim();
                        break;

                    default:
                        break;
                }
            }

            if (config.Length > 0)
            {
                return true;
            }

            return false;
        }
    }
}
