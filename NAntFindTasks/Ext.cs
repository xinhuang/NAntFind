using System.Collections.Generic;
using System.Xml;
using System;

namespace NAntFind
{
    delegate TRet Func<TArg, TRet>(TArg value);

    static class Ext
    {
        public static string GetAttributeValue(XmlNode self, string name)
        {
            if (self.Attributes == null || self.Attributes.Count == 0)
                return string.Empty;
            if (self.Attributes[name] == null)
                return string.Empty;
            return self.Attributes[name].Value;
        }

        public static bool IsNullOrWhiteSpace(string str)
        {
            return str == null || string.IsNullOrEmpty(str.Trim());
        }

        public static TElement First<TElement>(IEnumerable<TElement> collection, Func<TElement, bool> prediction)
        {
            foreach (var obj in collection)
            {
                if (prediction(obj))
                    return obj;
            }
            return default(TElement);
        }

        public static List<TElement> Where<TElement>(List<TElement> collection, Func<TElement, bool> prediction)
        {
            var result = new List<TElement>();
            foreach (var obj in collection)
            {
                if (prediction(obj))
                    result.Add(obj);
            }
            return result;
        }

        public static bool All<TElement>(IEnumerable<TElement> collection, Func<TElement, bool> prediction)
        {
            foreach (var obj in collection)
            {
                if (!prediction(obj))
                    return false;
            }
            return true;
        }

        public static IEnumerable<TElement> Take<TElement>(List<TElement> collection, int count)
        {
            for (int i = 0; i < collection.Count && i < count; ++i)
                yield return collection[i];
        }

        public static TResultElement[] Take<TElement, TResultElement>(List<TElement> collection, int count, Func<TElement, TResultElement> func)
        {
            var result = new List<TResultElement>();
            foreach (var element in Take(collection, count))
            {
                result.Add(func(element));
            }
            return result.ToArray();
        }
    }
}