using Brook.ZhiHuRiBao.Models;
using Brook.ZhiHuRiBao.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Brook.ZhiHuRiBao.Utils
{
    public class StorageUtil
    {
        private const string StorageInfoKey = "StorageInfo";

        private const string TileBeInitedKey = "TileBeInited";

        static ApplicationDataContainer _localSetting = ApplicationData.Current.LocalSettings;

        public static StorageInfo StorageInfo;

        static StorageUtil()
        {
            if (!TryGetJsonObj(StorageInfoKey, out StorageInfo))
            {
                StorageInfo = new StorageInfo();
            }
        }

        public static void UpdateStorageInfo()
        {
            if (StorageInfo == null)
                return;

            AddObject(StorageInfoKey, StorageInfo);
        }

        public static void SetCommentPanelStatus(bool isOpen)
        {
            StorageInfo.IsCommentPanelOpen = isOpen;
            UpdateStorageInfo();
        }

        public static bool IsTileInited()
        {
            int value = 0;
            if (TryGet(TileBeInitedKey, out value))
            {
                return value == 1;
            }

            return false;
        }

        public static void SetTileInited()
        {
            Add(TileBeInitedKey, "1");
        }

        public static void Add(string key, string value)
        {
            _localSetting.Values[key] = value;
        }

        public static void AddObject(string key, object value)
        {
            _localSetting.Values[key] = JsonSerializer.Serialize(value);
        }

        public static bool TryGet(string key, out string value)
        {
            if(_localSetting.Values.ContainsKey(key))
            {
                value = _localSetting.Values[key].ToString();
                return true;
            }

            value = null;
            return false;
        }

        public static bool TryGet(string key, out int value)
        {
            if (_localSetting.Values.ContainsKey(key))
            {
                bool ret = int.TryParse(_localSetting.Values[key].ToString(), out value);
                return ret;
            }

            value = -1;
            return false;
        }

        public static bool TryGetJsonObj<T>(string key, out T value) where T : class
        {
            if (_localSetting.Values.ContainsKey(key))
            {
                try {
                    var content = _localSetting.Values[key].ToString();
                    value = JsonSerializer.Deserialize<T>(content);
                }
                catch(Exception)
                {
                    value = default(T);
                    return false;
                }
                return true;
            }

            value = default(T);
            return false;
        }

        public static void Remove(string key)
        {
            if(_localSetting.Values.ContainsKey(key))
            {
                _localSetting.Values.Remove(key);
            }
        }
    }
}
