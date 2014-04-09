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

namespace envset.ViewModels
{
    public class EditEnvWindowViewModel : ViewModel
    {
        public class ValueItem
        {
            public string Value { set; get; }
        }

        // ウィンドウが受け取る引数扱いなので、ビューのコードビハインドで受け取って設定されている
        public EnvItem TargetEnvItem { get; set; }


        #region Key変更通知プロパティ
        private string _Key;

        public string Key
        {
            get
            { return _Key; }
            set
            { 
                if (_Key == value)
                    return;
                _Key = value;
                RaisePropertyChanged("Key");
            }
        }
        #endregion

        #region ValueList変更通知プロパティ
        private List<ValueItem> _ValueList;

        public List<ValueItem> ValueList
        {
            get
            { return _ValueList; }
            set
            { 
                if (_ValueList == value)
                    return;
                _ValueList = value;
                RaisePropertyChanged("ValueList");
            }
        }
        #endregion


        public void Initialize()
        {
            Key = TargetEnvItem.Key;
            UpdateValueList();
        }


        public void UpdateValueList()
        {
            ValueList = TargetEnvItem.Value.Split(';')
                .Select(item => new ValueItem() { Value = item })
                .ToList();
        }


        public void EditComplete()
        {
            var newValues = ValueList.Where(item => item.Value.Trim().Length > 0).ToArray();
            TargetEnvItem.Value = (newValues.Length > 1) ? string.Join(";", newValues.Select(item => item.Value)) : newValues[0].Value;
            TargetEnvItem.WriteIfChanged();

            UpdateValueList();
        }
    }
}
