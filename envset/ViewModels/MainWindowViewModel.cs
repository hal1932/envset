using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using envset.Models;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;

namespace envset.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region ConfigList変更通知プロパティ
        private List<string> _ConfigList;

        public List<string> ConfigList
        {
            get
            { return _ConfigList; }
            set
            { 
                if (_ConfigList == value)
                    return;
                _ConfigList = value;
                RaisePropertyChanged("ConfigList");
            }
        }
        #endregion

        #region CurrentConfigIndex変更通知プロパティ
        private int _CurrentConfigIndex;

        public int CurrentConfigIndex
        {
            get
            { return _CurrentConfigIndex; }
            set
            { 
                //if (_CurrentConfigIndex == value)
                //    return;
                _CurrentConfigIndex = value;
                if (0 <= value && value < _envSetDic.Count)
                {
                    EnvSet = new BindingList<EnvItem>(_envSetDic.Values.Skip(value).First());
                }
                RaisePropertyChanged("CurrentConfigIndex");
            }
        }
        #endregion

        #region EnvSet変更通知プロパティ
        private BindingList<EnvItem> _EnvSet;

        public BindingList<EnvItem> EnvSet
        {
            get
            { return _EnvSet; }
            set
            { 
                if (_EnvSet == value)
                    return;
                _EnvSet = value;
                RaisePropertyChanged("EnvSet");
            }
        }
        #endregion

        #region ModeList変更通知プロパティ
        private string[] _ModeList;

        public string[] ModeList
        {
            get
            { return _ModeList; }
            set
            { 
                if (_ModeList == value)
                    return;
                _ModeList = value;
                RaisePropertyChanged("ModeList");
            }
        }
        #endregion

        #region CurrentModeIndex変更通知プロパティ
        private int _CurrentModeIndex;

        public int CurrentModeIndex
        {
            get
            { return _CurrentModeIndex; }
            set
            { 
                if (_CurrentModeIndex == value)
                    return;
                _CurrentModeIndex = value;
                RaisePropertyChanged("CurrentModeIndex");
            }
        }
        #endregion



        private Dictionary<string, List<EnvItem>> _envSetDic = new Dictionary<string, List<EnvItem>>();
        private List<EnvItem> _deletedEnvList = new List<EnvItem>();


        public void Initialize()
        {
            EnvSet = new BindingList<EnvItem>();
            ModeList = OperatingMode.ModeList;
        }

        public void UpdateList()
        {
        }

        #region メニュー
        #region ファイル
        public void ImportList(string filename = "")
        {
            if (filename == "")
            {
                var form = new OpenFileDialog()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                };
                if(form.ShowDialog() == true)
                {
                    filename = form.FileName;
                }
            }

            _envSetDic = ConfigFile.LoadDic(filename);
            ConfigList = _envSetDic.Keys.ToList();
            CurrentConfigIndex = 0;
        }

        public void OpenList(string filename = "")
        {
            if (filename == "")
            {
                var form = new OpenFileDialog()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                };
                if (form.ShowDialog() == true)
                {
                    filename = form.FileName;
                }
            }

            WindowsShell.StartProcess(filename);
        }

        public void Import(string filename = "")
        {
            if (filename == "")
            {
                var form = new OpenFileDialog()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                };
                if (form.ShowDialog() == true)
                {
                    filename = form.FileName;
                }
            }

            var envSet = ConfigFile.Load(filename);
            EnvSet = new BindingList<EnvItem>(envSet);

            _envSetDic.Clear();
            CurrentConfigIndex = -1;
        }

        public void Export(string filename = "")
        {
            if (filename == "")
            { 
                var form = new OpenFileDialog()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    CheckFileExists = false,
                };
                if (form.ShowDialog() == true)
                {
                    filename = form.FileName;
                }
            }

            ConfigFile.Save(filename, EnvSet);
        }
        #endregion

        #region 表示
        public void OpenRegEdit()
        {
            WindowsShell.StartProcess("regedit.exe");
        }
        #endregion
        #endregion

        #region 環境変数
        public void DeleteCheckedEnv()
        {
            // RemoveAll 使えない。。orz
            for (var i = 0; i < EnvSet.Count; ++i)
            {
                var item = EnvSet[i];
                if (item.IsChecked)
                {
                    _deletedEnvList.Add(item.Clone());
                    EnvSet.RemoveAt(i);
                }
            }
        }

        public void ApplyEnvAdd()
        {
            switch(ModeList[CurrentModeIndex])
            {
                case OperatingMode.OverWrite:
                    EnvSet.Where(item => item.Value.Length > 0)
                        .Select(item => WindowsShell.SetEnvironmentVariableSync(item.Key, item.Value));
                    break;

                case OperatingMode.Diff:
                    EnvSet.Where(item => item.Value.Length > 0)
                        .Select(item => WindowsShell.AddEnvironmentVariableSync(item.Key, item.Value));
                    break;

                default:
                    break;
            }
        }

        public void ApplyEnvSub()
        {
            switch (ModeList[CurrentModeIndex])
            {
                case OperatingMode.OverWrite:
                    EnvSet.Where(item => item.Value.Length > 0)
                        .Select(item => WindowsShell.DeleteEnvironmentVariableSync(item.Key));
                    break;

                case OperatingMode.Diff:
                    EnvSet.Where(item => item.Value.Length > 0)
                        .Select(item => WindowsShell.SubstructEnvironmentVariableSync(item.Key, item.Value));
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
