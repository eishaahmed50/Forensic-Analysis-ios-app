using System;
using System.IO;
using System.Collections.Generic;
using Claunia.PropertyList;
namespace PlistFileReadingConsole
{
    class DataExtractor
    {
        private DataExtractor() { }
        public static Dictionary<string, object> GetDataFronPList(Stream fileStream) {
            var data = PropertyListParser.Parse(fileStream);
            return DictHandler(data);
        }
        public static Dictionary<string, object> GetDataFronPList(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var data = PropertyListParser.Parse(fs);
            return DictHandler(data);
        }
        
        private static Dictionary<string, object> DictHandler(NSObject data)
        {
            Dictionary<string, object> nativeDict = new Dictionary<string, object>();
            if (data?.GetType() == typeof(NSDictionary))
            {
                Dictionary<string, NSObject> dict = ((NSDictionary)data).GetDictionary();
                foreach (var item in dict)
                {
                    nativeDict.Add(item.Key, item.Value);
                    if (item.Value.GetType() == typeof(NSDictionary))
                    {
                        nativeDict[item.Key] = DictHandler(item.Value);
                    }
                    else if (item.Value.GetType() == typeof(NSArray))
                    {
                        nativeDict[item.Key] = arrayHandler(item.Value);
                    }
                    else if (item.Value.GetType() == typeof(NSDictionary))
                    {
                        nativeDict[item.Key] = DictHandler(item.Value);
                    }
                    else if (item.Value.GetType() == typeof(NSDictionary))
                    {
                        nativeDict[item.Key] = DictHandler(item.Value);
                    }
                    else if (item.Value.GetType() == typeof(NSDictionary))
                    {
                        nativeDict[item.Key] = DictHandler(item.Value);
                    }
                    else
                    {
                        nativeDict[item.Key] = ((NSObject)item.Value).ToObject();
                    }
                }
            }

            return nativeDict;
        }
        private static object[] arrayHandler(NSObject data)
        {
            NSArray arr = (NSArray)data;
            object[] nativeArray = new object[arr.Count];
            foreach (var element in arr)
            {
                nativeArray[arr.IndexOf(element)] = element;
                if (element.GetType() == typeof(NSDictionary))
                {
                    nativeArray[arr.IndexOf(element)] = DictHandler(element);
                }
                else if (element.GetType() == typeof(NSArray))
                {
                    nativeArray[arr.IndexOf(element)] = arrayHandler((NSArray)element);
                }
                else if (element.GetType() == typeof(NSSet))
                {
                    nativeArray[arr.IndexOf(element)] = arrayHandler((NSArray)element);
                }
                else
                {
                    nativeArray[arr.IndexOf(element)] = ((NSObject)element).ToObject();
                }
            }
            return nativeArray;
        }
        private static List<object> setHandler(NSObject data)
        {
            NSSet set = (NSSet)data;
            List<object> nativeList = new List<object>();
            foreach (var element in set)
            {
                nativeList.Add(element);
                if (element.GetType() == typeof(NSDictionary))
                {
                   nativeList[nativeList.IndexOf(element)] = DictHandler((NSDictionary)element);
                }
                else if (element.GetType() == typeof(NSArray))
                {
                    nativeList[nativeList.IndexOf(element)] = arrayHandler((NSArray)element);
                }
                else if (element.GetType() == typeof(NSSet))
                {
                    nativeList[nativeList.IndexOf(element)] = arrayHandler((NSArray)element);
                }
                else
                {
                    nativeList[nativeList.IndexOf(element)] = ((NSObject)element).ToObject();
                }
            }
            return nativeList;
        }
    }
}
