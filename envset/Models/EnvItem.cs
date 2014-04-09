using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace envset.Models
{
    public class EnvItem
    {
        public delegate void OnPropertyChanged(EnvItem oldItem, string newKey, string newValue);
        public event OnPropertyChanged PropertyChanged;

        public EnvItem Clone()
        {
            return new EnvItem() { Key = _key, Value = _value };
        }


        public bool IsChecked { get; set; }
        public bool IsUserEnv { get; set; }
        public bool IsUpdated { get; set; }

        public string Key
        {
            get { return _key; }
            set
            {
                if (value == _key) return;
                if (PropertyChanged != null) PropertyChanged(this, value, null);
                _key = value;
                IsUpdated = true;
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                if (value == _value) return;
                if (PropertyChanged != null) PropertyChanged(this, null, value);
                _value = value;
                IsUpdated = true;
            }
        }


        public void WriteIfChanged()
        {
            if (IsUpdated && Key.Length > 0)
            {
                if (Value.Length > 0)
                {
                    WindowsShell.SetEnvironmentVariableSync(Key, Value, IsUserEnv);
                }
                else
                {
                    WindowsShell.DeleteEnvironmentVariableSync(Key, IsUserEnv);
                }
                IsUpdated = false;
            }
        }


        private string _key;
        private string _value;
    }


    public class EnvSet
    {
        public string Name { get; set; }
        public BindingList<EnvItem> ItemList { get; set; }

        public EnvItem CurrentItem { get; set; }
    }
}
