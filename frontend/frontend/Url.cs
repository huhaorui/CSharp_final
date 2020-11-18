using System;
using System.Collections.Generic;
using System.Text;

namespace frontend
{
    static class Url
    {
        public static string Header = "https://csharp.nas.huhaorui.com:8888";
        //public readonly static string Header = "http://localhost:5000";
        public static readonly string LoginUrl = "/api/user/login";
        public static readonly string RegisterUrl = "/api/user/register";
        public static readonly string GetDeskListUrl = "/api/desk/list";
        public static readonly string EnterDesk = "/api/desk/enter";
        public static readonly string status = "/api/game/status";
        public static readonly string press = "/api/game/press";
        public static readonly string ready = "/api/game/ready";
    }
}
