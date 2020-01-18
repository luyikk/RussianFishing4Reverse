using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilterRussianFishingUploadInfo
{
    public static class FilterRussianFishing
    {
        public static List<string> FilterProcessName { get; set; } = new List<string>();

        static FilterRussianFishing()
        {
            if (System.IO.File.Exists("./fiter.txt"))            
                FilterProcessName.AddRange(System.IO.File.ReadAllLines("./fiter.txt", Encoding.Default));              
            
        }

        public static string Filter(string queryP)
        {
            /// get MachineGuid and set proxy 
            /// It's a very stupid way

            //  var start = Marshal.PtrToStringUni(kkmheohgckn());

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(queryP));
            var info = Newtonsoft.Json.JsonConvert.DeserializeObject<FilterUp>(json);

            info.dll.hash = "74ECB20B0CEA4D490AB743AA4B52D2B61FF5DD8C".ToLower();
            info.dll.trust = true;

            foreach (var item in info.self.mods)
            {
                if (item.path.IndexOf("Assembly-CSharp", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    item.hash = "74ECB20B0CEA4D490AB743AA4B52D2B61FF5DD8C".ToLower();
                    item.trust = true;
                }
                else if (item.path.IndexOf("Newtonsoft.Json", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    item.hash = "5462419339271A1F948F4A51D01019A72127F9D8".ToLower();
                    item.trust = true;
                }
            }

            info.self.mods.RemoveAll(p => p.path.IndexOf("FilterRussianFishingUploadInfo", StringComparison.OrdinalIgnoreCase) != -1);
            info.vm = 0;

            foreach (var fiter in FilterProcessName)            
                info.all.RemoveAll(p => p.name.IndexOf(fiter, StringComparison.OrdinalIgnoreCase) != -1 || p.path.IndexOf(fiter, StringComparison.OrdinalIgnoreCase) != -1);            

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(info)));
        }


        public class FilterUp
        {
            public List<ProcessInfo> all { get; set; }
            public dllinfo dll { get; set; }
            public List<dllinfo> dlls { get; set; }
            public List<int> h { get; set; }
            public ProcessInfo parent { get; set; }
            public selfinfo self { get; set; }
            public int vm { get; set; }
        }


        public class selfinfo
        {
            public string capt { get; set; }
            public string cmd { get; set; }
            public string hash { get; set; }
            public List<dllinfo> mods { get; set; }
            public string name { get; set; }
            public string path { get; set; }
            public int pid { get; set; }
            public int ppid { get; set; }
            public bool trust { get; set; }
        }

        public class dllinfo
        {
            public string hash { get; set; }
            public string path { get; set; }
            public bool trust { get; set; }
        }

        public class ProcessInfo
        {
            public string capt { get; set; }
            public string cmd { get; set; }
            public string hash { get; set; }
            public string name { get; set; }
            public string path { get; set; }
            public int pid { get; set; }
            public int ppid { get; set; }
            public bool trust { get; set; }
        }
    }
}
