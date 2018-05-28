﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MangoWebPool.Functions
{
    public static class Dtl
    {
        private static DataTools d;

        static Dtl()
        {
            d = new DataTools();
        }

        public static string convert_pre_event2(string pre_event)
        {
            pre_event = pre_event ?? "";
            pre_event = pre_event.Trim();
            if (pre_event.Length > 7)
            {
                return d.Convert2PreEvent2(pre_event);
            }
            else
            {
                return null;
            }
        }

        public static string convert_pre_event0(string pre_event2)
        {
            pre_event2 = pre_event2 ?? "";
            pre_event2 = pre_event2.Trim();
            if (pre_event2.Length == 7)
            {
                return d.Convert2PreEvent(pre_event2);
            }
            else if (pre_event2.Length > 7)
            {
                return d.Convert2PreEvent(d.Convert2PreEvent2(pre_event2));
            }
            else
            {
                return null;
            }
        }

        public static decimal? parse_dec(object n)
        {
            return d.ObjToDecimal(n);
        }

        public static int? parse_int(object n)
        {
            return d.ObjToInt(n);
        }

        public static DateTime? parse_date(object date)
        {
            return d.ObjToDateTime(date);
        }

        public static string json_stringify(object o)
        {
            var json = "";
            try
            {
                json = JsonConvert.SerializeObject(o);
            }
            catch { }
            return json;
        }

        public static string json_request()
        {
            var json = "";
            try
            {
                json = d.JsonRequest();
            }
            catch { }
            return json;
        }

        private static bool IsGenericList(this object o)
        {
            var oType = o.GetType();
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)));
        }

        private static bool CheckIfAnonymousType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        public static T json_to_object<T>(string json, T obj) where T : class
        {
            var cc = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                Converters = new[] { new JsonCoverter1() }
            });
            return cc;
        }

        public static string thai_baht_text(decimal? amount)
        {
            return d.ThaiBathText(amount);
        }

        private class JsonCoverter1 : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                var _types = new[] {
                typeof(string),
                typeof(int),
                typeof(int?),
                typeof(decimal),
                typeof(decimal?),
                typeof(double),
                typeof(double?),
                typeof(DateTime),
                typeof(DateTime?),
                //typeof(bool?),
                //typeof(bool),
            };
                return _types.Any(x => x == objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var val = reader.Value?.ToString();
                if (objectType == typeof(string))
                {
                    return val;
                }
                else if (objectType == typeof(decimal?) || objectType == typeof(decimal))
                {
                    return objectType == typeof(decimal?) ? Dtl.parse_dec(val) : Dtl.parse_dec(val) ?? 0;
                }
                else if (objectType == typeof(double?) || objectType == typeof(double))
                {
                    return objectType == typeof(double?) ? (double?)Dtl.parse_dec(val) : (double?)Dtl.parse_dec(val) ?? 0;
                }
                else if (objectType == typeof(int?) || objectType == typeof(int))
                {
                    return objectType == typeof(int?) ? Dtl.parse_int(val) : Dtl.parse_int(val) ?? 0;
                }
                else if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
                {
                    return objectType == typeof(DateTime?) ? Dtl.parse_date(val) : Dtl.parse_date(val) ?? DateTime.Now;
                }
                else
                    return null;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }

    internal class DataTools
    {
        public DataTools()
        {
        }

        public List<object> JsonToModels(string JsonString, object ModelClass)
        {
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

            try
            {
                dataList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(JsonString);
            }
            catch
            {
            }

            if (dataList.Count == 0)
            {
                try
                {
                    dataList.Add(JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonString));
                }
                catch
                {
                }
            }

            return DictionaryToModel(dataList, ModelClass);
        }

        public List<object> DictionaryToModel(List<Dictionary<string, object>> dataList, object ModelClass)
        {
            var mList = new List<object>();
            foreach (var data in dataList)
            {
                var m = Activator.CreateInstance(ModelClass.GetType());
                foreach (var prop in (m.GetType()).GetProperties())
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        string val = data.ContainsKey(prop.Name) ? data[prop.Name]?.ToString() : null;
                        prop.SetValue(m, val);
                    }
                    else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                    {
                        decimal? val = null;
                        if (data.ContainsKey(prop.Name))
                        {
                            val = ObjToDecimal(data[prop.Name]);
                        }
                        prop.SetValue(m, val);
                    }
                    else if (prop.GetType() == typeof(double) || prop.GetType() == typeof(double?))
                    {
                        double? val = null;
                        if (data.ContainsKey(prop.Name)) val = (double?)ObjToDecimal(data[prop.Name]);
                        prop.SetValue(m, val);
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                    {
                        int? val = null;
                        if (data.ContainsKey(prop.Name)) val = ObjToInt(data[prop.Name]);
                        prop.SetValue(m, val);
                    }
                    else if (prop.PropertyType == typeof(DateTime?))
                    {
                        string dateStr = data.ContainsKey(prop.Name) && data[prop.Name] != null ? data[prop.Name].ToString() : "";
                        prop.SetValue(m, this.ObjToDateTime(dateStr));
                    }
                    else if (prop.PropertyType == typeof(TimeSpan?))
                    {
                        TimeSpan? val = null;
                        string dateStr = data.ContainsKey(prop.Name) && data[prop.Name] != null ? data[prop.Name].ToString() : "";
                        if (dateStr != "")
                        {
                            DateTime dt;
                            TimeSpan ts;
                            if (DateTime.TryParseExact(dateStr,
                                                   "HH:mm:ss",
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None,
                                                   out dt))
                            {
                                dt.ToLocalTime();
                                val = dt.TimeOfDay;
                            }
                            else if (TimeSpan.TryParseExact(dateStr,
                                                 "HH:mm:ss",
                                                 CultureInfo.InvariantCulture,
                                                 TimeSpanStyles.None,
                                                 out ts))
                            {
                                val = ts;
                            }
                        }
                        prop.SetValue(m, val);
                    }
                }
                mList.Add(m);
            }
            return mList;
        }

        public List<object> DictionaryToModel(Dictionary<string, object> dataList, object ModelClass)
        {
            var o = new List<Dictionary<string, object>>();
            o.Add(dataList);
            return DictionaryToModel(o, ModelClass);
        }

        public string JsonRequest()
        {
            Stream req = HttpContext.Current.Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            var outp = new StreamReader(req).ReadToEnd();
            return outp;
        }

        public object PostToModel(NameValueCollection Form, object m)
        {
            m = Activator.CreateInstance(m.GetType());

            foreach (var prop in (m.GetType()).GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    string val = null;
                    if (Form[prop.Name] != null && Form[prop.Name] != "") val = Form[prop.Name].Trim();
                    prop.SetValue(m, val);
                }
                else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    decimal? val = null;

                    val = ObjToDecimal(Form[prop.Name] ?? "");

                    prop.SetValue(m, val);
                }
                else if (prop.GetType() == typeof(double) || prop.GetType() == typeof(double?))
                {
                    double? val = null;
                    val = (double?)ObjToDecimal(Form[prop.Name] ?? "");
                    prop.SetValue(m, val);
                }
                else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                {
                    int? val = null;
                    val = ObjToInt(Form[prop.Name] ?? "");
                    prop.SetValue(m, val);
                }
                else if (prop.PropertyType == typeof(TimeSpan?))
                {
                    TimeSpan? val = null;
                    string dateStr = Form[prop.Name] != null ? Form[prop.Name].ToString() : "";
                    if (dateStr != "")
                    {
                        DateTime dt;
                        TimeSpan ts;
                        if (DateTime.TryParseExact(dateStr,
                                               "HH:mm:ss",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out dt))
                        {
                            dt.ToLocalTime();
                            val = dt.TimeOfDay;
                        }
                        else if (TimeSpan.TryParseExact(dateStr,
                                             "HH:mm:ss",
                                             CultureInfo.InvariantCulture,
                                             TimeSpanStyles.None,
                                             out ts))
                        {
                            val = ts;
                        }
                    }
                    prop.SetValue(m, val);
                }
                else
                {
                    prop.SetValue(m, null);
                }
            }

            return m;
        }

        public string Compress(string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return Convert.ToBase64String(mso.ToArray());
            }
        }

        public string Decompress(string s)
        {
            var bytes = Convert.FromBase64String(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }

        public NameValueCollection GetFormData()
        {
            NameValueCollection data = new NameValueCollection();

            foreach (string key in HttpContext.Current.Request.Form.Keys)
            {
                data.Add(key, HttpContext.Current.Request.Form[key]);
            }

            foreach (string key in HttpContext.Current.Request.QueryString.Keys)
            {
                if (data.AllKeys.Contains(key))
                {
                    data[key] = HttpContext.Current.Request.QueryString[key];
                }
                else
                {
                    data.Add(key, HttpContext.Current.Request.QueryString[key]);
                }
            }

            return data;
        }

        public List<Dictionary<string, object>> ListModelsToDictionary(dynamic listModel)
        {
            var table = new List<Dictionary<string, object>>();

            if (listModel.Count == 0) return table;

            var m = listModel[0];

            foreach (var row in listModel)
            {
                var rowObject = new Dictionary<string, object>();
                foreach (var prop in m.GetType().GetProperties())
                {
                    rowObject.Add(prop.Name, prop.GetValue(row));
                }
                table.Add(rowObject);
            }

            return table;
        }

        public NameValueCollection ModelToPostData(object m)
        {
            NameValueCollection postData = new NameValueCollection();
            foreach (var prop in m.GetType().GetProperties())
            {
                if (prop.GetValue(m) == null || string.IsNullOrEmpty(prop.GetValue(m).ToString())) continue;
                if (prop.GetType() == typeof(Nullable<DateTime>))
                {
                    postData.Add(prop.Name, ((DateTime)prop.GetValue(m)).ToString("o"));
                }
                else
                {
                    postData.Add(prop.Name, prop.GetValue(m).ToString().Trim());
                }
            }
            return postData;
        }

        public int? ObjToInt(object o)
        {
            if (o == null)
            {
                return null;
            }
            var D = ObjToDecimal(o);
            return D == null ? (int?)null : (int?)D;
        }

        public decimal? ObjToDecimal(object o)
        {
            if (o == null) return null;
            decimal? D = null;
            try
            {
                D = Convert.ToDecimal(o);
            }
            catch {; }

            if (D == null)
            {
                try
                {
                    D = Decimal.Parse(o.ToString().Replace(" ", "").Replace(",", "").Trim());
                }
                catch {; }
            }
            return D == null ? D : Math.Round((decimal)D, 4);
        }

        public DateTime? ObjToDateTime(object x)
        {
            DateTime dt;
            if (x == null) return null;
            if (DateTime.TryParseExact(x.ToString(),
                                               "d/M/yyyy",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out dt) || DateTime.TryParseExact(x.ToString(),
                                               "dd/MM/yyyy",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out dt) ||
                                               DateTime.TryParseExact(x.ToString(),
                                               "dd/MM/yyyy HH:mm:ss",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out dt) ||
                                               DateTime.TryParseExact(x.ToString(),
                                               "dd/MM/yyyy HH:mm",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out dt
                                               ) || DateTime.TryParse(x.ToString(), out dt))
            {
                return dt;
            }

            return null;
        }

        public string Convert2PreEvent2(string pre_event)
        {
            if (pre_event != null && pre_event.Length > 6)
            {
                var x = pre_event;
                return x.Substring((x.Length - 4), 4) + x.Substring(0, 3);
            }
            else
                return "";
        }

        public string Convert2PreEvent(string pre_event2)
        {
            if (pre_event2.Length > 5)
            {
                var x = pre_event2;
                return x.Substring(x.Length - 3, 3) + "0" + x.Substring(0, 4);
            }
            else
                return "";
        }

        private string StringToCSVCell(string str, Type obj)
        {
            if (str == null) str = "";
            str = str.Trim();
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            //if (str.IndexOf("0") == 0) {
            //    str = "=\"" + str + "\"";
            //}
            return str;
        }

        public string CsvFromList(dynamic x)
        {
            string outp = "";
            if (x != null && x.Count > 0)
            {
                foreach (object n in x)
                {
                    outp += string.Join(",", n.GetType().GetProperties().ToList().Select(y => StringToCSVCell(y.GetValue(n)?.ToString() ?? "", y.GetType())).ToArray()) + System.Environment.NewLine;
                }
            }

            return outp.Trim();
        }

        public dynamic MergeObject(object item1, object item2)
        {
            if (item1 == null || item2 == null)
                return item1 ?? item2 ?? new ExpandoObject();

            dynamic expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>;
            foreach (System.Reflection.PropertyInfo fi in item1.GetType().GetProperties())
            {
                result[fi.Name] = fi.GetValue(item1, null);
            }
            foreach (System.Reflection.PropertyInfo fi in item2.GetType().GetProperties())
            {
                result[fi.Name] = fi.GetValue(item2, null);
            }
            return result;
        }

        public string ConvertToVirtualPath(string RealPath)
        {
            string vPath = "";
            try
            {
                vPath = System.IO.Path.GetFileName(RealPath);
                vPath = new UrlHelper(HttpContext.Current.Request.RequestContext).Content("~/file/load?filename=") + vPath;
            }
            catch { }
            return vPath;
        }

        public string ThaiBathText(decimal? amount)
        {
            if (amount == 0)
            {
                return " - ";
            }

            amount = amount ?? 0;
            var TxtNumArr = new string[] { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" }.ToList();
            var TxtDigitArr = new string[] { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน" }.ToList();

            amount = Math.Round((decimal)amount, 2);

            string[] amount_text = amount?.ToString("0.00").Split('.');
            string bath = amount_text.First();
            string stang = amount_text.Last();

            int digit = -1;
            int ml = -1;

            List<string> bathText = new List<string>();

            foreach (var x in bath.ToCharArray().Reverse())
            {
                digit++;
                if (x == '-')
                {
                    bathText.Add("ลบ");
                }
                else if (new System.Text.RegularExpressions.Regex("[0-9]").IsMatch(x.ToString()))
                {
                    var t = TxtNumArr[int.Parse(x.ToString())];

                    var d = digit % 6;

                    if (x == '0')
                    {
                        t = "";
                    }

                    if (d == 0)
                    {
                        ml++;
                        for (int i = 1; i <= ml; i++)
                        {
                            t += "ล้าน";
                        }
                    }
                    else if (x != '0')
                    {
                        t += TxtDigitArr[d];
                    }

                    bathText.Add(t);
                }
            }
            bathText.Reverse();
            bathText = bathText.Where(x => !string.IsNullOrEmpty(x)).ToList();

            string b = string.Join("", bathText) + "บาท";
            if (b == "ลบบาท") b = "ลบ";
            b = b.Replace("หนึ่งสิบ", "สิบ");
            b = b.Replace("สองสิบ", "ยี่สิบ");
            b = b.Replace("สิบหนึ่ง", "สิบเอ็ด");

            b = bath == "0" ? "ศูนย์บาท" : b;

            List<string> stangText = new List<string>();
            digit = -1;
            foreach (var x in stang.ToCharArray().Reverse())
            {
                digit++;
                var d = digit % 2;
                if (new System.Text.RegularExpressions.Regex("[0-9]").IsMatch(x.ToString()))
                {
                    var t = TxtNumArr[int.Parse(x.ToString())];
                    if (x == '0')
                    {
                        t = "";
                    }
                    if (x != '0')
                    {
                        t += TxtDigitArr[d];
                    }
                    stangText.Add(t);
                }
            }
            stangText.Reverse();
            stangText = stangText.Where(x => !string.IsNullOrEmpty(x)).ToList();
            string s = stangText.Count > 0 ? string.Join("", stangText) + "สตางค์" : "ถ้วน";
            s = s.Replace("หนึ่งสิบ", "สิบ");
            s = s.Replace("สองสิบ", "ยี่สิบ");
            s = s.Replace("สิบหนึ่ง", "สิบเอ็ด");

            return b + s;
        }

        public string ThaiLandText(decimal? land_space)
        {
            if (land_space == 0)
            {
                return " - ";
            }

            land_space = land_space ?? 0;
            var TxtNumArr = new string[] { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" }.ToList();
            var TxtDigitArr = new string[] { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน" }.ToList();

            land_space = Math.Round((decimal)land_space, 2);

            string[] amount_text = land_space?.ToString("0.00").Split('.');
            string bath = amount_text.First();
            string stang = amount_text.Last();

            int digit = -1;
            int ml = -1;

            List<string> landText = new List<string>();

            foreach (var x in bath.ToCharArray().Reverse())
            {
                digit++;
                if (x == '-')
                {
                    landText.Add("ลบ");
                }
                else if (new System.Text.RegularExpressions.Regex("[0-9]").IsMatch(x.ToString()))
                {
                    var t = TxtNumArr[int.Parse(x.ToString())];

                    var d = digit % 6;

                    if (x == '0')
                    {
                        t = "";
                    }

                    if (d == 0)
                    {
                        ml++;
                        for (int i = 1; i <= ml; i++)
                        {
                            t += "ล้าน";
                        }
                    }
                    else if (x != '0')
                    {
                        t += TxtDigitArr[d];
                    }

                    landText.Add(t);
                }
            }
            landText.Reverse();
            landText = landText.Where(x => !string.IsNullOrEmpty(x)).ToList();

            string b = string.Join("", landText) + "ตารางวา";
            if (b == "ลบบาท") b = "ลบ";
            b = b.Replace("หนึ่งสิบ", "สิบ");
            b = b.Replace("สองสิบ", "ยี่สิบ");
            b = b.Replace("สิบหนึ่ง", "สิบเอ็ด");

            b = bath == "0" ? "ศูนย์ตารางวา" : b;

            List<string> stangText = new List<string>();
            digit = -1;
            foreach (var x in stang.ToCharArray().Reverse())
            {
                digit++;
                var d = digit % 2;
                if (new System.Text.RegularExpressions.Regex("[0-9]").IsMatch(x.ToString()))
                {
                    var t = TxtNumArr[int.Parse(x.ToString())];
                    if (x == '0')
                    {
                        t = "";
                    }
                    if (x != '0')
                    {
                        t += TxtDigitArr[d];
                    }
                    stangText.Add(t);
                }
            }

            return b;
        }

        public int WeekOfYear(DateTime time, DayOfWeek day)
        {
            int w = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, day);
            if (time.Month == 1 && w >= 52)
            {
                w = 0;
            }
            return w;
        }

        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public class LandConvert
        {
            //private decimal land_space{ get; set; } = 0;
            public decimal sqr { get; set; } = 0;

            public int work { get; set; } = 0;
            public int rai { get; set; } = 0;

            public LandConvert(decimal land_space)
            {
                rai = (int)(land_space / 400);
                work = (int)(((land_space / 400) - rai) * 4);
                sqr = (decimal)(land_space % 100M);
            }
        }

        public bool FileUpload(HttpPostedFileBase file, string path, string accept, bool create_sub_directory, ref string error_message, ref FileInfo file_info, bool img_resize = false)
        {
            file_info = null;
            if (file == null || file.ContentLength == 0)
            {
                error_message = "No file upload";
                return false;
            }

            if (!accept.Split(',').Select(x => (x ?? "").Trim().ToLower()).Where(x => !string.IsNullOrEmpty(x)).ToArray().Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                error_message = "File not support";
                return false;
            }

            try
            {
                DateTime d = DateTime.Now;
                if (create_sub_directory)
                {
                    path = (path + $"/{d.ToString("yyyyMMdd")}/");
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string ext = Path.GetExtension(file.FileName).ToLower();

                var file_name = (path + d.ToString("yyyyMMddHHmmss_") + Path.GetRandomFileName() + ext);
                var file_name2 = "";
                file.SaveAs(file_name);
                if (img_resize)
                {
                    if (!file.ContentType.ToLower().StartsWith("image/"))
                    {
                        throw new Exception("File must be an image file");
                    }

                    using (System.Drawing.Image objGraphic = System.Drawing.Image.FromFile(file_name))
                    {
                        if (objGraphic.Width > 800 || objGraphic.Height > 800)
                        {
                            int new_w = 1;
                            int new_h = 1;

                            if (objGraphic.Width > objGraphic.Height)
                            {
                                new_w = 800;
                                new_h = (int)Math.Ceiling((1.0 * objGraphic.Height * new_w) / (objGraphic.Width == 0 ? 1 : objGraphic.Width));
                            }
                            else
                            {
                                new_h = 800;
                                new_w = (int)Math.Ceiling((1.0 * objGraphic.Width * new_h) / (objGraphic.Height == 0 ? 1 : objGraphic.Height));
                            }

                            file_name2 = path = (path + d.ToString("yyyyMMddHHmmss_") + Path.GetRandomFileName() + ".jpg");
                            using (var objBitmap = new System.Drawing.Bitmap(objGraphic, new_w, new_h))
                            {
                                objBitmap.Save(file_name2, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(file_name2))
                    {
                        File.Delete(file_name);
                        file_name = file_name2;
                    }
                }

                file_info = new FileInfo(file_name);
                //MimeMapping.GetMimeMapping
            }
            catch (Exception ex)
            {
                error_message = ex.Message;
                return false;
            }

            return true;
        }
    }
}