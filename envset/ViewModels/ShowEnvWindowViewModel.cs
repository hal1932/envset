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
using System.Collections;

namespace envset.ViewModels
{
    public class ShowEnvWindowViewModel : ViewModel
    {
        #region EnvSetList変更通知プロパティ
        private List<EnvSet> _EnvSetList;

        public List<EnvSet> EnvSetList
        {
            get
            { return _EnvSetList; }
            set
            { 
                if (_EnvSetList == value)
                    return;
                _EnvSetList = value;
                RaisePropertyChanged("EnvSetList");
            }
        }
        #endregion

        #region CurrentEnvSetIndex変更通知プロパティ
        private int _CurrentEnvSetIndex;

        public int CurrentEnvSetIndex
        {
            get
            { return _CurrentEnvSetIndex; }
            set
            { 
                if (_CurrentEnvSetIndex == value)
                    return;
                _CurrentEnvSetIndex = value;
                RaisePropertyChanged("CurrentEnvSetIndex");
            }
        }
        #endregion

        #region CurrentItem変更通知プロパティ
        private EnvItem _CurrentItem;

        public EnvItem CurrentItem
        {
            get
            { return _CurrentItem; }
            set
            {
                if (_CurrentItem == value)
                    return;
                _CurrentItem = value;
                RaisePropertyChanged("CurrentItem");
            }
        }
        #endregion


        public void Initialize()
        {
            UpdateList();
        }


        public void UpdateList()
        {
            var userEnv = new List<EnvItem>();
            foreach (DictionaryEntry item in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User))
            {
                userEnv.Add(new EnvItem()
                {
                    Key = item.Key.ToString(),
                    Value = item.Value.ToString(),
                    IsUserEnv = true,
                    IsUpdated = false,
                });
            }

            var systemEnv = new List<EnvItem>();
            foreach (DictionaryEntry item in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine))
            {
                systemEnv.Add(new EnvItem()
                {
                    Key = item.Key.ToString(),
                    Value = item.Value.ToString(),
                    IsUserEnv = false,
                    IsUpdated = false,
                });
            }

            var _current = CurrentEnvSetIndex;
            {
                EnvSetList = new List<EnvSet>()
                {
                    new EnvSet() { Name = "ユーザ", ItemList = new BindingList<EnvItem>(userEnv) },
                    new EnvSet() { Name = "システム", ItemList = new BindingList<EnvItem>(systemEnv) },
                };
                EnvSetList[0].ItemList.AddingNew += (sender, e) => e.NewObject = new EnvItem() { IsUserEnv = true };
                EnvSetList[1].ItemList.AddingNew += (sender, e) => e.NewObject = new EnvItem() { IsUserEnv = false };
            }
            CurrentEnvSetIndex = _current;
        }


        public void EditComplete()
        {
            foreach (var set in EnvSetList)
            {
                foreach (var item in set.ItemList)
                {
                    item.WriteIfChanged();
                }
            }
            UpdateList();
        }
    }
}
