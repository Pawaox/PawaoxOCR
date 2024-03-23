using PawaoxOCRWPF.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.Helpers
{
    public static class MessageBroker
    {
        private static Dictionary<Type, List<object>> _dicRegistry = new Dictionary<Type, List<object>>();

        public static void Register<T>(Action<T> act) where T : BaseMessage
        {
            if (act == null)
                return;

            Type key = typeof(T);
            Init(key);
            _dicRegistry[key].Add(act);
        }
        public static void Remove<T>(Action<T> act) where T : BaseMessage
        {
            if (act == null)
                return;

            Type key = typeof(T);
            Init(key);
            if (_dicRegistry.TryGetValue(key, out List<object> lstReg))
                lstReg?.Remove(act);
        }

        public static void Send<T>(T msg) where T : BaseMessage
        {
            if (msg == null)
                return;

            Type key = msg.GetType();
            if (_dicRegistry != null && _dicRegistry.TryGetValue(key, out List<object> lst))
            {
                if (lst != null)
                {
                    foreach (object obj in lst)
                    {
                        Action<T> act = (Action<T>)obj;
                        act?.Invoke(msg);
                    }
                }
            }
        }

        #region Internal
        private static void Init(Type key)
        {
            if (_dicRegistry == null)
                _dicRegistry = new Dictionary<Type, List<object>>();

            if (!_dicRegistry.ContainsKey(key))
                _dicRegistry.Add(key, new List<object>());
        }
        #endregion
    }
}
