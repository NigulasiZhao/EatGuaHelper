using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace EatGuaHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime? TimerFlag = null;
            while (true)
            {
                if (TimerFlag == null || TimerFlag < DateTime.Now)
                {
                    using (WebClient webClient = new WebClient())
                    {
                        #region 新浪
                        //新浪热搜接口
                        string SinaUrl = "https://s.weibo.com/ajax/jsonp/gettopsug";
                        //调用接口
                        string SinaResponseData = Encoding.UTF8.GetString(webClient.DownloadData(SinaUrl));
                        //处理结果
                        var SinaResult = JsonConvert.DeserializeObject<dynamic>(SinaResponseData.Replace("try{window.&({\"data\":", "").Replace("})}catch(e){}", ""));
                        using (SpeechSynthesizer reader = new SpeechSynthesizer())
                        {
                            string SinaText = string.Empty;
                            for (int i = 0; i < SinaResult.list.Count; i++)
                            {
                                SinaText = "新浪热搜第" + (i + 1) + "名:" + SinaResult.list[i].note.ToString() + ";";
                                Console.WriteLine(DateTime.Now + SinaText);
                                reader.Speak(SinaText);
                            }
                            reader.Dispose();
                        }
                        #endregion
                        #region 知乎热榜
                        //知乎热榜接口
                        string ZhihuUrl = "https://www.zhihu.com/api/v3/feed/topstory/hot-lists/total?limit=50&desktop=true";
                        //调用接口
                        string ZhihuResponseData = Encoding.UTF8.GetString(webClient.DownloadData(ZhihuUrl));
                        //处理结果
                        var ZhihuResult = JsonConvert.DeserializeObject<dynamic>(ZhihuResponseData);
                        using (SpeechSynthesizer reader = new SpeechSynthesizer())
                        {
                            string ZhihuText = string.Empty;
                            for (int i = 0; i < 10; i++)
                            {
                                ZhihuText = "知乎热榜第" + (i + 1) + "名:" + ZhihuResult.data[i].target.title.ToString() + ";";
                                Console.WriteLine(DateTime.Now + ZhihuText);
                                reader.Speak(ZhihuText);
                            }
                            reader.Dispose();
                        }
                        #endregion
                    }
                    Console.WriteLine("----------------------------------截止时间:" + DateTime.Now + "----------------------------------");
                    TimerFlag = DateTime.Now.AddMinutes(30);
                }

            }
        }
    }
}
